using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NyanQueue.Core.UiSystem.ScreenSystem.Containers;
using NyanQueue.Core.UiSystem.ScreenSystem.Providers;
using NyanQueue.Core.UiSystem.ScreenSystem.Screens;
using NyanQueue.Core.UiSystem.ScreenSystem.Screens.Models;
using NyanQueue.Core.UiSystem.ScreenSystem.Settings;
using NyanQueue.Core.UiSystem.ScreenSystem.Switching;
using NyanQueue.Core.UiSystem.Utilities.Enums;
using NyanQueue.Core.Utilities.Classes;
using UnityEngine;

namespace NyanQueue.Core.UiSystem.ScreenSystem
{
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private UiContainersManager _uiContainersManager;
        
        private readonly Dictionary<Type, ScreenSettings> _defaultScreenSettings = new();
        private readonly Dictionary<OrderedTypePair, SwitchSettings> _defaultSwitchSettings = new();
        private IPrefabProvider _prefabProvider;
        
        public AbstractScreen CurrentScreen { get; set; }
        public Type PreparingScreenType { get; set; }
        public Type CurrentScreenType => CurrentScreen?.GetType();
        
        public void SetPrefabProvider(IPrefabProvider prefabProvider) => _prefabProvider = prefabProvider;

        public void RegisterDefaultScreenSettings<TScreen>(ScreenSettings screenSettings)
            where TScreen : AbstractScreen
            => _defaultScreenSettings.TryAdd(typeof(TScreen), screenSettings);

        public void RegisterDefaultSwitchSettings<TScreen, TPrevScreen>(SwitchSettings switchSettings)
            where TScreen : AbstractScreen
            where TPrevScreen : AbstractScreen
        {
            var pair = new OrderedTypePair(typeof(TScreen), typeof(TPrevScreen));
            _defaultSwitchSettings.TryAdd(pair, switchSettings);
        }
        
        public async UniTask OpenScreen<TScreen, TModel>()
            where TScreen : InitializableScreen<TModel>
            where TModel : ScreenModel, new()
        {
            var operation = new ScreenOpenOperation<TScreen, TModel>();
            await OpenScreen(operation);
        }

        public async UniTask OpenScreen<TScreen, TModel>(TModel model)
            where TScreen : InitializableScreen<TModel>
            where TModel : ScreenModel, new()
        {
            var operation = new ScreenOpenOperation<TScreen, TModel>();
            operation.SetModel(model);
            await OpenScreen(operation);
        }

        public async UniTask OpenScreen<TScreen, TModel>(ScreenOpenOperation<TScreen, TModel> openOperation)
            where TScreen : InitializableScreen<TModel>
            where TModel : ScreenModel, new()
        {
            if (IsScreenPreparingOrOpen<TScreen>()) return;
            
            var screenType = typeof(TScreen);
            PreparingScreenType = screenType;
            var screenData = await _prefabProvider.ProvideScreen(screenType);
            if (screenData == null)
            {
                Debug.LogError($"[UiManager] Screen {screenType} not found");
                return;
            }

            if (openOperation.Model == null) openOperation.SetModel(new TModel());
            
            var typedScreen = CreateScreenInstance<TScreen, TModel>(screenData);
            var settings = GetFinalScreenSettings(openOperation, screenType);
            typedScreen.SetSettings(settings);
            ProcessScreen(typedScreen);
            
            var overrides = GetFinalSwitchSettings(openOperation, screenType);
            var container = _uiContainersManager.GetContainer(settings.Order);
            container.AddScreen(typedScreen);
            
            await ScreenOpenSequence(typedScreen, openOperation.Model, settings.CloseBehaviour, overrides);
            
            CurrentScreen = typedScreen;
            PreparingScreenType = null;
        }

        private TScreen CreateScreenInstance<TScreen, TModel>(ScreenData screenData)
            where TScreen : InitializableScreen<TModel>
            where TModel : ScreenModel, new()
        {
            var screenInstance = Instantiate(screenData.Screen);
            screenInstance.gameObject.SetActive(false);
            return screenInstance.GetComponent<TScreen>();
        }

        private ScreenSettings GetFinalScreenSettings<TScreen, TModel>(ScreenOpenOperation<TScreen, TModel> openOperation, 
            Type screenType)
            where TScreen : InitializableScreen<TModel> 
            where TModel : ScreenModel, new()
        {
            _defaultScreenSettings.TryGetValue(screenType, out var screenSettings);
            var settings = screenSettings ?? new();
            settings = settings.Merge(openOperation.OrderOverride, openOperation.CloseBehaviourOverride);
            return settings;
        }

        private SwitchSettings GetFinalSwitchSettings<TScreen, TModel>(
            ScreenOpenOperation<TScreen, TModel> openOperation, Type screenType)
            where TScreen : InitializableScreen<TModel> 
            where TModel : ScreenModel, new()
        {
            SwitchSettings overrides = null;
            if (CurrentScreenType != null)
            {
                _defaultSwitchSettings.TryGetValue(new OrderedTypePair(screenType, CurrentScreenType), out var defOverrides);
                overrides = defOverrides ?? new();
            }
            else 
                overrides = new();
            
            if (openOperation.SwitchSettingsOverrides != null) 
                overrides = overrides.Merge(openOperation.SwitchSettingsOverrides);
            
            return overrides;
        }

        private async UniTask ScreenOpenSequence<TScreen, TModel>(TScreen openingScreen, TModel model, 
            CloseBehaviour closeBehaviour, SwitchSettings switchSettings)
            where TScreen : InitializableScreen<TModel>
            where TModel : ScreenModel, new()
        {
            await openingScreen.Initialize(model);

            var finalCloseBehaviour = !switchSettings.OverrideCloseBehavior 
                ? closeBehaviour 
                : switchSettings.PrevScreenCloseBehaviour;
            if (switchSettings.PrevScreenCloseBehaviour is not CloseBehaviour.WithNext)
            {
                if (finalCloseBehaviour is CloseBehaviour.BeforeNext) await CloseScreen(switchSettings.PrevScreenAnimation);
                openingScreen.gameObject.SetActive(true);
                await openingScreen.Open(switchSettings.CurrentScreenAnimation);
                if (finalCloseBehaviour is CloseBehaviour.AfterNext) await CloseScreen(switchSettings.PrevScreenAnimation);
            }
            else
            {
                openingScreen.gameObject.SetActive(true);
                var current = CloseScreen(switchSettings.PrevScreenAnimation);
                var next = openingScreen.Open(switchSettings.CurrentScreenAnimation);
                await UniTask.WhenAll(current, next);
            }
            ClearScreen();
        }
        
        /// <summary>
        /// Use to do some custom stuff, like injecting screen's GO here
        /// </summary>
        /// <param name="screen"></param>
        protected virtual void ProcessScreen(AbstractScreen screen) { }
        
        public bool IsScreenOpen(Type type) => CurrentScreen != null && CurrentScreen.GetType() == type;
        public bool IsScreenOpen<TScreen>() => IsScreenOpen(typeof(TScreen));
        public bool IsScreenPreparing(Type type) => CurrentScreen != null 
                                                    && CurrentScreen.GetType() == PreparingScreenType;
        public bool IsScreenPreparing<TScreen>() => IsScreenPreparing(typeof(TScreen));
        public bool IsScreenPreparingOrOpen(Type type) => IsScreenOpen(type) || IsScreenPreparing(type);
        public bool IsScreenPreparingOrOpen<TScreen>() => IsScreenPreparingOrOpen(typeof(TScreen));

        private async UniTask CloseScreen(string transitionName = "")
        {
            if (CurrentScreen == null) return;
            
            await CurrentScreen.Close(transitionName);
        }

        private void ClearScreen()
        {
            if (CurrentScreen == null) return;
            
            Destroy(CurrentScreen.gameObject);
            CurrentScreen = null;
        }
    }
}
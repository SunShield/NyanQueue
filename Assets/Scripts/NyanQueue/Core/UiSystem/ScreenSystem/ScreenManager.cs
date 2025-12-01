using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NyanQueue.Core.UiSystem.ScreenSystem.Containers;
using NyanQueue.Core.UiSystem.ScreenSystem.Providers;
using NyanQueue.Core.UiSystem.ScreenSystem.Providers.Prefab;
using NyanQueue.Core.UiSystem.ScreenSystem.Screens;
using NyanQueue.Core.UiSystem.ScreenSystem.Screens.Models;
using NyanQueue.Core.UiSystem.Utilities.Classes.Settings;
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
        private IScreenPrefabProvider _prefabProvider;
        
        public AbstractScreen CurrentScreen { get; set; }
        public Type PreparingScreenType { get; set; }
        public Type CurrentScreenType => CurrentScreen?.GetType();
        
        public void SetPrefabProvider(IScreenPrefabProvider prefabProvider) => _prefabProvider = prefabProvider;

        public void RegisterDefaultScreenSettings<TScreen>(ScreenSettings screenSettings)
            where TScreen : AbstractScreen
            => RegisterDefaultScreenSettings(typeof(TScreen), screenSettings);
        
        public void RegisterDefaultScreenSettings(Type screenType, ScreenSettings screenSettings)
            => _defaultScreenSettings.TryAdd(screenType, screenSettings);

        public void RegisterDefaultSwitchSettings<TScreen, TPrevScreen>(SwitchSettings switchSettings)
            where TScreen : AbstractScreen
            where TPrevScreen : AbstractScreen
            => RegisterDefaultSwitchSettings(typeof(TScreen), typeof(TPrevScreen), switchSettings);

        public void RegisterDefaultSwitchSettings(Type screenType, Type prevSceenType, SwitchSettings switchSettings)
        {
            var pair = new OrderedTypePair(screenType, prevSceenType);
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
            var screenPrefab = await _prefabProvider.ProvidePrefab(screenType);
            if (screenPrefab == null)
            {
                Debug.LogError($"[UiManager] Screen {screenType} prefab not found");
                return;
            }

            if (openOperation.Model == null) openOperation.SetModel(new TModel());
            
            var typedScreen = CreateScreenInstance<TScreen, TModel>(screenPrefab);
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

        private TScreen CreateScreenInstance<TScreen, TModel>(AbstractScreen screenPrefab)
            where TScreen : InitializableScreen<TModel>
            where TModel : ScreenModel, new()
        {
            var screenInstance = Instantiate(screenPrefab);
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
                : switchSettings.PrevCloseBehaviour;
            if (switchSettings.PrevCloseBehaviour is not CloseBehaviour.WithNext)
            {
                if (finalCloseBehaviour is CloseBehaviour.BeforeNext) await CloseScreen(switchSettings.PrevAnimation);
                openingScreen.gameObject.SetActive(true);
                await openingScreen.Open(switchSettings.CurAnimation);
                if (finalCloseBehaviour is CloseBehaviour.AfterNext) await CloseScreen(switchSettings.PrevAnimation);
            }
            else
            {
                openingScreen.gameObject.SetActive(true);
                var current = CloseScreen(switchSettings.PrevAnimation);
                var next = openingScreen.Open(switchSettings.CurAnimation);
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
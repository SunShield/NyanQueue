using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NyanQueue.Core.ScreenSystem.Containers;
using NyanQueue.Core.ScreenSystem.Overrides;
using NyanQueue.Core.ScreenSystem.Overrides.Impl;
using NyanQueue.Core.ScreenSystem.Providers;
using NyanQueue.Core.ScreenSystem.Screens;
using NyanQueue.Core.ScreenSystem.Screens.Models;
using NyanQueue.Core.ScreenSystem.Settings;
using NyanQueue.Core.ScreenSystem.Settings.Impl;
using NyanQueue.Core.Utilities.Classes;
using UnityEngine;

namespace NyanQueue.Core.ScreenSystem
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private UiContainersManager _uiContainersManager;
        
        private readonly Dictionary<Type, ScreenSettings> _defaultScreenSettings = new();
        private readonly Dictionary<OrderedTypePair, ScreenOpenOverrides> _defaultOverrides = new();
        private IPrefabProvider _prefabProvider;
        
        public AbstractScreen CurrentScreen { get; set; }
        public Type PreparingScreenType { get; set; }
        public Type CurrentScreenType => CurrentScreen?.GetType();
        
        public void SetPrefabProvider(IPrefabProvider prefabProvider) => _prefabProvider = prefabProvider;

        public void RegisterDefaultScreenSettings<TScreen>(ScreenSettings screenSettings)
            where TScreen : AbstractScreen
        {
            _defaultScreenSettings.TryAdd(typeof(TScreen), new());
            _defaultScreenSettings[typeof(TScreen)].Merge(screenSettings);
        }

        public void RegisterDefaultOverrides<TScreen, TPrevScreen>(ScreenOpenOverrides screenOverrides)
            where TScreen : AbstractScreen
            where TPrevScreen : AbstractScreen
        {
            var pair = new OrderedTypePair(typeof(TScreen), typeof(TPrevScreen));
            _defaultOverrides.TryAdd(pair, new ScreenOpenOverrides());
            _defaultOverrides[pair] = _defaultOverrides[pair].Merge(screenOverrides);
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
            
            var screenInstance = Instantiate(screenData.Screen);
            var typedScreen = screenInstance.GetComponent<TScreen>();
            var settings = GetFinalSettings(openOperation, screenType);
            typedScreen.SetSettings(settings);
            ProcessScreen(typedScreen);
            
            var overrides = GetFinalOverrides(openOperation, screenType);
            var finalOrder = GetFinalOrder(overrides, screenData);
            var container = _uiContainersManager.GetContainer(finalOrder);
            container.AddScreen(typedScreen);
            
            await ScreenOpenSequence(typedScreen, openOperation.Model, overrides);
            
            CurrentScreen = typedScreen;
            PreparingScreenType = null;
        }

        private ScreenSettings GetFinalSettings<TScreen, TModel>(ScreenOpenOperation<TScreen, TModel> openOperation, 
            Type screenType)
            where TScreen : InitializableScreen<TModel> where TModel : ScreenModel, new()
        {
            _defaultScreenSettings.TryGetValue(screenType, out var screenSettings);
            var settings = screenSettings ?? new();
            if (openOperation.ScreenSettings != null) settings = settings.Merge(openOperation.ScreenSettings);
            return settings;
        }

        private ScreenOpenOverrides GetFinalOverrides<TScreen, TModel>(
            ScreenOpenOperation<TScreen, TModel> openOperation, Type screenType)
            where TScreen : InitializableScreen<TModel> where TModel : ScreenModel, new()
        {
            ScreenOpenOverrides overrides = null;
            if (CurrentScreenType != null)
            {
                _defaultOverrides.TryGetValue(new OrderedTypePair(screenType, CurrentScreenType), out var defOverrides);
                overrides = defOverrides ?? new();
            }
            else 
                overrides = new();
            
            if (openOperation.OpenOverrides != null) overrides = overrides.Merge(openOperation.OpenOverrides);
            return overrides;
        }

        private int GetFinalOrder(ScreenOpenOverrides overrides, ScreenData screenData)
        {
            var orderSetting = overrides.Get<OrderOverride>();
            var finalOrder = orderSetting?.Order ?? screenData.DefaultOrder;
            return finalOrder;
        }

        private async UniTask ScreenOpenSequence<TScreen, TModel>(TScreen typedScreen, TModel model
            , ScreenOpenOverrides overrides)
            where TScreen : InitializableScreen<TModel>
            where TModel : ScreenModel, new()
        {
            var closeBehaviour = overrides.Get<PrevScreenCloseBehaviourOverride>()?.CloseBehaviour;
            if (closeBehaviour == null)
            {
                var closeBehaviourSetting = CurrentScreen != null 
                    ? CurrentScreen.Settings.Get<CloseBehaviourSetting>() 
                    : null; 
                closeBehaviour = closeBehaviourSetting?.CloseBehaviour ?? ScreenCloseBehaviour.AfterNext;
            }

            var inTransitionName = "";
            var inTransitionOverride = overrides.Get<CurrentScreenTransitionOverride>();
            if (inTransitionOverride != null) inTransitionName = inTransitionOverride.TransitionName;
            
            var outTransitionName = "";
            var outTransitionOverride = overrides.Get<PrevScreenTransitionOverride>();
            if (outTransitionOverride != null) outTransitionName = outTransitionOverride.TransitionName;

            await typedScreen.Initialize(model);

            if (closeBehaviour is not ScreenCloseBehaviour.WithNext)
            {
                if (closeBehaviour is ScreenCloseBehaviour.BeforeNext) await CloseScreen(outTransitionName);
                await typedScreen.Open(inTransitionName);
                if (closeBehaviour is ScreenCloseBehaviour.AfterNext) await CloseScreen(outTransitionName);
            }
            else
            {
                var current = CloseScreen(outTransitionName);
                var next = typedScreen.Open(inTransitionName);
                await UniTask.WhenAll(current, next);
            }
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

        public async UniTask CloseScreen(string transitionName = "")
        {
            if (CurrentScreen == null) return;
            
            await CurrentScreen.Close(transitionName);
            Destroy(CurrentScreen.gameObject);
            CurrentScreen = null;
        }
    }
}
using System;
using Cysharp.Threading.Tasks;
using NyanQueue.Core.ScreenSystem.Containers;
using NyanQueue.Core.ScreenSystem.Providers;
using NyanQueue.Core.ScreenSystem.Screens;
using NyanQueue.Core.ScreenSystem.Settings;
using NyanQueue.Core.ScreenSystem.Settings.Impl;
using UnityEngine;
using Screen = NyanQueue.Core.ScreenSystem.Screens.Screen;

namespace NyanQueue.Core.ScreenSystem
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private UiContainersManager _uiContainersManager;
        
        private IPrefabProvider _prefabProvider;
        
        public Screen CurrentScreen { get; set; }
        public Type PreparingScreenType { get; set; }
        
        public void SetPrefabProvider(IPrefabProvider prefabProvider) => _prefabProvider = prefabProvider;

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
            var settings = openOperation.ScreenSettings ?? new ScreenSettings();
            typedScreen.SetSettings(settings);
            ProcessScreen(typedScreen);

            var orderSetting = settings.Get<OrderScreenSetting>();
            var finalOrder = orderSetting?.Order ?? screenData.DefaultOrder;
            var container = _uiContainersManager.GetContainer(finalOrder);
            container.AddScreen(typedScreen);
            
            await ScreenOpenSequence(typedScreen, openOperation.Model);
            
            CurrentScreen = typedScreen;
            PreparingScreenType = null;
        }

        private async UniTask ScreenOpenSequence<TScreen, TModel>(TScreen typedScreen, TModel model)
            where TScreen : InitializableScreen<TModel>
            where TModel : ScreenModel, new()
        {
            var closeBehaviourSetting = CurrentScreen != null 
                ? CurrentScreen.Settings.Get<CloseBehaviourSetting>() 
                : null; 
            var closeBehaviour = closeBehaviourSetting?.CloseBehaviour;

            await typedScreen.Initialize(model);

            if (closeBehaviour is not ScreenCloseBehaviour.WithNext)
            {
                if (closeBehaviour is ScreenCloseBehaviour.BeforeNext) await CloseScreen();
                await typedScreen.Open();
                if (closeBehaviour is ScreenCloseBehaviour.AfterNext) await CloseScreen();
            }
            else
            {
                var current = CurrentScreen.Close();
                var next = typedScreen.Open();
                await UniTask.WhenAll(current, next);
            }
        }
        
        /// <summary>
        /// Use to do some custom stuff, like injecting screen's GO here
        /// </summary>
        /// <param name="screen"></param>
        protected virtual void ProcessScreen(Screen screen) { }
        
        public bool IsScreenOpen(Type type) => CurrentScreen != null && CurrentScreen.GetType() == type;
        public bool IsScreenOpen<TScreen>() => IsScreenOpen(typeof(TScreen));
        public bool IsScreenPreparing(Type type) => CurrentScreen != null 
                                                    && CurrentScreen.GetType() == PreparingScreenType;
        public bool IsScreenPreparing<TScreen>() => IsScreenPreparing(typeof(TScreen));
        public bool IsScreenPreparingOrOpen(Type type) => IsScreenOpen(type) || IsScreenPreparing(type);
        public bool IsScreenPreparingOrOpen<TScreen>() => IsScreenPreparingOrOpen(typeof(TScreen));

        public async UniTask CloseScreen()
        {
            await CurrentScreen.Close();
            Destroy(CurrentScreen.gameObject);
            CurrentScreen = null;
        }
    }
}
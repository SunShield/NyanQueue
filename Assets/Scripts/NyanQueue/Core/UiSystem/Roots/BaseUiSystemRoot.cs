using System;
using Cysharp.Threading.Tasks;
using NyanQueue.Core.UiSystem.ScreenSystem;
using NyanQueue.Core.UiSystem.ScreenSystem.Providers.Defaults;
using NyanQueue.Core.UiSystem.ScreenSystem.Providers.Prefab;
using UnityEngine;

namespace NyanQueue.Core.UiSystem.Roots
{
    public abstract class BaseUiSystemRoot : MonoBehaviour
    {
        protected abstract ScreenManager ScreenManager { get; }
        protected abstract IScreenPrefabProvider ScreenPrefabProvider { get; }
        protected abstract IScreenDefaultsProvider ScreenDefaultsProvider { get; }
        
        public bool IsBuilt { get; private set; }

        private async void Awake()
        {
            DontDestroyOnLoad(gameObject);
            await DoAwake();
        }
        
        protected virtual UniTask DoAwake() => UniTask.CompletedTask;

        public async UniTask Build()
        {
            await BuildInternal();
            IsBuilt = true;
        }

        protected virtual async UniTask BuildInternal()
        {
            ScreenManager.SetPrefabProvider(ScreenPrefabProvider);
            await ScreenDefaultsProvider.ProvideDefaults(ScreenManager);
        }
    }
}
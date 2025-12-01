using System;
using Cysharp.Threading.Tasks;
using NyanQueue.Core.UiSystem.ScreenSystem.Screens.Views;
using NyanQueue.Core.UiSystem.Utilities.Classes.Settings;
using UnityEngine;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Screens
{
    public abstract class AbstractScreen : MonoBehaviour
    {
        protected abstract ScreenView BaseView { get; }
        
        public ScreenSettings Settings { get; private set; }
        
        public void SetSettings(ScreenSettings screenSettings) => Settings = screenSettings;

        public virtual async UniTask Open(string animationName = "")
        {
            await PreOpen();
            await BaseView.Open(animationName);
            await PostOpen();
        }
        
        protected virtual UniTask PreOpen() => UniTask.CompletedTask;
        protected virtual UniTask PostOpen() => UniTask.CompletedTask;

        public virtual async UniTask Close(string animationName = "")
        {
            await PreClose();
            await BaseView.Close(animationName);
            await PostClose();
        }
        
        protected virtual UniTask PreClose() => UniTask.CompletedTask;
        protected virtual UniTask PostClose() => UniTask.CompletedTask;
    }
}
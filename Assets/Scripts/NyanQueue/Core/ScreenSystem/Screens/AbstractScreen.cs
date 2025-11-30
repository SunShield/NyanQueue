using Cysharp.Threading.Tasks;
using NyanQueue.Core.ScreenSystem.Screens.Views;
using NyanQueue.Core.ScreenSystem.Settings;
using UnityEngine;

namespace NyanQueue.Core.ScreenSystem.Screens
{
    public abstract class AbstractScreen : MonoBehaviour
    {
        protected abstract ScreenView BaseView { get; }
        
        public ScreenSettings Settings { get; private set; }
        
        public void SetSettings(ScreenSettings screenSettings) => Settings = screenSettings;
        
        public virtual async UniTask Open(string transitionName = "")
        {
            await PreOpen();
            await BaseView.Open(transitionName);
            await PostOpen();
        }
        
        protected virtual UniTask PreOpen() => UniTask.CompletedTask;
        protected virtual UniTask PostOpen() => UniTask.CompletedTask;

        public virtual async UniTask Close(string transitionName = "")
        {
            await PreClose();
            await BaseView.Close(transitionName);
            await PostClose();
        }
        
        protected virtual UniTask PreClose() => UniTask.CompletedTask;
        protected virtual UniTask PostClose() => UniTask.CompletedTask;
    }
}
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NyanQueue.Core.UiSystem.Utilities.Classes.Animations;
using UnityEngine;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Screens.Views
{
    public abstract class ScreenView : MonoBehaviour
    {
        [SerializeField] private List<AbstractAnimation> _openAnimations = new();
        [SerializeField] private List<AbstractAnimation> _closeAnimations = new();
        
        protected AbstractAnimation DefaultOpenAnimation => _openAnimations?.FirstOrDefault();
        protected AbstractAnimation DefaultCloseAnimation => _closeAnimations?.FirstOrDefault();
        
        public virtual async UniTask Open(string animationName = "")
        {
            await PreOpen();
            await WaitForAnimation(animationName, DefaultOpenAnimation, _openAnimations);
            await PostOpen();
        }
        
        protected virtual UniTask PreOpen() => UniTask.CompletedTask;
        protected virtual UniTask PostOpen() => UniTask.CompletedTask;

        public virtual async UniTask Close(string animationName = "")
        {
            await PreClose();
            await WaitForAnimation(animationName, DefaultCloseAnimation, _closeAnimations);
            await PostClose();
        }
        
        protected virtual UniTask PreClose() => UniTask.CompletedTask;
        protected virtual UniTask PostClose() => UniTask.CompletedTask;

        private async UniTask WaitForAnimation(string animationName,
            AbstractAnimation defaultAnimation, List<AbstractAnimation> transitions)
        {
            var transition = GetAnimation(animationName, defaultAnimation, transitions);
            if (transition == null) return;
            await transition.Run();
        }

        private AbstractAnimation GetAnimation(string animationName,
            AbstractAnimation defaultAnimation, List<AbstractAnimation> transitions)
        {
            if (string.IsNullOrEmpty(animationName)) return defaultAnimation;
            
            var transition = transitions.FirstOrDefault(t => t.TransitionName == animationName);
            return transition == null ? defaultAnimation : transition;
        }
    }
}
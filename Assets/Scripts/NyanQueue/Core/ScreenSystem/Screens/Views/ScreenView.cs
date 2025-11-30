using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NyanQueue.Examples.Transitions;
using UnityEngine;

namespace NyanQueue.Core.ScreenSystem.Screens.Views
{
    public abstract class ScreenView : MonoBehaviour
    {
        [SerializeField] private List<AbstractTransition> _openTransitions;
        [SerializeField] private List<AbstractTransition> _closeTransitions;
        
        protected AbstractTransition DefaultOpenTransition => _openTransitions?.FirstOrDefault();
        protected AbstractTransition DefaultCloseTransition => _closeTransitions?.FirstOrDefault();
        
        public virtual async UniTask Open(string transitionName = "")
        {
            await PreOpen();
            await WaitForTransition(transitionName, DefaultOpenTransition, _openTransitions);
            await PostOpen();
        }
        
        protected virtual UniTask PreOpen() => UniTask.CompletedTask;
        protected virtual UniTask PostOpen() => UniTask.CompletedTask;

        public virtual async UniTask Close(string transitionName = "")
        {
            await PreClose();
            await WaitForTransition(transitionName, DefaultCloseTransition, _closeTransitions);
            await PostClose();
        }
        
        protected virtual UniTask PreClose() => UniTask.CompletedTask;
        protected virtual UniTask PostClose() => UniTask.CompletedTask;

        private async UniTask WaitForTransition(string transitionName,
            AbstractTransition defaultTransition, List<AbstractTransition> transitions)
        {
            var transition = GetTransition(transitionName, defaultTransition, transitions);
            if (transition == null) return;
            await transition.Run();
        }

        private AbstractTransition GetTransition(string transitionName,
            AbstractTransition defaultTransition, List<AbstractTransition> transitions)
        {
            if (string.IsNullOrEmpty(transitionName)) return defaultTransition;
            
            var transition = transitions.FirstOrDefault(t => t.TransitionName == transitionName);
            return transition == null ? defaultTransition : transition;
        }
    }
}
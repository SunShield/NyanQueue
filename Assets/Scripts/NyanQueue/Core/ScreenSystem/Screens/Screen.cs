using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NyanQueue.Core.ScreenSystem.Settings;
using NyanQueue.Examples.Transitions;
using UnityEngine;

namespace NyanQueue.Core.ScreenSystem.Screens
{
    public abstract class Screen : MonoBehaviour
    {
        [SerializeField] private List<AbstractTransition> _openTransitions;
        [SerializeField] private List<AbstractTransition> _closeTransitions;
        
        protected Dictionary<string, AbstractTransition> _openTransitionsDict;
        protected Dictionary<string, AbstractTransition> _closeTransitionsDict;
        
        protected IReadOnlyDictionary<string, AbstractTransition> OpenTransitionsDictionary 
            => _openTransitionsDict ?? _openTransitions.ToDictionary(t => t.TransitionName);
        protected IReadOnlyDictionary<string, AbstractTransition> CloseTransitionsDictionary
            => _closeTransitionsDict ?? _closeTransitions.ToDictionary(t => t.TransitionName);
        
        protected AbstractTransition DefaultOpenTransition => _openTransitions?.FirstOrDefault();
        protected AbstractTransition DefaultCloseTransition => _closeTransitions?.FirstOrDefault();

        public ScreenSettings Settings { get; private set; }
        
        public void SetSettings(ScreenSettings screenSettings) => Settings = screenSettings;
        
        public virtual async UniTask Open(string transitionName = "")
        {
            var transition =
                string.IsNullOrEmpty(transitionName)
                    ? DefaultOpenTransition
                    : (OpenTransitionsDictionary.TryGetValue(transitionName, out var t)
                        ? t
                        : DefaultOpenTransition);
            if (transition == null) return;
            await transition.Run();
        }

        public virtual async UniTask Close(string transitionName = "")
        {
            var transition =
                string.IsNullOrEmpty(transitionName)
                    ? DefaultCloseTransition
                    : (CloseTransitionsDictionary.TryGetValue(transitionName, out var t)
                        ? t
                        : DefaultCloseTransition);
            if (transition == null) return;
            await transition.Run();
        }
    }
}
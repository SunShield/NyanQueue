using System;

namespace NyanQueue.Core.ScreenSystem.Overrides.Impl
{
    [Serializable]
    public class PrevScreenTransitionOverride : ScreenOpenOverride
    {
        public string TransitionName;
        
        public PrevScreenTransitionOverride() { }
        public PrevScreenTransitionOverride(string transitionName) => TransitionName = transitionName;
        public void SetTransitionName(string transitionName) => TransitionName = transitionName;
    }
}
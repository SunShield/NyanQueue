namespace NyanQueue.Core.ScreenSystem.Overrides.Impl
{
    public class CurrentScreenTransitionOverride : ScreenOpenOverride
    {
        public string TransitionName;
        
        public CurrentScreenTransitionOverride() { }
        public CurrentScreenTransitionOverride(string transitionName) => TransitionName = transitionName;
        public void SetTransitionName(string transitionName) => TransitionName = transitionName;
    }
}
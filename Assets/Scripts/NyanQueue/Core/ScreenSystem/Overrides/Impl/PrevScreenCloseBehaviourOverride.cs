using NyanQueue.Core.ScreenSystem.Settings.Impl;

namespace NyanQueue.Core.ScreenSystem.Overrides.Impl
{
    public class PrevScreenCloseBehaviourOverride : ScreenOpenOverride
    {
        public ScreenCloseBehaviour CloseBehaviour;
        
        public PrevScreenCloseBehaviourOverride() { }
        public PrevScreenCloseBehaviourOverride(ScreenCloseBehaviour closeBehaviour) => CloseBehaviour = closeBehaviour;
        public void SetCloseBehaviour(ScreenCloseBehaviour closeBehaviour) => CloseBehaviour = closeBehaviour;
    }
}
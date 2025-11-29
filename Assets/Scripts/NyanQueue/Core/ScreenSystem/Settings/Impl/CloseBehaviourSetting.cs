using System;

namespace NyanQueue.Core.ScreenSystem.Settings.Impl
{
    [Serializable]
    public class CloseBehaviourSetting : ScreenSetting
    {
        public ScreenCloseBehaviour CloseBehaviour;
        
        public CloseBehaviourSetting() { }
        public CloseBehaviourSetting(ScreenCloseBehaviour closeBehaviour) => CloseBehaviour = closeBehaviour;
        public void SetCloseBehaviour(ScreenCloseBehaviour closeBehaviour) => CloseBehaviour = closeBehaviour;
    }
}
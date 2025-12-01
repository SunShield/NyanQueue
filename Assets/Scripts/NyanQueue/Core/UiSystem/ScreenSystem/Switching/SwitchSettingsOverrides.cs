using System;
using NyanQueue.Core.UiSystem.Utilities.Enums;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Switching
{
    [Serializable]
    public class SwitchSettingsOverrides
    {
        public bool OverrideCloseBehaviour         { get; private set; }
        public bool OverridePrevScreenAnimation    { get; private set; }
        public bool OverrideCurrentScreenAnimation { get; private set; }
        
        public CloseBehaviour CloseBehaviourOverride  { get; private set; }
        public string PreviousScreenAnimationOverride { get; private set; }
        public string CurrentScreenAnimationOverride  { get; private set; }

        public SwitchSettingsOverrides SetCloseBehaviour(CloseBehaviour closeBehaviour)
        {
            OverrideCloseBehaviour = true;
            CloseBehaviourOverride = closeBehaviour;
            return this;
        }

        public SwitchSettingsOverrides SetPrevScreenAnimationOverride(string animOverride)
        {
            OverridePrevScreenAnimation = true;
            PreviousScreenAnimationOverride = animOverride;
            return this;
        }

        public SwitchSettingsOverrides SetCurrentScreenAnimationOverride(string animOverride)
        {
            OverrideCurrentScreenAnimation = true;
            CurrentScreenAnimationOverride = animOverride;
            return this;
        }
    }
}
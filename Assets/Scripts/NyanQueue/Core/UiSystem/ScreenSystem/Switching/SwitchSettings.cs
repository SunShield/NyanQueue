using System;
using NyanQueue.Core.UiSystem.Utilities.Enums;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Switching
{
    [Serializable]
    public class SwitchSettings
    {
        public bool           OverrideCloseBehavior    { get; private set; }
        public CloseBehaviour PrevScreenCloseBehaviour { get; private set; }
        public string         PrevScreenAnimation      { get; private set; }
        public string         CurrentScreenAnimation   { get; private set; }
        
        public SwitchSettings() { }

        public SwitchSettings SetPrevScreenCloseBehaviour(CloseBehaviour closeBehaviour)
        {
            OverrideCloseBehavior = true;
            PrevScreenCloseBehaviour = closeBehaviour;
            return this;
        }

        public SwitchSettings SetPrevScreenAnimation(string prevScreenAnimation)
        {
            PrevScreenAnimation = prevScreenAnimation;
            return this;
        }

        public SwitchSettings SetCurrentScreenAnimation(string currentScreenAnimation)
        {
            CurrentScreenAnimation = currentScreenAnimation;
            return this;
        }

        public SwitchSettings Merge(SwitchSettingsOverrides overrides)
           => new ()
               {
                   OverrideCloseBehavior = OverrideCloseBehavior || overrides.OverrideCloseBehaviour,
                   PrevScreenCloseBehaviour = !overrides.OverrideCloseBehaviour 
                       ? PrevScreenCloseBehaviour 
                       : overrides.CloseBehaviourOverride,
                   PrevScreenAnimation  = !overrides.OverridePrevScreenAnimation 
                       ? PrevScreenAnimation 
                       : overrides.PreviousScreenAnimationOverride,
                   CurrentScreenAnimation = !overrides.OverrideCurrentScreenAnimation 
                       ? CurrentScreenAnimation
                       : overrides.CurrentScreenAnimationOverride,
               };
    }
}
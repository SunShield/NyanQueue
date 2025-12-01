using System;
using NyanQueue.Core.UiSystem.Utilities.Enums;
using UnityEngine;

namespace NyanQueue.Core.UiSystem.Utilities.Classes.Settings
{
    [Serializable]
    public class SwitchSettings
    {
        [field: SerializeField] public bool           OverrideCloseBehavior { get; private set; }
        [field: SerializeField] public bool           OverridePrevAnimation { get; private set; }
        [field: SerializeField] public bool           OverrideCurAnimation  { get; private set; }
        [field: SerializeField] public CloseBehaviour PrevCloseBehaviour    { get; private set; }
        [field: SerializeField] public string         PrevAnimation         { get; private set; }
        [field: SerializeField] public string         CurAnimation          { get; private set; }
        
        public SwitchSettings() { }

        public SwitchSettings SetPrevCloseBehaviour(CloseBehaviour closeBehaviour)
        {
            OverrideCloseBehavior = true;
            PrevCloseBehaviour = closeBehaviour;
            return this;
        }

        public SwitchSettings SetPrevAnimation(string prevScreenAnimation)
        {
            PrevAnimation = prevScreenAnimation;
            return this;
        }

        public SwitchSettings SetCurrentAnimation(string currentScreenAnimation)
        {
            CurAnimation = currentScreenAnimation;
            return this;
        }

        public SwitchSettings Merge(SwitchSettings overrides)
           => new ()
               {
                   OverrideCloseBehavior = OverrideCloseBehavior || overrides.OverrideCloseBehavior,
                   PrevCloseBehaviour = !overrides.OverrideCloseBehavior 
                       ? PrevCloseBehaviour 
                       : overrides.PrevCloseBehaviour,
                   PrevAnimation  = !overrides.OverridePrevAnimation 
                       ? PrevAnimation 
                       : overrides.PrevAnimation,
                   CurAnimation = !overrides.OverrideCurAnimation 
                       ? CurAnimation
                       : overrides.CurAnimation,
               };
    }
}
using System;
using NyanQueue.Core.UiSystem.Utilities.Enums;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Settings
{
    [Serializable]
    public class ScreenSettings
    {
        public int Order;
        public CloseBehaviour CloseBehaviour;
        
        public ScreenSettings() { }
        public ScreenSettings(int order) { Order = order; }
        public ScreenSettings(CloseBehaviour closeBehaviour) { CloseBehaviour = closeBehaviour; }
        public ScreenSettings(int order, CloseBehaviour closeBehaviour) : this(order) { CloseBehaviour = closeBehaviour; }

        public ScreenSettings Merge(int? orderOverride = null, CloseBehaviour? closeBehaviourOverride = null)
            => new ()
            {
                Order = orderOverride ?? Order,
                CloseBehaviour = closeBehaviourOverride ?? CloseBehaviour,
            };
    }
}
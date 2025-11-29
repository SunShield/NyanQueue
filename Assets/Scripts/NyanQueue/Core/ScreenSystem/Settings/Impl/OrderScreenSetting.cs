using System;

namespace NyanQueue.Core.ScreenSystem.Settings.Impl
{
    [Serializable]
    public class OrderScreenSetting : ScreenSetting
    {
        public int Order;
        
        public OrderScreenSetting() { }
        public OrderScreenSetting(int order) => Order = order;
        public void SetOrder(int order) => Order = order;
    }
}
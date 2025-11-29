using System;

namespace NyanQueue.Core.ScreenSystem.Overrides.Impl
{
    [Serializable]
    public class OrderOverride : ScreenOpenOverride
    {
        public int Order;
        
        public OrderOverride() { }
        public OrderOverride(int order) => Order = order;
        public void SetOrder(int order) => Order = order;
    }
}
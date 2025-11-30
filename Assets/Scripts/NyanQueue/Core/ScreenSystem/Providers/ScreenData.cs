using System;
using NyanQueue.Core.ScreenSystem.Screens;

namespace NyanQueue.Core.ScreenSystem.Providers
{
    [Serializable]
    public class ScreenData
    {
        public int DefaultOrder;
        public AbstractScreen Screen;
    }
}
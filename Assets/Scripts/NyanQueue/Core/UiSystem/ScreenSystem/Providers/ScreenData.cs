using System;
using NyanQueue.Core.UiSystem.ScreenSystem.Screens;
using NyanQueue.Core.UiSystem.ScreenSystem.Settings;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Providers
{
    [Serializable]
    public class ScreenData
    {
        public ScreenSettings DefaultSettings;
        public AbstractScreen Screen;
    }
}
using System;
using System.Collections.Generic;
using NyanQueue.Core.UiSystem.Utilities.Classes.Settings;
using UnityEngine;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Providers.Defaults
{
    [Serializable]
    public class ScreenDefaults
    {
        [field: SerializeField] public string                     ScreenType     { get; private set; }
        [field: SerializeField] public ScreenSettings             ScreenSettings { get; private set; }
        [field: SerializeField] public List<ScreenSwitchDefaults> SwitchDefaults { get; private set; } = new();
    }
}
using System;
using NyanQueue.Core.UiSystem.Utilities.Classes.Settings;
using UnityEngine;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Providers.Defaults
{
    [Serializable]
    public class ScreenSwitchDefaults
    {
        [field: SerializeField] public string         PrevScreenType { get; private set; }
        [field: SerializeField] public SwitchSettings SwitchSettings { get; private set; }
    }
}
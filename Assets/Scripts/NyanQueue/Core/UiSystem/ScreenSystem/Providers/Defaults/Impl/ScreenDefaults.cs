using System;
using System.Collections.Generic;
using NyanQueue.Core.UiSystem.ScreenSystem.Screens;
using NyanQueue.Core.UiSystem.Utilities.Classes.Settings;
using NyanTypeReference;
using NyanTypeReference.Attribs;
using UnityEngine;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Providers.Defaults.Impl
{
    [Serializable]
    public class ScreenDefaults
    {
        [field: TypeOptions(baseType: typeof(AbstractScreen))]
        [field: SerializeField] public TypeReference              ScreenType     { get; private set; }
        [field: SerializeField] public ScreenSettings             ScreenSettings { get; private set; }
        [field: SerializeField] public List<ScreenSwitchDefaults> SwitchDefaults { get; private set; } = new();
    }
}
using System;
using NyanQueue.Core.UiSystem.ScreenSystem.Screens;
using NyanQueue.Core.UiSystem.Utilities.Classes.Settings;
using NyanTypeReference;
using NyanTypeReference.Attribs;
using UnityEngine;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Providers.Defaults.Impl
{
    [Serializable]
    public class ScreenSwitchDefaults
    {   
        [field: TypeOptions(baseType: typeof(AbstractScreen))]
        [field: SerializeField] public TypeReference  PrevScreenType { get; private set; }
        [field: SerializeField] public SwitchSettings SwitchSettings { get; private set; }
    }
}
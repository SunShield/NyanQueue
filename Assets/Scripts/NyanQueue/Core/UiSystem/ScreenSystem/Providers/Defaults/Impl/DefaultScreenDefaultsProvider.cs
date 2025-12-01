using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Providers.Defaults.Impl
{
    [CreateAssetMenu(fileName = "ScreenDefaultsProvider", menuName = "NyanQueue/Screens/Defaults Provider")]
    public class DefaultScreenDefaultsProvider : ScriptableObject, IScreenDefaultsProvider
    {
        [SerializeField] private List<ScreenDefaults> _defaults = new();
        
        public async UniTask ProvideDefaults(ScreenManager manager)
        {
            foreach (var screenDefault in _defaults)
            {
                var type = screenDefault.ScreenType.Type;
                manager.RegisterDefaultScreenSettings(type, screenDefault.ScreenSettings);

                foreach (var switchDefault in screenDefault.SwitchDefaults)
                {
                    var prevType = switchDefault.PrevScreenType.Type;
                    manager.RegisterDefaultSwitchSettings(type, prevType, switchDefault.SwitchSettings);
                }
            }
        }
    }
}
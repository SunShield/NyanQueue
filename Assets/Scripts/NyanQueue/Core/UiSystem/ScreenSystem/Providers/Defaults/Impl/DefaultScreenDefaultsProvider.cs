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
        
        private Dictionary<string, Type> _typeNameMap;
        
        public async UniTask ProvideDefaults(ScreenManager manager)
        {
            _typeNameMap = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .ToDictionary(t => t.Name, t => t, StringComparer.Ordinal);
            
            foreach (var screenDefault in _defaults)
            {
                var type = _typeNameMap[screenDefault.ScreenType];
                manager.RegisterDefaultScreenSettings(type, screenDefault.ScreenSettings);

                foreach (var switchDefault in screenDefault.SwitchDefaults)
                {
                    var prevType = _typeNameMap[switchDefault.PrevScreenType];
                    manager.RegisterDefaultSwitchSettings(type, prevType, switchDefault.SwitchSettings);
                }
            }
        }
    }
}
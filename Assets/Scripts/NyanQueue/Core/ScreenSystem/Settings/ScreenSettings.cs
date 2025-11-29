using System;
using System.Collections.Generic;
using System.Linq;
using NyanQueue.Core.ScreenSystem.Settings.Impl;
using NyanQueue.Core.Utilities.Extensions;
using UnityEngine;

namespace NyanQueue.Core.ScreenSystem.Settings
{
    [Serializable]
    public class ScreenSettings
    {
        [SerializeField] private List<ScreenSetting> _screenSettings = new ();

        private Dictionary<Type, ScreenSetting> _screenSettingsDict;
        private IReadOnlyDictionary<Type, ScreenSetting> ScreenSettingsDict 
            => _screenSettingsDict ??= _screenSettings.ToDictionary(ss => ss.GetType());
        
        public ScreenSettings SetCloseBehaviour(ScreenCloseBehaviour closeBehaviour)
        {
            var orderSetting = _screenSettings.FirstOrDefault(ss => ss.GetType() == typeof(CloseBehaviourSetting));
            if (orderSetting != null)
            {
                ((CloseBehaviourSetting)orderSetting).SetCloseBehaviour(closeBehaviour);
                return this;
            }
            
            _screenSettings.Add(new CloseBehaviourSetting(closeBehaviour));
            return this;
        }

        public TSetting Get<TSetting>()
            where TSetting : ScreenSetting
        {
            var settingExists = ScreenSettingsDict.TryGetValue(typeof(TSetting), out var setting);
            return settingExists ? setting as TSetting : null;
        }

        public ScreenSettings Merge(ScreenSettings screenSettings)
        {
            var settings = new ScreenSettings()
            {
                _screenSettings = _screenSettings.MergeWithReplaceByTypeSafe(screenSettings._screenSettings)
            };
            return settings;
        }
    }
}
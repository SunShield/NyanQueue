using System;
using System.Collections.Generic;
using System.Linq;
using NyanQueue.Core.ScreenSystem.Overrides.Impl;
using NyanQueue.Core.ScreenSystem.Settings.Impl;
using NyanQueue.Core.Utilities.Extensions;
using UnityEngine;

namespace NyanQueue.Core.ScreenSystem.Overrides
{
    [Serializable]
    public class ScreenOpenOverrides
    {
        [SerializeField] private List<ScreenOpenOverride> _screenOverrides = new ();

        private Dictionary<Type, ScreenOpenOverride> _screenOverridesDict;
        private IReadOnlyDictionary<Type, ScreenOpenOverride> ScreenOverridesDict 
            => _screenOverridesDict ??= _screenOverrides.ToDictionary(ss => ss.GetType());
        
        public ScreenOpenOverrides SetOrderOverride(int order)
        {
            var orderSetting = _screenOverrides.FirstOrDefault(ss => ss.GetType() == typeof(OrderOverride));
            if (orderSetting != null)
            {
                ((OrderOverride)orderSetting).SetOrder(order);
                return this;
            }
            
            _screenOverrides.Add(new OrderOverride(order));
            return this;
        }
        
        public ScreenOpenOverrides SetPrevScreenCloseBehaviour(ScreenCloseBehaviour closeBehaviour)
        {
            var orderSetting = _screenOverrides
                .FirstOrDefault(ss => ss.GetType() == typeof(PrevScreenCloseBehaviourOverride));
            if (orderSetting != null)
            {
                ((PrevScreenCloseBehaviourOverride)orderSetting).SetCloseBehaviour(closeBehaviour);
                return this;
            }
            
            _screenOverrides.Add(new PrevScreenCloseBehaviourOverride(closeBehaviour));
            return this;
        }
        
        public ScreenOpenOverrides SetPrevScreenTransition(string transitionName)
        {
            var orderSetting = _screenOverrides
                .FirstOrDefault(ss => ss.GetType() == typeof(PrevScreenTransitionOverride));
            if (orderSetting != null)
            {
                ((PrevScreenTransitionOverride)orderSetting).SetTransitionName(transitionName);
                return this;
            }
            
            _screenOverrides.Add(new PrevScreenTransitionOverride(transitionName));
            return this;
        }
        
        public ScreenOpenOverrides SetCurrentScreenTransition(string transitionName)
        {
            var orderSetting = _screenOverrides
                .FirstOrDefault(ss => ss.GetType() == typeof(CurrentScreenTransitionOverride));
            if (orderSetting != null)
            {
                ((CurrentScreenTransitionOverride)orderSetting).SetTransitionName(transitionName);
                return this;
            }
            
            _screenOverrides.Add(new CurrentScreenTransitionOverride(transitionName));
            return this;
        }

        public TSetting Get<TSetting>()
            where TSetting : ScreenOpenOverride
        {
            var settingExists = ScreenOverridesDict.TryGetValue(typeof(TSetting), out var setting);
            return settingExists ? setting as TSetting : null;
        }

        public ScreenOpenOverrides Merge(ScreenOpenOverrides screenOpenOverrides)
        {
            var overrides = new ScreenOpenOverrides
            {
                _screenOverrides = _screenOverrides.MergeWithReplaceByTypeSafe(screenOpenOverrides._screenOverrides),
            };
            return overrides;
        }
    }
}
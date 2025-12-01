using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NyanQueue.Core.UiSystem.ScreenSystem.Screens;
using UnityEngine;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Providers.Prefab.Impl
{
    [CreateAssetMenu(fileName = "ScreenPrefabsProvider", menuName = "NyanQueue/Screens/Prefabs Provider")]
    public class DefaultScreenPrefabProvider : ScriptableObject, IScreenPrefabProvider
    {
        [SerializeField] private List<AbstractScreen> _screenDatas;

        private Dictionary<Type, AbstractScreen> _screenDatasDictionary;
        private IReadOnlyDictionary<Type, AbstractScreen> ScreenDatasDictionary
            => _screenDatasDictionary ??= _screenDatas.ToDictionary(sd => sd.GetType());
        
        public UniTask<AbstractScreen> ProvidePrefab(Type screenType)
            => UniTask.FromResult(ScreenDatasDictionary[screenType]);
    }
}
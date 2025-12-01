using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Providers.Impl
{
    [CreateAssetMenu(fileName = "UiPrefabProvider", menuName = "NyanQueue/Ui Prefab Provider")]
    public class DefaultLocalPrefabProvider : ScriptableObject, IPrefabProvider
    {
        [SerializeField] private List<ScreenData> _screenDatas;

        private Dictionary<Type, ScreenData> _screenDatasDictionary;
        private IReadOnlyDictionary<Type, ScreenData> ScreenDatasDictionary
            => _screenDatasDictionary ??= _screenDatas.ToDictionary(sd => sd.Screen.GetType());
        
        public UniTask<ScreenData> ProvideScreen(Type screenType)
            => UniTask.FromResult(ScreenDatasDictionary[screenType]);
    }
}
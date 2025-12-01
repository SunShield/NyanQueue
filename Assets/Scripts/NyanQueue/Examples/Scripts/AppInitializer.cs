using NyanQueue.Core.UiSystem.ScreenSystem;
using NyanQueue.Core.UiSystem.ScreenSystem.Providers.Impl;
using NyanQueue.Examples.Screens;
using NyanQueue.Examples.Scripts.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace NyanQueue.Examples
{
    public class AppInitializer : MonoBehaviour
    {
        [SerializeField] private DefaultLocalPrefabProvider _localPrefabProvider;
        [FormerlySerializedAs("_uiManager")] [SerializeField] private ScreenManager _screenManager;

        private void Start()
        {
            _screenManager.SetPrefabProvider(_localPrefabProvider);
            _screenManager.OpenScreen<TestScreen1, TestScreen1Model>();
        }
    }
}


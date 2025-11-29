using NyanQueue.Core.ScreenSystem;
using NyanQueue.Core.ScreenSystem.Providers.Impl;
using NyanQueue.Examples.Screens;
using NyanQueue.Examples.Scripts.Models;
using UnityEngine;

namespace NyanQueue.Examples
{
    public class AppInitializer : MonoBehaviour
    {
        [SerializeField] private DefaultLocalPrefabProvider _localPrefabProvider;
        [SerializeField] private UiManager _uiManager;

        private void Start()
        {
            _uiManager.SetPrefabProvider(_localPrefabProvider);
            _uiManager.OpenScreen<TestScreen1, TestScreen1Model>(new ());
        }
    }
}


using Cysharp.Threading.Tasks;
using NyanQueue.Core.UiSystem.ScreenSystem;
using NyanQueue.Core.UiSystem.ScreenSystem.Providers.Defaults;
using NyanQueue.Core.UiSystem.ScreenSystem.Providers.Defaults.Impl;
using NyanQueue.Core.UiSystem.ScreenSystem.Providers.Prefab;
using NyanQueue.Core.UiSystem.ScreenSystem.Providers.Prefab.Impl;
using UnityEngine;

namespace NyanQueue.Core.UiSystem.Roots.Impl
{
    public class DefaultUiSystemRoot : BaseUiSystemRoot
    {
        [SerializeField] private ScreenManager _screenManager;
        [SerializeField] private DefaultScreenPrefabProvider _screenPrefabProvider;
        [SerializeField] private DefaultScreenDefaultsProvider _screenDefaultsProvider;

        protected override ScreenManager ScreenManager => _screenManager;
        protected override IScreenPrefabProvider ScreenPrefabProvider => _screenPrefabProvider;
        protected override IScreenDefaultsProvider ScreenDefaultsProvider => _screenDefaultsProvider;

        protected override async UniTask DoAwake() => await Build();
    }
}
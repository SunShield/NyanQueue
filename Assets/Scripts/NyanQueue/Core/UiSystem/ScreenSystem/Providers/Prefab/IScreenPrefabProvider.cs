using System;
using Cysharp.Threading.Tasks;
using NyanQueue.Core.UiSystem.ScreenSystem.Screens;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Providers.Prefab
{
    public interface IScreenPrefabProvider
    {
        UniTask<AbstractScreen> ProvidePrefab(Type screenType);
    }
}
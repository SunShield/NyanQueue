using System;
using Cysharp.Threading.Tasks;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Providers
{
    public interface IPrefabProvider
    {
        UniTask<ScreenData> ProvideScreen(Type screenType);
        //public abstract UniTask<IPopup> ProvidePopup(Type popupType);
    }
}
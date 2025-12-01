using Cysharp.Threading.Tasks;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Providers.Defaults
{
    public interface IScreenDefaultsProvider
    {
        UniTask ProvideDefaults(ScreenManager manager);
    }
}
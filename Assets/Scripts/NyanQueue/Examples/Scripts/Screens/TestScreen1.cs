using Cysharp.Threading.Tasks;
using NyanQueue.Core.ScreenSystem.Screens;
using NyanQueue.Examples.Scripts.Models;

namespace NyanQueue.Examples.Screens
{
    public class TestScreen1 : InitializableScreen<TestScreen1Model>
    {
        protected override UniTask InitializeInternal(TestScreen1Model model)
        {
            return UniTask.CompletedTask;
        }
    }
}
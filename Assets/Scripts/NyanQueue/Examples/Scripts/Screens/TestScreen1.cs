using Cysharp.Threading.Tasks;
using NyanQueue.Core.UiSystem.ScreenSystem.Screens;
using NyanQueue.Examples.Scripts.Models;
using NyanQueue.Examples.Views;

namespace NyanQueue.Examples.Screens
{
    public class TestScreen1 : BaseScreen<TestScreen1Model, TestEmptyView>
    {
        protected override UniTask InitializeInternal(TestScreen1Model model)
        {
            return UniTask.CompletedTask;
        }
    }
}
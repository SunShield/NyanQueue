using Cysharp.Threading.Tasks;
using NyanQueue.Core.UiSystem.ScreenSystem.Screens.Models;

namespace NyanQueue.Core.UiSystem.ScreenSystem.Screens
{
    public abstract class InitializableScreen<TModel> : AbstractScreen 
        where TModel : ScreenModel
    {
        public bool IsInitialized { get; private set; }

        public async UniTask Initialize(TModel model)
        {
            await InitializeInternal(model);
            IsInitialized = true;
        }
        
        protected virtual UniTask InitializeInternal(TModel model) => UniTask.CompletedTask;
    }
}
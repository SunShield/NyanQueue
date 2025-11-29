using Cysharp.Threading.Tasks;

namespace NyanQueue.Core.ScreenSystem.Screens
{
    public abstract class InitializableScreen<TModel> : Screen 
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
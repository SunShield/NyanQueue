using Cysharp.Threading.Tasks;

namespace NyanQueue.Core.Scheduling.Actions
{
    public abstract class ScheduledAction
    {
        public async UniTask Await()
        {
            await AwaitMain();
        }
        
        protected abstract UniTask AwaitMain();
    }
}
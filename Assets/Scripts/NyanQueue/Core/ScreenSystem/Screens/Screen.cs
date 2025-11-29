using Cysharp.Threading.Tasks;
using NyanQueue.Core.ScreenSystem.Settings;
using UnityEngine;

namespace NyanQueue.Core.ScreenSystem.Screens
{
    public abstract class Screen : MonoBehaviour
    {
        public ScreenSettings Settings { get; private set; }
        
        public void SetSettings(ScreenSettings screenSettings) => Settings = screenSettings;
        
        public virtual UniTask Open() => UniTask.CompletedTask;
        public virtual UniTask Close() => UniTask.CompletedTask;
    }
}
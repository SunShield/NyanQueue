using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NyanQueue.Core.UiSystem.Utilities.Classes.Animations
{
    public abstract class AbstractAnimation : MonoBehaviour
    {
        [SerializeField] private string _transitionName;
        
        public string TransitionName => _transitionName;
        
        public abstract UniTask Run();
    }
}
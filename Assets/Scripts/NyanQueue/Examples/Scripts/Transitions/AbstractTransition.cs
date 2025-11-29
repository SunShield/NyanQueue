using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NyanQueue.Examples.Transitions
{
    public abstract class AbstractTransition : MonoBehaviour
    {
        [SerializeField] private string _transitionName;
        
        public string TransitionName => _transitionName;
        
        public abstract UniTask Run();
    }
}
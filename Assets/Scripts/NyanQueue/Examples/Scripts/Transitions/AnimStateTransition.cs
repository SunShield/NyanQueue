using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NyanQueue.Examples.Transitions
{
    public class AnimStateTransition : AbstractTransition
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _trigger;
        [SerializeField] private string _targetState;
        
        public override async UniTask Run(CancellationToken token)
        {
            // if we have to go to non-starting state first
            if (!string.IsNullOrEmpty(_trigger)) _animator.SetTrigger(_trigger);
            
            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_targetState)) await UniTask.Yield(token);
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) await UniTask.Yield(token);
        }
    }
}
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NyanQueue.Core.UiSystem.Utilities.Classes.Animations
{
    public class AnimStateAnimation : AbstractAnimation
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _trigger;
        [SerializeField] private string _targetState;
        
        public override async UniTask Run()
        {
            // if we have to go to non-starting state first
            if (!string.IsNullOrEmpty(_trigger)) _animator.SetTrigger(_trigger);
            
            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_targetState)) await UniTask.Yield();
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) await UniTask.Yield();
        }
    }
}
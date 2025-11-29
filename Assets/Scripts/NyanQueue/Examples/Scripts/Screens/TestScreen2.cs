using System.Threading;
using Cysharp.Threading.Tasks;
using NyanQueue.Core.ScreenSystem.Screens;
using NyanQueue.Examples.Scripts.Models;
using NyanQueue.Examples.Transitions;
using UnityEngine;

namespace NyanQueue.Examples.Screens
{
    public class TestScreen2 : InitializableScreen<TestScreen2Model>
    {
        [SerializeField] private AbstractTransition _transition;
        [SerializeField] private AbstractTransition _closeTransition;

        public override async UniTask Open()
        {
            await _transition.Run(CancellationToken.None);
        }

        public override async UniTask Close()
        {
            await _closeTransition.Run(CancellationToken.None);
        }
    }
}
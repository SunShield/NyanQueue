using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NyanQueue.Examples.Transitions
{
    public abstract class AbstractTransition : MonoBehaviour
    {
        public abstract UniTask Run(CancellationToken token);
    }
}
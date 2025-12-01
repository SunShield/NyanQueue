using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using NyanQueue.Core.UiSystem.Roots;
using NyanQueue.Core.UiSystem.ScreenSystem;
using NyanQueue.Examples.Screens;
using NyanQueue.Examples.Scripts.Models;
using UnityEngine;

namespace NyanQueue.Examples
{
    public class AppInitializer : MonoBehaviour
    {
        [SerializeField] private BaseUiSystemRoot _uiSystemRoot;
        [SerializeField] private ScreenManager _screenManager;

        private async void Start() => await OpenFirstScreen();

        private async UniTask OpenFirstScreen()
        {
            if (!_uiSystemRoot.IsBuilt) await UniTask.WaitUntil(() => _uiSystemRoot.IsBuilt);

            await _screenManager.OpenScreen<TestScreen1, TestScreen1Model>();
        }
    }
}


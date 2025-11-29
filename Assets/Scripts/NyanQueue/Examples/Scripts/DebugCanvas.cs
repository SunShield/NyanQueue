using System;
using NyanQueue.Core.ScreenSystem;
using NyanQueue.Core.ScreenSystem.Settings;
using NyanQueue.Core.ScreenSystem.Settings.Impl;
using NyanQueue.Examples.Screens;
using NyanQueue.Examples.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace NyanQueue.Examples
{
    public class DebugCanvas : MonoBehaviour
    {
        [SerializeField] private UiManager _uiManager;
        [SerializeField] private Button _screen1Button;
        [SerializeField] private Button _screen2Button;
        [SerializeField] private Button _screen3Button;

        private void Awake()
        {
            _screen1Button.onClick.AddListener(Screen1ButtonClickHandler);
            _screen2Button.onClick.AddListener(Screen2ButtonClickHandler);
            _screen3Button.onClick.AddListener(Screen3ButtonClickHandler);
        }

        private async void Screen1ButtonClickHandler()
        {
            await _uiManager.OpenScreen(
                new ScreenOpenOperation<TestScreen1, TestScreen1Model>());
        }

        private async void Screen2ButtonClickHandler()
        {
            await _uiManager.OpenScreen(
                new ScreenOpenOperation<TestScreen2, TestScreen2Model>()
                    .SetSettings(new ScreenSettings()
                        .SetCloseBehaviour(ScreenCloseBehaviour.AfterNext)));
        }

        private async void Screen3ButtonClickHandler()
        {
            await _uiManager.OpenScreen(
                new ScreenOpenOperation<TestScreen3, TestScreen3Model>());
        }
    }
}
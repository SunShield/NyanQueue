using NyanQueue.Core.ScreenSystem;
using NyanQueue.Core.ScreenSystem.Overrides;
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
        [SerializeField] private Button _screen4Button;
        [SerializeField] private Button _screen5Button;

        private void Awake()
        {
            _screen1Button.onClick.AddListener(Screen1ButtonClickHandler);
            _screen2Button.onClick.AddListener(Screen2ButtonClickHandler);
            _screen3Button.onClick.AddListener(Screen3ButtonClickHandler);
            _screen4Button.onClick.AddListener(Screen4ButtonClickHandler);
            _screen5Button.onClick.AddListener(Screen5ButtonClickHandler);
            
            _uiManager.RegisterDefaultScreenSettings<TestScreen2>(
                new ScreenSettings()
                    .SetCloseBehaviour(ScreenCloseBehaviour.AfterNext));
            _uiManager.RegisterDefaultScreenSettings<TestScreen4>(
                new ScreenSettings()
                    .SetCloseBehaviour(ScreenCloseBehaviour.AfterNext));
            _uiManager.RegisterDefaultScreenSettings<TestScreen5>(
                new ScreenSettings()
                    .SetCloseBehaviour(ScreenCloseBehaviour.AfterNext));
            
            _uiManager.RegisterDefaultOverrides<TestScreen4, TestScreen2>(
                new ScreenOpenOverrides()
                    .SetPrevScreenCloseBehaviour(ScreenCloseBehaviour.WithNext)
                    .SetCurrentScreenTransition("SlideIn")
                    .SetPrevScreenTransition("SlideOut"));
        }

        private async void Screen1ButtonClickHandler()
        {
            await _uiManager.OpenScreen<TestScreen1, TestScreen1Model>();
        }

        private async void Screen2ButtonClickHandler()
        {
            await _uiManager.OpenScreen<TestScreen2, TestScreen2Model>();
        }

        private async void Screen3ButtonClickHandler()
        {
            await _uiManager.OpenScreen<TestScreen3, TestScreen3Model>();
        }

        private async void Screen4ButtonClickHandler()
        {
            await _uiManager.OpenScreen<TestScreen4, TestScreen4Model>();
        }

        private async void Screen5ButtonClickHandler()
        {
            await _uiManager.OpenScreen<TestScreen5, TestScreen5Model>();
        }
    }
}
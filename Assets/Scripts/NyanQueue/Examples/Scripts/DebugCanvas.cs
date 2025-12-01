using NyanQueue.Core.UiSystem.ScreenSystem;
using NyanQueue.Core.UiSystem.ScreenSystem.Settings;
using NyanQueue.Core.UiSystem.ScreenSystem.Switching;
using NyanQueue.Core.UiSystem.Utilities.Enums;
using NyanQueue.Examples.Screens;
using NyanQueue.Examples.Scripts.Models;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace NyanQueue.Examples
{
    public class DebugCanvas : MonoBehaviour
    {
        [FormerlySerializedAs("_uiManager")] [SerializeField] private ScreenManager _screenManager;
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

            _screenManager.RegisterDefaultScreenSettings<TestScreen2>(new ScreenSettings(CloseBehaviour.AfterNext));
            _screenManager.RegisterDefaultScreenSettings<TestScreen4>(new ScreenSettings(CloseBehaviour.AfterNext));
            _screenManager.RegisterDefaultScreenSettings<TestScreen5>(new ScreenSettings(CloseBehaviour.AfterNext));
            
            _screenManager.RegisterDefaultSwitchSettings<TestScreen4, TestScreen2>(
                new SwitchSettings()
                    .SetPrevScreenCloseBehaviour(CloseBehaviour.WithNext)
                    .SetCurrentScreenAnimation("SlideIn")
                    .SetPrevScreenAnimation("SlideOut"));
        }

        private async void Screen1ButtonClickHandler()
        {
            await _screenManager.OpenScreen<TestScreen1, TestScreen1Model>();
        }

        private async void Screen2ButtonClickHandler()
        {
            await _screenManager.OpenScreen<TestScreen2, TestScreen2Model>();
        }

        private async void Screen3ButtonClickHandler()
        {
            await _screenManager.OpenScreen<TestScreen3, TestScreen3Model>();
        }

        private async void Screen4ButtonClickHandler()
        {
            await _screenManager.OpenScreen<TestScreen4, TestScreen4Model>();
        }

        private async void Screen5ButtonClickHandler()
        {
            await _screenManager.OpenScreen<TestScreen5, TestScreen5Model>();
        }
    }
}
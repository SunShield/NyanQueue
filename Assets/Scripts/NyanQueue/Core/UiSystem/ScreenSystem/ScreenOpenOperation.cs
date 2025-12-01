using NyanQueue.Core.UiSystem.ScreenSystem.Screens;
using NyanQueue.Core.UiSystem.ScreenSystem.Screens.Models;
using NyanQueue.Core.UiSystem.Utilities.Classes.Settings;
using NyanQueue.Core.UiSystem.Utilities.Enums;

namespace NyanQueue.Core.UiSystem.ScreenSystem
{
    public class ScreenOpenOperation<TScreen, TModel>
        where TScreen : InitializableScreen<TModel>
        where TModel : ScreenModel
    {
        public TModel          Model                   { get; private set; }
        public int?            OrderOverride           { get; private set; }
        public CloseBehaviour? CloseBehaviourOverride  { get; private set; }
        public SwitchSettings  SwitchSettingsOverrides { get; private set; }

        public ScreenOpenOperation<TScreen, TModel> SetModel(TModel model)
        {
            Model = model;
            return this;
        }

        public ScreenOpenOperation<TScreen, TModel> SetOrder(int order)
        {
            OrderOverride = order;
            return this;
        }

        public ScreenOpenOperation<TScreen, TModel> SetCloseBehaviour(CloseBehaviour closeBehaviour)
        {
            CloseBehaviourOverride = closeBehaviour;
            return this;
        }

        public ScreenOpenOperation<TScreen, TModel> SetSwitchSettings(SwitchSettings overrides)
        {
            SwitchSettingsOverrides = overrides;
            return this;
        }
    }
}
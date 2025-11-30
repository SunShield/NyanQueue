using NyanQueue.Core.ScreenSystem.Overrides;
using NyanQueue.Core.ScreenSystem.Screens;
using NyanQueue.Core.ScreenSystem.Screens.Models;
using NyanQueue.Core.ScreenSystem.Settings;

namespace NyanQueue.Core.ScreenSystem
{
    public class ScreenOpenOperation<TScreen, TModel>
        where TScreen : InitializableScreen<TModel>
        where TModel : ScreenModel
    {
        public TModel Model { get; private set; }
        public ScreenSettings ScreenSettings { get; private set; }
        public ScreenOpenOverrides OpenOverrides { get; private set; }

        public ScreenOpenOperation<TScreen, TModel> SetModel(TModel model)
        {
            Model = model;
            return this;
        }

        public ScreenOpenOperation<TScreen, TModel> SetSettings(ScreenSettings screenSettings)
        {
            ScreenSettings = screenSettings;
            return this;
        }
        
        public ScreenOpenOperation<TScreen, TModel> SetOverrides(ScreenOpenOverrides openOverrides)
        {
            OpenOverrides = openOverrides;
            return this;
        }
    }
}
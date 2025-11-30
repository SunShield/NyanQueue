using NyanQueue.Core.ScreenSystem.Screens.Models;
using NyanQueue.Core.ScreenSystem.Screens.Views;
using UnityEngine;

namespace NyanQueue.Core.ScreenSystem.Screens
{
    public class BaseScreen<TModel, TView> : InitializableScreen<TModel>
        where TModel : ScreenModel
        where TView : ScreenView
    {
        [SerializeField] protected TView _view;

        protected override ScreenView BaseView => _view;
        public TView View => _view;
    }
}
using OverviewApp.ViewModels;

namespace OverviewApp.Auxiliary.Helpers
{
    /// <summary>
    ///     Used as message, to switch the view to a different one. Whatever you set
    ///     to catches this message will be the one changing the views.
    /// </summary>
    /// <remarks>
    ///     NOT USED IN THIS TEMPLATE. This is just the prototype to
    ///     show you how you can easily do it.
    /// </remarks>
    public class SwitchView
    {
        #region

        public SwitchView(MyBase_ViewModel viewmodel)
        {
            ViewModel = viewmodel;
        }

        #endregion

        #region Properties

        public MyBase_ViewModel ViewModel { get; set; }

        #endregion
    }
}
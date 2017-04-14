namespace OverviewApp.Auxiliary.Helpers
{
    /// <summary>
    ///     Global class used to refresh the people in the datagrid.
    ///     Used so you can call this from the menu, or another view model
    ///     for example.
    /// </summary>
    public class RefreshPeople
    {
        #region

        public RefreshPeople(int anAmount)
        {
            PeopleToFetch = anAmount;
        }

        #endregion

        #region Properties

        public int PeopleToFetch { get; set; }

        #endregion
    }
}
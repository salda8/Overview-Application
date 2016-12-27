namespace DataStructures.POCO
{
    /// <summary>
    ///     This is the default MVVM Light DataItem. It's a simlpe class that holds a string and
    ///     is used to show how to use the IDataservice Interface and get back either design
    ///     or runtime data.
    /// </summary>
    public class DataItem
    {
        #region

        public DataItem(string title)
        {
            Title = title;
        }

        #endregion

        #region Properties

        public string Title { get; private set; }

        #endregion
    }
}
using System;

namespace OverviewApp.Properties
{
    /// <summary>
    ///     This attribute is intended to mark publicly available API
    ///     which should not be removed and so is treated as used
    /// </summary>
    [MeansImplicitUse]
    public sealed class PublicAPIAttribute : Attribute
    {
        #region

        public PublicAPIAttribute()
        {
        }

        public PublicAPIAttribute([NotNull] string comment)
        {
            Comment = comment;
        }

        #endregion

        #region Properties

        [NotNull]
        public string Comment { get; private set; }

        #endregion
    }
}
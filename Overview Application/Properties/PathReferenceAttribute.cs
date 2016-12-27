using System;

namespace OverviewApp.Properties
{
    /// <summary>
    ///     Indicates that a parameter is a path to a file or a folder
    ///     within a web project. Path can be relative or absolute,
    ///     starting from web root (~)
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class PathReferenceAttribute : Attribute
    {
        #region

        public PathReferenceAttribute()
        {
        }

        public PathReferenceAttribute([PathReference] string basePath)
        {
            BasePath = basePath;
        }

        #endregion

        #region Properties

        [NotNull]
        public string BasePath { get; private set; }

        #endregion
    }
}
using System;

namespace OverviewApp.Properties
{
    [AttributeUsage(
        AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Field)]
    public sealed class HtmlElementAttributesAttribute : Attribute
    {
        #region

        public HtmlElementAttributesAttribute()
        {
        }

        public HtmlElementAttributesAttribute([NotNull] string name)
        {
            Name = name;
        }

        #endregion

        #region Properties

        [NotNull]
        public string Name { get; private set; }

        #endregion
    }
}
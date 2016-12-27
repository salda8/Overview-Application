using System;

namespace OverviewApp.Properties
{
    [AttributeUsage(
        AttributeTargets.Parameter | AttributeTargets.Field |
        AttributeTargets.Property)]
    public sealed class HtmlAttributeValueAttribute : Attribute
    {
        #region

        public HtmlAttributeValueAttribute([NotNull] string name)
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
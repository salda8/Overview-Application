using System;
using System.Reflection;

namespace OverviewApp.Auxiliary.Helpers
{
    public interface IAttributesHelper
    {
        /// <summary>
        /// Gets or sets the customer name fallback.
        /// </summary>
        /// <value>
        /// The customer name fallback.
        /// </value>
        string CustomerNameFallback { get; }

        /// <summary>
        /// Gets the product name fallback.
        /// </summary>
        /// <value>
        /// The product name fallback.
        /// </value>
        string ProductNameFallback { get; }

        /// <summary>
        /// Gets the atrribute value.
        /// </summary>
        /// <typeparam name="TAttributeType">The type of the attribute type.</typeparam>
        /// <param name="assembly">The assembly.</param>
        /// <param name="extractValue">The extract value.</param>
        /// <param name="defaulValue">The defaul value.</param>
        /// <returns></returns>
        string GetAtrributeValue<TAttributeType>(Assembly assembly, Func<TAttributeType, string> extractValue, string defaulValue)
            where TAttributeType : Attribute;
    }
}
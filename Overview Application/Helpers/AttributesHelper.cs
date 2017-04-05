using System;
using System.Linq;
using System.Reflection;

namespace Common.Helpers
{
    public class AttributesHelper : IAttributesHelper
    {

        /// <summary>
        /// Gets or sets the customer name fallback.
        /// </summary>
        /// <value>
        /// The customer name fallback.
        /// </value>
        public string CustomerNameFallback
        {
            get { return "UndefinedCustomer"; }
        }

        /// <summary>
        /// Gets the product name fallback.
        /// </summary>
        /// <value>
        /// The product name fallback.
        /// </value>
        public string ProductNameFallback
        {
            get { return "UndefinedProduct"; }
        }

        /// <summary>
        /// Gets the atrribute value.
        /// </summary>
        /// <typeparam name="TAttributeType">The type of the attribute type.</typeparam>
        /// <param name="assembly">The assembly.</param>
        /// <param name="extractValue">The extract value.</param>
        /// <param name="defaulValue">The defaul value.</param>
        /// <returns></returns>
        public string GetAtrributeValue<TAttributeType>(Assembly assembly, Func<TAttributeType, string> extractValue, string defaulValue)
            where TAttributeType : Attribute
        {
            var attributes = assembly.GetCustomAttributes<TAttributeType>().ToArray();
            return attributes.Any() ? extractValue(attributes.First()) : defaulValue;
        }
    }
}
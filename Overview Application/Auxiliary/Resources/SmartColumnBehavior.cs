using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace OverviewApp.Auxiliary.Resources
{

    //https://www.codeproject.com/Articles/389764/A-Smart-Behavior-for-DataGrid-AutoGenerateColumn
    public class SmartColumnBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            AssociatedObject.AutoGeneratingColumn +=
                OnAutoGeneratingColumn;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.AutoGeneratingColumn -=
                OnAutoGeneratingColumn;
        }

        protected void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string displayName = GetPropertyDisplayName(e.PropertyDescriptor);
            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }
            
            else
            {
                e.Cancel = true;//this will show only properties with DisplayNameAttribute
            }
        }

        protected static string GetPropertyDisplayName(object descriptor)
        {
            PropertyDescriptor propertyDescriptor = descriptor as PropertyDescriptor;
            if (propertyDescriptor != null)
            {
                DisplayNameAttribute attr = propertyDescriptor.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
                if (attr != null && !Equals(attr, DisplayNameAttribute.Default))
                {
                    return attr.DisplayName;
                }
            }
            else
            {
                PropertyInfo pi = descriptor as PropertyInfo;
                if (pi != null)
                {
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    foreach (var att in attributes)
                    {
                        DisplayNameAttribute attribute = att as DisplayNameAttribute;
                        if (attribute != null && !Equals(attribute, DisplayNameAttribute.Default))
                        {
                            return attribute.DisplayName;
                        }
                    }
                }
            }
            return null;
        }
    }
}
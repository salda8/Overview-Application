using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace OverviewApp.Converters
{
    /// <summary>
    ///     An example for an error converter.
    ///     Should return a list of errors.
    ///     It should solve the issue with warning thrown to the debug output when
    ///     a validation goes from error to no-error.
    ///     Not working for some reason.
    ///     Here's where I got it from:
    ///     http://zhebrun.blogspot.com.au/2008/03/wpf-bug-with-validationerror.html
    ///     or: http://stackoverflow.com/q/2260616/1698987
    /// </summary>
    public class ErrorContentConverter : IValueConverter
    {
        #region

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var errors = value as ReadOnlyObservableCollection<ValidationError>;
            if (errors == null) return "";

            return errors.Count > 0
                ? errors[0].ErrorContent
                : "";
        }

       
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
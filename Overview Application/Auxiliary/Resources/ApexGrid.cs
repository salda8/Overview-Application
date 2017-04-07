﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OverviewApp.Auxiliary.Resources
{
    /// <summary>
    ///     The ApexGrid control is a Grid that supports easy definition of rows and columns.
    /// </summary>
    public class ApexGrid : Grid
    {
        #region Fields

        /// <summary>
        ///     The rows dependency property.
        /// </summary>
        private static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows", typeof(string), typeof(ApexGrid),
                new PropertyMetadata(null, OnRowsChanged));

        /// <summary>
        ///     The columns dependency property.
        /// </summary>
        private static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register("Columns", typeof(string), typeof(ApexGrid),
                new PropertyMetadata(null, OnColumnsChanged));

        #endregion Fields

        #region Properties

        /// <summary>
        ///     Gets or sets the rows.
        /// </summary>
        /// <value>The rows.</value>
        [Description("The rows property."), Category("Common Properties")]
        public string Rows
        {
            get { return (string)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the columns.
        /// </summary>
        /// <value>The columns.</value>
        [Description("The columns property."), Category("Common Properties")]
        public string Columns
        {
            get { return (string)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        #endregion Properties

        /// <summary>
        ///     Called when the rows property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="args">
        ///     The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event
        ///     data.
        /// </param>
        private static void OnRowsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            //  Get the apex grid.
            var apexGrid = dependencyObject as ApexGrid;

            //  Clear any current rows definitions.
            apexGrid?.RowDefinitions.Clear();

            //  Add each row from the row lengths definition.
            foreach (var rowLength in StringLengthsToGridLengths(apexGrid?.Rows))
                apexGrid?.RowDefinitions.Add(new RowDefinition { Height = rowLength });
        }

        /// <summary>
        ///     Called when the columns property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="args">
        ///     The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event
        ///     data.
        /// </param>
        private static void OnColumnsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            //  Get the apex grid.
            var apexGrid = dependencyObject as ApexGrid;
            if (apexGrid == null)
            {
                throw new ArgumentNullException(nameof(apexGrid));
            }
            else
            {
                //  Clear any current column definitions.
                apexGrid.ColumnDefinitions.Clear();

                //  Add each column from the column lengths definition.
                foreach (var columnLength in StringLengthsToGridLengths(apexGrid.Columns))
                    apexGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = columnLength });
            }
        }

        /// <summary>
        ///     Turns a string of lengths, such as "3*,Auto,2000" into a set of gridlength.
        /// </summary>
        /// <param name="lengths">The string of lengths, separated by commas.</param>
        /// <returns>A list of GridLengths.</returns>
        private static List<GridLength> StringLengthsToGridLengths(string lengths)
        {
            //  Create the list of GridLengths.
            var gridLengths = new List<GridLength>();

            //  If the string is null or empty, this is all we can do.
            if (string.IsNullOrEmpty(lengths))
                return gridLengths;

            //  Split the string by comma.
            var theLengths = lengths.Split(',');

            //  If we're NOT in silverlight, we have a gridlength converter
            //  we can use.
#if !SILVERLIGHT

            //  Create a grid length converter.
            var gridLengthConverter = new GridLengthConverter();

            //  Use the grid length converter to set each length.
            gridLengths.AddRange(theLengths.Select(length => gridLengthConverter.ConvertFromString(length)).Where(convertFromString => convertFromString != null).Cast<GridLength>());

#else

    //  We are in silverlight and do not have a grid length converter.
    //  We can do the conversion by hand.
      foreach(var length in theLengths)
      {
        //  Auto is easy.
        if(length == "Auto")
        {
          gridLengths.Add(new GridLength(1, GridUnitType.Auto));
        }
        else if (length.Contains("*"))
        {
          //  It's a starred value, remove the star and get the coefficient as a double.
          double coefficient = 1;
          string starVal = length.Replace("*", "");

          //  If there is a coefficient, try and convert it.
          //  If we fail, throw an exception.
          if (starVal.Length > 0 && double.TryParse(starVal, out coefficient) == false)
            throw new Exception("'" + length + "' is not a valid value.");

          //  We've handled the star value.
          gridLengths.Add(new GridLength(coefficient, GridUnitType.Star));
        }
        else
        {
          //  It's not auto or star, so unless it's a plain old pixel
          //  value we must throw an exception.
          double pixelVal = 0;
          if(double.TryParse(length, out pixelVal) == false)
            throw new Exception("'" + length + "' is not a valid value.");

          //  We've handled the star value.
          gridLengths.Add(new GridLength(pixelVal, GridUnitType.Pixel));
        }
      }
#endif

            //  Return the grid lengths.
            return gridLengths;
        }
    }
}
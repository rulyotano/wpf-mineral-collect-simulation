using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using RecopilationProyect.Utils;

namespace RecopilationProyect.View.Converters
{
    [ValueConversion(typeof(int), typeof(Brush))]
    public class IntToColorConverter: IValueConverter
    {
        #region ColorsMap

        private Dictionary<int, SolidColorBrush> _colorsMap;
        public Dictionary<int, SolidColorBrush> ColorsMap
        {
            get { return _colorsMap ?? (_colorsMap = new Dictionary<int,SolidColorBrush>()); }
        }

        private SolidColorBrush GetNewRandomColor()
        {
            var rand = new Random();

            ThreadUtils.Sleep(8);
            var r = rand.Next(0, 255);

            ThreadUtils.Sleep(8);
            var g = rand.Next(0, 255);

            ThreadUtils.Sleep(8);
            var b = rand.Next(0, 255);

            return new SolidColorBrush(Color.FromRgb((byte) r, (byte) g, (byte) b));
        }

        #endregion
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var teamNumber = (int) value;
            if (!ColorsMap.ContainsKey(teamNumber))
                ColorsMap.Add(teamNumber, GetNewRandomColor());
            return ColorsMap[teamNumber];
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CarKitProject.Converters
{
	public class FuelTankToMarginConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var percent = int.Parse(value.ToString());

			if(percent == 100)
				return new Thickness(-28,0,0,0);

			if (percent >= 94 && percent <= 99)
			{
				var tmp = 94 - percent;
				var left = (94 - percent) - 9 + tmp;
				return new Thickness(left, 0, 0, 0);
			}
				

			if (percent < 10)
				return new Thickness(-2, 0, 0, 0);

			return new Thickness(-7,0,0,0);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CarKitProject.Converters
{
	public class FuelTankToWidthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var percent = int.Parse(value.ToString());
			var width = 0d;
			if (parameter != null)
			{
				if(percent == 100)
					return new GridLength(1, GridUnitType.Star);

				if (percent == 0)
					return new GridLength(0, GridUnitType.Absolute);

				width = 100d/(100 - percent);
			}
			else
			{
				if (percent == 100)
					return new GridLength(0, GridUnitType.Absolute);

				if(percent == 0)
					return new GridLength(1, GridUnitType.Star);

				width = 100d / percent;
				
			}

			return new GridLength(width, GridUnitType.Star);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

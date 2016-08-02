using CarKitProject.Enums;
using Xamarin.Forms;

namespace CarKitProject.Helpers
{
	public static class ImageLoader
	{
		public static ImageSource LoadImageFromResources(WeatherCondition weatherCondition)
		{
			var resourceFullPath = "WeatherApp.Droid.Resources.weather_conditions." + weatherCondition + ".png";
			return ImageSource.FromResource(resourceFullPath);
		}

		public static ImageSource LoadImageFromResources(string resourceName)
		{
			var resourceFullPath = "WeatherApp.Droid.Resources.drawable." + resourceName;
			return ImageSource.FromResource(resourceFullPath);
		}
	}
}

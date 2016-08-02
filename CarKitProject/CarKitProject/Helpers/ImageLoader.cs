using CarKitProject.Enums;
using Xamarin.Forms;

namespace CarKitProject.Helpers
{
	public static class ImageLoader
	{
		public static ImageSource LoadImageFromResources(WeatherCondition weatherCondition)
		{
			var resourceFullPath = "CarKitProject.Resources.weather_conditions." + weatherCondition + ".png";
			return ImageSource.FromResource(resourceFullPath);
		}

		public static ImageSource LoadImageFromResources(string resourceName)
		{
			var resourceFullPath = "CarKitProject.Resources." + resourceName;
			return ImageSource.FromResource(resourceFullPath);
		}
	}
}

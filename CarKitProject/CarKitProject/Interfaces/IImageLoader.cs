using CarKitProject.Enums;
using Xamarin.Forms;

namespace CarKitProject.Interfaces
{
	public interface IImageLoader
	{
		ImageSource LoadImageFromResources(WeatherCondition resourceName);
		ImageSource LoadImageFromResources(string resourceName);
	}
}

using CarKitProject.Helpers;
using CarKitProject.ViewModels;
using Xamarin.Forms;

namespace CarKitProject.UserControls
{
	public partial class WeatherClockControl : ContentView
	{
		public WeatherClockControl()
		{
			InitializeComponent();

			BindingContext = new WeatherClockViewModel();

			var backgroundSource = ImageLoader.LoadImageFromResources("WeatherClockBackgroundRound.png");

			Background.Source = backgroundSource;

			var firstSource = ImageLoader.LoadImageFromResources("ClockRound.png");
			FirstClock.Source = firstSource;

			var secondSource = ImageLoader.LoadImageFromResources("ClockRound.png");
			SecondClock.Source = secondSource;
		}
	}
}

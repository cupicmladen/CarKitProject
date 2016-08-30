using CarKitProject.Helpers;
using Xamarin.Forms;

namespace CarKitProject.UserControls
{
	public partial class IndicatorControl : ContentView
	{
		public IndicatorControl()
		{
			InitializeComponent();

			var backgroundSource = ImageLoader.LoadImageFromResources("IndicatorBackground.png");
			Background.Source = backgroundSource;
		}
	}
}

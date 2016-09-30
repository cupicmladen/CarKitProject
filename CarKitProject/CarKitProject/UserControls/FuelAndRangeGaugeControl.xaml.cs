using CarKitProject.Helpers;
using Xamarin.Forms;

namespace CarKitProject.UserControls
{
	public partial class FuelAndRangeGaugeControl : ContentView
	{
		public FuelAndRangeGaugeControl()
		{
			InitializeComponent();

			var backgroundSource = ImageLoader.LoadImageFromResources("opel_insignia-icon.png");
			CarImage.Source = backgroundSource;
		}
	}
}

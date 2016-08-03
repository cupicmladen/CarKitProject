using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarKitProject.Helpers;
using CarKitProject.ViewModels;
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

			BindingContext = new IndicatorViewModel
			{
				Unit = "km/h",
				ValueDescription = "Speed",
				Value = 78
			};
		}
	}
}

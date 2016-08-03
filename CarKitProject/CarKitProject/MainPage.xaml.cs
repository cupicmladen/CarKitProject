using CarKitProject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarKitProject.Interfaces;
using CarKitProject.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CarKitProject
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

			var imageSource = ImageLoader.LoadImageFromResources("hexagons_texture_2560x1440.jpg");
			BackgroundImg.Source = imageSource;

			var currentLocationService = DependencyService.Get<ICurrentLocationService>();
			currentLocationService.RaiseLocationChanged += CurrentLocationService_RaiseLocationChanged;
		}

		private void CurrentLocationService_RaiseLocationChanged(LocationCoordinates location)
		{
			Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(Map.VisibleRegion.Radius.Kilometers)));
			//label.Text = "" + location.Latitude + Environment.NewLine + location.Longitude;
		}
	}
}

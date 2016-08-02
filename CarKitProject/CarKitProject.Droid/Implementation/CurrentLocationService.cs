using System.Linq;
using Android.App;
using Android.Locations;
using CarKitProject.Droid.Implementation;
using CarKitProject.Interfaces;
using CarKitProject.Models;

[assembly: Xamarin.Forms.Dependency(typeof(CurrentLocationService))]
namespace CarKitProject.Droid.Implementation
{
	public class CurrentLocationService : ICurrentLocationService
	{
		public CurrentLocationService()
		{
		}

		public event LocationChangedEvent RaiseLocationChanged;

		public LocationCoordinates GetCurrentLocation()
		{
			var location = _locationManager.GetLastKnownLocation(_locationProvider);
			//return new LocationCoordinates
			//{
			//	Longitude = location.Longitude,
			//	Latitude = location.Latitude
			//};

			return new LocationCoordinates
			{
				Longitude = 19.8356876373291,
				Latitude = 45.2554626464844
			};
		}

		public void InitializeLocationManager()
		{
			var context = Application.Context;
			_locationManager = (LocationManager)context.GetSystemService("location");
			var criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Fine
			};

			var acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

			if (acceptableLocationProviders.Any())
			{
				_locationProvider = acceptableLocationProviders.First();
			}
			else
			{
				_locationProvider = string.Empty;
			}

			var gpsLocationListener = new GpsLocationListener();
			gpsLocationListener.RaiseLocationChanged +=
				location => RaiseLocationChanged?.Invoke(location);

			_locationManager.RequestLocationUpdates(_locationProvider, 0, 0, gpsLocationListener);
		}

		private LocationManager _locationManager;
		private string _locationProvider;
	}
}
using System;
using Android.Locations;
using Android.OS;
using CarKitProject.Interfaces;
using CarKitProject.Models;

namespace CarKitProject.Droid.Implementation
{
	public class GpsLocationListener : Java.Lang.Object, ILocationListener
	{
		public event LocationChangedEvent RaiseLocationChanged;

		public GpsLocationListener()
		{
			_locationCoordinates = new LocationCoordinates();
		}

		public void OnLocationChanged(Location location)
		{
			_locationCoordinates.Latitude = location.Latitude;
			_locationCoordinates.Longitude = location.Longitude;
			RaiseLocationChanged?.Invoke(_locationCoordinates);
		}

		public void OnProviderDisabled(string provider)
		{
			throw new NotImplementedException();
		}

		public void OnProviderEnabled(string provider)
		{
			throw new NotImplementedException();
		}

		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
			//throw new NotImplementedException();
		}

		private LocationCoordinates _locationCoordinates;
	}
}
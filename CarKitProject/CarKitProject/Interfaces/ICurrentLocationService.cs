using CarKitProject.Models;

namespace CarKitProject.Interfaces
{
	public interface ICurrentLocationService
	{
		void InitializeLocationManager();
		LocationCoordinates GetCurrentLocation();
		event LocationChangedEvent RaiseLocationChanged;
	}

	public delegate void LocationChangedEvent(LocationCoordinates location);
}

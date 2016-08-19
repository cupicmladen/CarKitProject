using System.Linq;
using System.Threading.Tasks;
using Android.Bluetooth;
using CarKitProject.Droid.OBD;
using CarKitProject.OBD;
using Java.Util;

[assembly: Xamarin.Forms.Dependency(typeof(BtConnectionManager))]
namespace CarKitProject.Droid.OBD
{
	public class BtConnectionManager : IBtConnectionManager
	{
		public void ConnectToOdb()
		{
			var btAdapter = BluetoothAdapter.DefaultAdapter;
			var devices = btAdapter.BondedDevices.ToList();

			if (!devices.Any())
				return;

			var device = devices.FirstOrDefault(it => it.Name.Contains("OBD"));
			if (device == null)
				return;

			_socket = device.CreateRfcommSocketToServiceRecord(UUID.RandomUUID());
			_socket.Connect();
		}

		BluetoothSocket _socket;
	}
}
using System;

namespace CarKitProject.OBD
{
	public interface IBtConnectionManager
	{
		bool ConnectToObd();
		void SendCommand(string command);
		void StartReadingData();
		void StopReadingData();
		event Action<string> DataReceived;
	}
}

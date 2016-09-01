using System;

namespace CarKitProject.OBD.Commands
{
	public class CoolantTemperatureCommand : ObdCommand
	{
		public CoolantTemperatureCommand()
		{
			Command = "0105";
			BytesReturned = 1;
		}

		public override void FormatResult(string hexValue)
		{
			var result = Convert.ToInt64(hexValue, 16);
		}

		private int _value;
	}
}

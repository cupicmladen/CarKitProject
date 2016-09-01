using System;
using System.Collections.Generic;

namespace CarKitProject.OBD.Commands
{
	public class CoolantTemperatureCommand : ObdCommand
	{
		public CoolantTemperatureCommand()
		{
			Command = "0105";
			CommandShort = "05";
			Unit = "C";
		}

		public override void CalculateValue(IList<string> hexValue)
		{
			Value = "" + (Convert.ToInt64(hexValue[0], 16) - 40);
		}
	}
}

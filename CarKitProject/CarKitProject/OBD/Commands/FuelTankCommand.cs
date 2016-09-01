using System;
using System.Collections.Generic;

namespace CarKitProject.OBD.Commands
{
	public class FuelTankCommand : ObdCommand
	{
		public FuelTankCommand()
		{
			Command = "012F";
			CommandShort = "2F";
			Unit = "%";
		}

		public override void CalculateValue(IList<string> hexValue)
		{
			Value = "" + ((100/255)* Convert.ToInt64(hexValue[0], 16));
		}
	}
}

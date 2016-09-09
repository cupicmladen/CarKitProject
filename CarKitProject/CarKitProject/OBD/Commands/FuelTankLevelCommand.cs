using System;
using System.Collections.Generic;

namespace CarKitProject.OBD.Commands
{
	public class FuelTankLevelCommand : ObdCommand
	{
		public FuelTankLevelCommand()
		{
			Command = "012F";
			CommandShort = "2F";
			Unit = "%";
			Value = "0";
		}

		public int GetFuelTankLevel => int.Parse(Value);

		public override void CalculateValue(IList<string> hexValue)
		{
			Value = "" + (0.392*Convert.ToInt64(hexValue[0], 16));
		}
	}
}

using System;
using System.Collections.Generic;

namespace CarKitProject.OBD.Commands
{
	public class EngineFuelRateCommand : ObdCommand
	{
		public EngineFuelRateCommand()
		{
			Command = "015E";
			CommandShort = "5E";
			Unit = "L/H";
			Value = "0";
		}

		public int GetEngineFuelRate => int.Parse(Value);

		public override void CalculateValue(IList<string> hexValue)
		{
			var a = Convert.ToInt64(hexValue[0], 16);
			var b = Convert.ToInt64(hexValue[1], 16);

			Value = "" + ((256*a) + b)/20;
		}
	}
}

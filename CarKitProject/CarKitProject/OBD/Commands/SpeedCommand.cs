using System;
using System.Collections.Generic;

namespace CarKitProject.OBD.Commands
{
	public class SpeedCommand : ObdCommand
	{
		public SpeedCommand()
		{
			Command = "010D";
			CommandShort = "0D";
			Unit = "Km/H";
			Value = "0";
		}

		public int GetSpeed => int.Parse(Value);

		public override void CalculateValue(IList<string> hexValue)
		{
			Value = "" + Convert.ToInt64(hexValue[0], 16);
		}
	}
}

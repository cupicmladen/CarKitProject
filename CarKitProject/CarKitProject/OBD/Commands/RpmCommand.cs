using System;
using System.Collections.Generic;

namespace CarKitProject.OBD.Commands
{
	public class RpmCommand : ObdCommand
	{
		public RpmCommand()
		{
			Command = "010C";
			CommandShort = "0C";
			Unit = "RPM";
		}

		public override void CalculateValue(IList<string> hexValue)
		{
			var a = Convert.ToInt64(hexValue[0], 16);
			var b = Convert.ToInt64(hexValue[1], 16);

			Value = "" + ((256*a) + b)/4;
		}
	}
}

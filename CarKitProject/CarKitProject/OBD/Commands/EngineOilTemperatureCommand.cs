using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarKitProject.OBD.Commands
{
	public class EngineOilTemperatureCommand : ObdCommand
	{
		public EngineOilTemperatureCommand()
		{
			Command = "015C";
			CommandShort = "5C";
			Value = "0";
			Unit = "C";
		}

		public override void CalculateValue(IList<string> hexValue)
		{
			Value = "" + (Convert.ToInt64(hexValue[0], 16) - 40);
		}
	}
}

using System;

namespace CarKitProject.OBD.Commands
{
	public class RpmCommand : ObdCommand
	{
		public RpmCommand()
		{
			Command = "010C";
			BytesReturned = 2;
		}

		public override void FormatResult(string hexValue)
		{
			var result = Convert.ToInt64(hexValue, 16);
		}

		private int _value;
	}
}

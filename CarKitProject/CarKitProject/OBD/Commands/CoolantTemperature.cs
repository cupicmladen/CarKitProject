using System;

namespace CarKitProject.OBD.Commands
{
	public class CoolantTemperature : ObdCommand
	{
		public CoolantTemperature()
		{
			Command = "0105";
		}

		public int Value
		{
			get { return _value; }
			set { _value = value; }
		}

		public override void FormatResult(string hexValue)
		{
			var result = Convert.ToInt64(hexValue, 16);
		}

		private int _value;
	}
}

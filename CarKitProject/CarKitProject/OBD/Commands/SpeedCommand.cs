﻿using System;

namespace CarKitProject.OBD.Commands
{
	public class SpeedCommand : ObdCommand
	{
		public SpeedCommand()
		{
			Command = "010D";
		}

		public override void FormatResult(string hexValue)
		{
			var result = Convert.ToInt64(hexValue, 16);
		}

		private int _value;

	}
}

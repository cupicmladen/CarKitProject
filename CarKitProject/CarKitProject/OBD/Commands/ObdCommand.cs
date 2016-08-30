using System;

namespace CarKitProject.OBD.Commands
{
	public class ObdCommand
	{
		public string Command
		{
			get { return _command; }
			set { _command = value; }
		}

		public string Response
		{
			get { return _response; }
			set { _response = value; }
		}

		public string Value
		{
			get { return _value; }
			set { _value = value; }
		}

		public string Unit
		{
			get { return _unit; }
			set { _unit = value; }
		}

		public string FormattedCommand => Command + "\r";

		public virtual void FormatResult(string hexValue)
		{
			throw new NotImplementedException();
		}

		private string _command;
		private string _response;
		private string _value;
		private string _unit;
	}
}

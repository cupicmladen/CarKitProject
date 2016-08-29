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

		public virtual void FormatResult(string hexValue)
		{
			throw new NotImplementedException();
		}

		private string _command;
		private string _response;
	}
}

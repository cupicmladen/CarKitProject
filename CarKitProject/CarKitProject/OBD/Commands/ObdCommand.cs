using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarKitProject.Annotations;

namespace CarKitProject.OBD.Commands
{
	public class ObdCommand : INotifyPropertyChanged
	{
		public string Command
		{
			get { return _command; }
			set { _command = value; }
		}

		public string CommandShort
		{
			get { return _commandShort; }
			set { _commandShort = value; }
		}

		public string Value
		{
			get { return _value; }
			set
			{
				_value = value;
				OnPropertyChanged("Value");
			}
		}

		public string Unit
		{
			get { return _unit; }
			set { _unit = value; }
		}

		public string FormattedCommand => Command + " 1\r";

		public virtual void CalculateValue(IList<string> hexValue)
		{
			throw new NotImplementedException();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private string _command;
		private string _commandShort;
		private string _value;
		private string _unit;
	}
}

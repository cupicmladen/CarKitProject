using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarKitProject.Properties;

namespace CarKitProject.OBD.Commands
{
	public class CalculatedViewModel : INotifyPropertyChanged
	{
		public CalculatedViewModel()
		{
			Value = "0";
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

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private string _value;
	}
}

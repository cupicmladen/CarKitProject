using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarKitProject.Annotations;

namespace CarKitProject.ViewModels
{
	public class IndicatorViewModel : INotifyPropertyChanged
	{
		public double Value
		{
			get { return _value; }
			set
			{
				_value = value;
				OnPropertyChanged("Value");
			}
		}

		public string Unit { get; set; }
		public string ValueDescription { get; set; }

		private double _value;

		public event PropertyChangedEventHandler PropertyChanged;
		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

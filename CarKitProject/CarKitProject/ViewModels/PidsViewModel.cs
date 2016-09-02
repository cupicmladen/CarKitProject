using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using CarKitProject.Annotations;
using CarKitProject.Extensions;
using CarKitProject.OBD;
using CarKitProject.OBD.Commands;
using Xamarin.Forms;

namespace CarKitProject.ViewModels
{
	public class PidsViewModel : INotifyPropertyChanged
	{
		public PidsViewModel()
		{
			TempResponseList = string.Empty;

			RpmCommand = new RpmCommand();
			SpeedCommand = new SpeedCommand();
			CoolantTemperatureCommand = new CoolantTemperatureCommand();
			FuelTankCommand = new FuelTankCommand();
		}

		public bool ConnectToObd()
		{
			_btManager = DependencyService.Get<IBtConnectionManager>();
			_btManager.ConnectToObd();

			if (_btManager == null || !_btManager.IsConnected)
				return false;

			_btManager.DataReceived += BtDataReceived;
			_btManager.StartReadingData();

			return _btManager.IsConnected;
		}

		private void BtDataReceived(string command)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				//TempResponseList += DateTime.Now.ToString("mm:ss.fff") + ": " + command + Environment.NewLine;

				var commandsSplit = command.Split(new[] { '>' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < commandsSplit.Length; i++)
				{
					var response = commandsSplit[i].SplitAndRefactor(2);

					if (response.Count == 0)
						continue;

					var obdCommand = FindCommand(response[0]);

					if (obdCommand == null)
						continue;

					response.RemoveAt(0);
					obdCommand.CalculateValue(response);

					if(_calculateGear)
						CalculateGear();
				}
			});
		}

		private ObdCommand FindCommand(string commandShort)
		{
			if (RpmCommand.CommandShort == commandShort)
				return RpmCommand;

			if (SpeedCommand.CommandShort == commandShort)
				return SpeedCommand;

			if (CoolantTemperatureCommand.CommandShort == commandShort)
				return CoolantTemperatureCommand;

			if (FuelTankCommand.CommandShort == commandShort)
				return FuelTankCommand;

			return null;
		}

		public void SendCommand(string command)
		{
			_btManager.SendCommand(command);
		}

		private int rpmCounter = 0;
		private int speedCounter = 0;
		private int tempCounter = 0;
		private int fuelCounter = 0;

		public void LoadData()
		{
			if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested)
				_cancellationTokenSource = new CancellationTokenSource();

			
			Task.Factory.StartNew((o) =>
			{
				var frequency = 0;

				while (!_cancellationTokenSource.IsCancellationRequested)
				{
					_btManager.SendCommand(RpmCommand.FormattedCommand);
					_btManager.SendCommand(SpeedCommand.FormattedCommand);

					rpmCounter++;
					speedCounter++;

					if (frequency % 1000 == 0)
					{
						_btManager.SendCommand(CoolantTemperatureCommand.FormattedCommand);
						_btManager.SendCommand(FuelTankCommand.FormattedCommand);

						tempCounter++;
						fuelCounter++;
					}

					frequency++;

					if (frequency == int.MaxValue)
						frequency = 0;
				}
			}, TaskCreationOptions.LongRunning, _cancellationTokenSource.Token);
		}

		public void StopReadingData()
		{
			_cancellationTokenSource.Cancel();

			RpmCommand.Counter = rpmCounter;
			SpeedCommand.Counter = speedCounter;
			CoolantTemperatureCommand.Counter = tempCounter;
			FuelTankCommand.Counter = fuelCounter;
		}

		public void UseOneResponse(bool value)
		{
			_calculateGear = value;
			//SpeedCommand.Value = "64";
			//RpmCommand.Value = "3500";
			//CalculateGear();
		}

		private void CalculateGear()
		{
			var result = (decimal)SpeedCommand.GetSpeed / RpmCommand.GetRpm;

			if (result > 0 && result <= 0.01m)
			{
				Gear = 1;
			}
			else if (result >= 0.01m && result <= 0.02m)
			{
				Gear = 2;
			}
			else if (result >= 0.02m && result <= 0.03m)
			{
				Gear = 3;
			}
			else if (result >= 0.03m && result <= 0.04m)
			{
				Gear = 4;
			}
			else if (result >= 0.04m && result <= 0.05m)
			{
				Gear = 5;
			}
			else
			{
				Gear = 0;
			}
		}

		#region ObdCommands

		public RpmCommand RpmCommand
		{
			get { return _rpmCommand; }
			set { _rpmCommand = value; }
		}

		public SpeedCommand SpeedCommand
		{
			get { return _speedCommand; }
			set { _speedCommand = value; }
		}

		public CoolantTemperatureCommand CoolantTemperatureCommand
		{
			get { return _coolantCoolantTemperatureCommand; }
			set { _coolantCoolantTemperatureCommand = value; }
		}

		public FuelTankCommand FuelTankCommand
		{
			get { return _fuelTankCommand; }
			set { _fuelTankCommand = value; }
		}

		#endregion

		public string TempResponseList
		{
			get { return _tempResponseList; }
			set
			{
				_tempResponseList = value;
				OnPropertyChanged("TempResponseList");
			}
		}

		public int Gear
		{
			get { return _gear; }
			set
			{
				_gear = value;
				OnPropertyChanged("Gear");
			}
		}

		#region Handlers

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		private CancellationTokenSource _cancellationTokenSource;
		private IBtConnectionManager _btManager;

		private RpmCommand _rpmCommand;
		private SpeedCommand _speedCommand;
		private CoolantTemperatureCommand _coolantCoolantTemperatureCommand;
		private FuelTankCommand _fuelTankCommand;
		private int _gear;

		private string _tempResponseList;
		private bool _useOneResponse = true;

		private bool _calculateGear = false;
	}
}

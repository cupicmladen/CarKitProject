using System;
using System.Collections.Generic;
using System.ComponentModel;
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
	public class ObdCommandsViewModel : INotifyPropertyChanged
	{
		public ObdCommandsViewModel()
		{
			RpmCommand = new RpmCommand();
			SpeedCommand = new SpeedCommand();
			CoolantTemperatureCommand = new CoolantTemperatureCommand();
			EngineOilTemperatureCommand = new EngineOilTemperatureCommand();
			CalculatedEngineLoadCommand = new CalculatedEngineLoadCommand();
			FuelTankLevelCommand = new FuelTankLevelCommand();
			MafAirFlowRateCommand = new MafAirFlowRateCommand();
			Gear = new CalculatedCommand();
			CurrentConsumptionCommand = new CalculatedCommand();
		}

		#region Public Methods

		public bool ConnectToObdAndInitialize()
		{
			_btManager = DependencyService.Get<IBtConnectionManager>();
			_btManager.ConnectToObd();

			if (_btManager == null || !_btManager.IsConnected)
				return false;

			//var initCommands = new List<string> { "ati\r", "atl0\r", "ath0\r", "ats0\r", "atsp6\r", "atcra 7e8\r", "0100\r", "0100\r", "atma\r"};

			//var index = 0;
			//Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
			//{
			//	_btManager.SendCommand(initCommands[index]);
			//	index++;

			//	if (index == initCommands.Count)
			//		return false;

			//	return true;
			//});

			_btManager.DataReceived += BtDataReceived;
			_btManager.StartReadingData();

			//LoadData();
			Log += "Connected: " + _btManager.IsConnected + Environment.NewLine;

			return _btManager.IsConnected;
		}

		public void StopReadingData()
		{
			_cancellationTokenSource.Cancel();
		}

		public void SendCommand(string command)
		{
			_btManager.SendCommand(command);
		}

		#endregion

		#region PrivateMethods

		private void BtDataReceived(string command)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Log += command + Environment.NewLine;
				//TotalNumberOfCommands++;
				var commandsSplit = command.Split(new[] { '>' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < commandsSplit.Length; i++)
				{
					var response = commandsSplit[i].SplitAndRefactor(2);

					if (response.Count == 0)
					{
						continue;
					}

					var obdCommand = FindCommand(response[0]);

					if (obdCommand == null)
					{
						continue;
					}

					response.RemoveAt(0);
					obdCommand.CalculateValue(response);
					//CalculateGear();
					//CalculateCurrentConsumption1();
				}
			});
		}

		private void CalculateCurrentConsumption1()
		{
			if (SpeedCommand.GetSpeed != 0 && MafAirFlowRateCommand.GetMafAirFlowRate != 0)
			{
				var kilometersPerLiter = SpeedCommand.GetSpeed / (MafAirFlowRateCommand.GetMafAirFlowRate / 4.08333333);
				var litersPer100Kilometers = 100d / kilometersPerLiter;
				var value1 = $"{litersPer100Kilometers:0.00}";
				var value2 = $"{CalculateCurrentConsumption2():0.00}";
				Consumption += "Speed: " + SpeedCommand.Value + " RPM: " + RpmCommand.Value + " Consumption1: " + value1 + " Consumption2: " + value2 + Environment.NewLine;
				CurrentConsumptionCommand.Value = value2;
			}
		}

		private double CalculateCurrentConsumption2()
		{
			var mpg = (SpeedCommand.GetSpeed*7.718)/MafAirFlowRateCommand.GetMafAirFlowRate;
			var litersPer100Kilometers = (100*3.785411784)/(1.609344*mpg);
			return litersPer100Kilometers;
		}

		private ObdCommand FindCommand(string commandShort)
		{
			if (RpmCommand.CommandShort == commandShort)
				return RpmCommand;

			if (SpeedCommand.CommandShort == commandShort)
				return SpeedCommand;

			if (CoolantTemperatureCommand.CommandShort == commandShort)
				return CoolantTemperatureCommand;

			if (EngineOilTemperatureCommand.CommandShort == commandShort)
				return EngineOilTemperatureCommand;

			if (CalculatedEngineLoadCommand.CommandShort == commandShort)
				return CalculatedEngineLoadCommand;

			if (FuelTankLevelCommand.CommandShort == commandShort)
				return FuelTankLevelCommand;

			if (MafAirFlowRateCommand.CommandShort == commandShort)
				return MafAirFlowRateCommand;

			return null;
		}

		private void LoadData()
		{
			if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested)
				_cancellationTokenSource = new CancellationTokenSource();

			Task.Factory.StartNew((o) =>
			{
				var frequency = 0;

				while (!_cancellationTokenSource.IsCancellationRequested)
				{
					if (frequency == 0)
						_btManager.SendCommand(RpmCommand.FormattedCommand);
					else if (frequency == 1)
						_btManager.SendCommand(SpeedCommand.FormattedCommand);
					else if (frequency == 2)
						_btManager.SendCommand(MafAirFlowRateCommand.FormattedCommand);

					if (frequency == 2)
						frequency = -1;

					frequency++;

					Task.Delay(50).Wait();

					//_btManager.SendCommand(RpmCommand.FormattedCommand);
					//_btManager.SendCommand(SpeedCommand.FormattedCommand);

					//if (frequency % 1000 == 0)
					//{
					//	_btManager.SendCommand(CoolantTemperatureCommand.FormattedCommand);
					//	_btManager.SendCommand(EngineOilTemperatureCommand.FormattedCommand);
					//}

					//frequency++;

					//if (frequency == int.MaxValue)
					//	frequency = 0;

				}
			}, TaskCreationOptions.LongRunning, _cancellationTokenSource.Token);
		}

		private void CalculateGear()
		{
			if (RpmCommand.GetRpm == 0)
				return;

			var result = (decimal)SpeedCommand.GetSpeed / RpmCommand.GetRpm;

			if (result > 0 && result <= 0.01m)
			{
				Gear.Value = "1";
			}
			else if (result >= 0.01m && result <= 0.02m)
			{
				Gear.Value = "2";
			}
			else if (result >= 0.02m && result <= 0.03m)
			{
				Gear.Value = "3";
			}
			else if (result >= 0.03m && result <= 0.04m)
			{
				Gear.Value = "4";
			}
			else if (result >= 0.04m && result <= 0.05m)
			{
				Gear.Value = "5";
			}
			else if (result >= 0.05m && result <= 0.06m)
			{
				Gear.Value = "6";
			}
			else
			{
				Gear.Value = "0";
			}
		}

		#endregion

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

		public EngineOilTemperatureCommand EngineOilTemperatureCommand
		{
			get { return _engineOilTemperatureCommand; }
			set { _engineOilTemperatureCommand = value; }
		}

		public CalculatedEngineLoadCommand CalculatedEngineLoadCommand
		{
			get { return _calculatedEngineLoadCommand; }
			set { _calculatedEngineLoadCommand = value; }
		}

		public FuelTankLevelCommand FuelTankLevelCommand
		{
			get { return _fuelTankLevelCommand; }
			set { _fuelTankLevelCommand = value; }
		}

		public MafAirFlowRateCommand MafAirFlowRateCommand
		{
			get { return _mafAirFlowRateCommand; }
			set { _mafAirFlowRateCommand = value; }
		}

		public CalculatedCommand Gear
		{
			get { return _gear; }
			set { _gear = value; }
		}

		public CalculatedCommand CurrentConsumptionCommand
		{
			get { return _currentConsumptionCommand; }
			set { _currentConsumptionCommand = value; }
		}

		public int TotalNumberOfCommands
		{
			get { return _totalNumberOfCommands; }
			set
			{
				_totalNumberOfCommands = value;
				OnPropertyChanged("TotalNumberOfCommands");
			}
		}

		#endregion

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
		private EngineOilTemperatureCommand _engineOilTemperatureCommand;
		private CalculatedEngineLoadCommand _calculatedEngineLoadCommand;
		private FuelTankLevelCommand _fuelTankLevelCommand;
		private MafAirFlowRateCommand _mafAirFlowRateCommand;
		private CalculatedCommand _currentConsumptionCommand;
		private CalculatedCommand _gear;

		public string Consumption = "";
		private int _totalNumberOfCommands = 0;
		public string Log = "";
	}
}

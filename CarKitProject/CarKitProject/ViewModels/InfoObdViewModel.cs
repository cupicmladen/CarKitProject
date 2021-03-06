﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using CarKitProject.Extensions;
using CarKitProject.OBD;
using CarKitProject.OBD.Commands;
using CarKitProject.Properties;
using Xamarin.Forms;

namespace CarKitProject.ViewModels
{
	public class InfoObdViewModel : INotifyPropertyChanged
	{
		public InfoObdViewModel()
		{
			RpmCommand = new RpmViewModel();
			SpeedCommand = new SpeedViewModel();
			CoolantTemperatureCommand = new CoolantTemperatureViewModel();
			EngineOilTemperatureCommand = new EngineOilTemperatureViewModel();
			CalculatedEngineLoadCommand = new CalculatedEngineLoadViewModel();
			FuelTankLevelCommand = new FuelTankLevelViewModel();
			MafAirFlowRateCommand = new MafAirFlowRateViewModel();
			GearCommand = new CalculatedViewModel();
			CurrentConsumptionCommand = new CalculatedViewModel();
		}

		#region Public Methods

		public bool ConnectToObdAndInitialize()
		{
			_btManager = DependencyService.Get<IBtConnectionManager>();
			_btManager.ConnectToObd();

			if (_btManager == null || !_btManager.IsConnected)
				return false;

			var initCommands = new List<string> { "ati\r", "atl0\r", "ath0\r", "ats0\r", "atsp6\r", "atcra 7e8\r", "0100\r", "0100\r" };

			var index = 0;
			Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
			{
				_btManager.SendCommand(initCommands[index]);
				index++;

				if (index == initCommands.Count)
					return false;

				return true;
			});

			_btManager.DataReceived += BtDataReceived;
			_btManager.StartReadingData();

			LoadData();

			return _btManager.IsConnected;
		}

		public void StopReadingData()
		{
			_cancellationTokenSource.Cancel();
		}

		#endregion

		#region PrivateMethods

		private void BtDataReceived(string command)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
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

					CalculateGear();
					CalculateCurrentConsumption();
				}
			});
		}

		private void CalculateCurrentConsumption()
		{
			if (SpeedCommand.GetSpeed != 0 && MafAirFlowRateCommand.GetMafAirFlowRate != 0)
			{
				var kilometersPerLiter = SpeedCommand.GetSpeed / (MafAirFlowRateCommand.GetMafAirFlowRate / 4.08333333);
				var litersPer100Kilometers = 100d / kilometersPerLiter;
				CurrentConsumptionCommand.Value = "" + litersPer100Kilometers;
			}
		}

		private ObdViewModel FindCommand(string commandShort)
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
				GearCommand.Value = "1";
			}
			else if (result >= 0.01m && result <= 0.02m)
			{
				GearCommand.Value = "2";
			}
			else if (result >= 0.02m && result <= 0.03m)
			{
				GearCommand.Value = "3";
			}
			else if (result >= 0.03m && result <= 0.04m)
			{
				GearCommand.Value = "4";
			}
			else if (result >= 0.04m && result <= 0.05m)
			{
				GearCommand.Value = "5";
			}
			else if (result >= 0.05m && result <= 0.06m)
			{
				GearCommand.Value = "6";
			}
			else
			{
				GearCommand.Value = "0";
			}
		}

		#endregion

		#region ObdCommands

		public RpmViewModel RpmCommand
		{
			get { return _rpmCommand; }
			set { _rpmCommand = value; }
		}

		public SpeedViewModel SpeedCommand
		{
			get { return _speedCommand; }
			set { _speedCommand = value; }
		}

		public CoolantTemperatureViewModel CoolantTemperatureCommand
		{
			get { return _coolantCoolantTemperatureCommand; }
			set { _coolantCoolantTemperatureCommand = value; }
		}

		public EngineOilTemperatureViewModel EngineOilTemperatureCommand
		{
			get { return _engineOilTemperatureCommand; }
			set { _engineOilTemperatureCommand = value; }
		}

		public CalculatedEngineLoadViewModel CalculatedEngineLoadCommand
		{
			get { return _calculatedEngineLoadCommand; }
			set { _calculatedEngineLoadCommand = value; }
		}

		public FuelTankLevelViewModel FuelTankLevelCommand
		{
			get { return _fuelTankLevelCommand; }
			set { _fuelTankLevelCommand = value; }
		}

		public MafAirFlowRateViewModel MafAirFlowRateCommand
		{
			get { return _mafAirFlowRateCommand; }
			set { _mafAirFlowRateCommand = value; }
		}

		public CalculatedViewModel GearCommand
		{
			get { return _gearCommand; }
			set { _gearCommand = value; }
		}

		public CalculatedViewModel CurrentConsumptionCommand
		{
			get { return _currentConsumptionCommand; }
			set { _currentConsumptionCommand = value; }
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

		private RpmViewModel _rpmCommand;
		private SpeedViewModel _speedCommand;
		private CoolantTemperatureViewModel _coolantCoolantTemperatureCommand;
		private EngineOilTemperatureViewModel _engineOilTemperatureCommand;
		private CalculatedEngineLoadViewModel _calculatedEngineLoadCommand;
		private FuelTankLevelViewModel _fuelTankLevelCommand;
		private MafAirFlowRateViewModel _mafAirFlowRateCommand;
		private CalculatedViewModel _currentConsumptionCommand;
		private CalculatedViewModel _gearCommand;
	}
}

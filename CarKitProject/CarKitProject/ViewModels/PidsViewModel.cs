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
			EngineOilTemperatureCommand = new EngineOilTemperatureCommand();
			CalculatedEngineLoadCommand = new CalculatedEngineLoadCommand();
			FuelTankLevelCommand = new FuelTankLevelCommand();
			EngineFuelRateCommand = new EngineFuelRateCommand();
			MafAirFlowRateCommand = new MafAirFlowRateCommand();

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
				TempResponseList += command + Environment.NewLine;
				
				ResponsesTotalLineCounter++;

				var commandsSplit = command.Split(new[] { '>' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < commandsSplit.Length; i++)
				{
					var response = commandsSplit[i].SplitAndRefactor(2);

					if (response.Count == 0)
					{
						InvalidResponses++;
						continue;
					}
						

					var obdCommand = FindCommand(response[0]);

					if (obdCommand == null)
					{
						InvalidResponses++;
						continue;
					}

					response.RemoveAt(0);
					obdCommand.CalculateValue(response);

					ValidResponsesWithSplitCounter++;

					if (_calculateGear)
					{
						//CalculateGear();
						CalculateCurrentConsumption();
					}
						
				}
			});
		}

		private void CalculateCurrentConsumption()
		{
			if (SpeedCommand.GetSpeed != 0 && MafAirFlowRateCommand.GetMafAirFlowRate != 0)
			{
				CalculatedCurrentConsumptionCommand = "" + SpeedCommand.GetSpeed/(MafAirFlowRateCommand.GetMafAirFlowRate/4.08333333);
			}
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

			if (EngineFuelRateCommand.CommandShort == commandShort)
				return EngineFuelRateCommand;

			if (MafAirFlowRateCommand.CommandShort == commandShort)
				return MafAirFlowRateCommand;

			return null;
		}

		public void SendCommand(string command)
		{
			_btManager.SendCommand(command);
		}

		public void LoadData()
		{
			if (_cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested)
				_cancellationTokenSource = new CancellationTokenSource();

			StartTime = DateTime.Now;

			Task.Factory.StartNew((o) =>
			{
				var frequency = 0;

				while (!_cancellationTokenSource.IsCancellationRequested)
				{
					if(frequency == 0)
						TestSendCommand(RpmCommand);
					else if(frequency == 1)
						TestSendCommand(SpeedCommand);
					else if(frequency == 2)
						TestSendCommand(MafAirFlowRateCommand);

					if (frequency == 2)
						frequency = -1;

					frequency++;

					Task.Delay(50).Wait();

					//if (UseFirstResponse)
					//{
					//	_btManager.SendCommand(RpmCommand.Command + " 1\r");
					//	CommandsSentCounter++;
					//	OuterCounter++;
					//	_btManager.SendCommand(SpeedCommand.Command + " 1\r");
					//	CommandsSentCounter++;
					//	OuterCounter++;

						//	if (frequency % 1000 == 0)
						//	{
						//		_btManager.SendCommand(CoolantTemperatureCommand.Command + " 1\r");
						//		CommandsSentCounter++;
						//		InternalCounter++;
						//		_btManager.SendCommand(EngineOilTemperatureCommand.Command + " 1\r");
						//		CommandsSentCounter++;
						//		InternalCounter++;
						//	}

						//	frequency++;

						//	if (frequency == int.MaxValue)
						//		frequency = 0;
						//}
						//else
						//{
						//	_btManager.SendCommand(RpmCommand.FormattedCommand);
						//	CommandsSentCounter++;
						//	OuterCounter++;
						//	_btManager.SendCommand(SpeedCommand.FormattedCommand);
						//	CommandsSentCounter++;
						//	OuterCounter++;

						//	if (frequency % 1000 == 0)
						//	{
						//		_btManager.SendCommand(CoolantTemperatureCommand.FormattedCommand);
						//		CommandsSentCounter++;
						//		InternalCounter++;
						//		_btManager.SendCommand(EngineOilTemperatureCommand.FormattedCommand);
						//		CommandsSentCounter++;
						//		InternalCounter++;
						//	}

						//	frequency++;

						//	if (frequency == int.MaxValue)
						//		frequency = 0;
						//}

				}
			}, TaskCreationOptions.LongRunning, _cancellationTokenSource.Token);
		}

		private void TestSendCommand(ObdCommand command)
		{
			TempResponseList += ">" + command.Command + Environment.NewLine;
			_btManager.SendCommand(command.Command + " 1\r");
			//CommandsSentCounter++;
		}

		public void StopReadingData()
		{
			_cancellationTokenSource.Cancel();
		}

		public void UseOneResponse(bool value)
		{
			UseFirstResponse = value;
		}

		private void CalculateGear()
		{
			if(RpmCommand.GetRpm == 0)
				return;

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
			else if (result >= 0.05m && result <= 0.06m)
			{
				Gear = 6;
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

		public EngineFuelRateCommand EngineFuelRateCommand
		{
			get { return _engineFuelRateCommand; }
			set { _engineFuelRateCommand = value; }
		}

		public MafAirFlowRateCommand MafAirFlowRateCommand
		{
			get { return _mafAirFlowRateCommand; }
			set { _mafAirFlowRateCommand = value; }
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

		public string CalculatedCurrentConsumptionCommand
		{
			get { return _calculatedCurrentConsumptionCommand; }
			set
			{
				_calculatedCurrentConsumptionCommand = value;
				OnPropertyChanged("CalculatedCurrentConsumptionCommand");
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
		private EngineOilTemperatureCommand _engineOilTemperatureCommand;
		private CalculatedEngineLoadCommand _calculatedEngineLoadCommand;
		private FuelTankLevelCommand _fuelTankLevelCommand;
		private EngineFuelRateCommand _engineFuelRateCommand;
		private MafAirFlowRateCommand _mafAirFlowRateCommand;
		private string _calculatedCurrentConsumptionCommand;
		private int _gear;

		private string _tempResponseList;
		public bool UseFirstResponse = false;
		public bool UseAtat2Command = false;

		private bool _calculateGear = true;

		public int CommandsSentCounter = 0;
		public int ResponsesTotalLineCounter = 0;
		public int ValidResponsesWithSplitCounter = 0;
		public int InvalidResponses = 0;
		public int OuterCounter = 0;
		public int InternalCounter = 0;

		public DateTime StartTime;
	}
}

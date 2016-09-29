using System;
using System.Collections.Generic;

namespace CarKitProject.OBD.Commands
{
	public class FuelTankLevelCommand : ObdCommand
	{
		public FuelTankLevelCommand()
		{
			Command = "012F";
			CommandShort = "2F";
			Unit = "%";
			Value = "0";
		}

		public int GetFuelTankLevel => int.Parse(Value);

		public double RemainingFuelInLitres
		{
			get { return _remainingFuelInLitres; }
			set
			{
				_remainingFuelInLitres = value;
				OnPropertyChanged("RemainingFuelInLitres");
			}
		}

		public int Range
		{
			get { return _range; }
			set
			{
				_range = value;
				OnPropertyChanged("Range");
			}
		}

		public override void CalculateValue(IList<string> hexValue)
		{
			var fuelLevel = (0.392 * Convert.ToInt64(hexValue[0], 16));
			RemainingFuelInLitres = (fuelLevel / 100) * FuelTankCapacity;
			Range = (int) ((RemainingFuelInLitres / AveregeConsumption)*100);
			Value = "" + fuelLevel;
		}

		private const int FuelTankCapacity = 70;
		private const double AveregeConsumption = 7.2; // TODO: calculate this later;
		private double _remainingFuelInLitres;
		private int _range;
	}
}

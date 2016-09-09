using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarKitProject.ViewModels;
using Xamarin.Forms;

namespace CarKitProject.OBD
{
	public partial class PidsListPage : ContentPage
	{
		public PidsListPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		private void Button_OnClicked(object sender, EventArgs e)
		{
			var text = (sender as Button).Text;
			var viewModel = BindingContext as PidsViewModel;

			if (text == "Speed")
			{
				viewModel.SendCommand(viewModel.SpeedCommand.FormattedCommand);
			}
			else if (text == "Rpm")
			{
				viewModel.SendCommand(viewModel.RpmCommand.FormattedCommand);
			}
			else if (text == "OilTemp")
			{
				viewModel.SendCommand(viewModel.EngineOilTemperatureCommand.FormattedCommand);
			}
			else if (text == "Coolant")
			{
				viewModel.SendCommand(viewModel.CoolantTemperatureCommand.FormattedCommand);
			}
			else if (text == "Load")
			{
				viewModel.SendCommand(viewModel.CalculatedEngineLoadCommand.FormattedCommand);
			}
			else if (text == "Fuel")
			{
				viewModel.SendCommand(viewModel.FuelTankLevelCommand.FormattedCommand);
			}
			else if (text == "Rate")
			{
				viewModel.SendCommand(viewModel.EngineFuelRateCommand.FormattedCommand);
			}
			else if (text == "Maf")
			{
				viewModel.SendCommand(viewModel.MafAirFlowRateCommand.FormattedCommand);
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarKitProject.OBD;
using CarKitProject.ViewModels;
using Xamarin.Forms;

namespace CarKitProject
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			//MainPage = new InfoObdPage();

			var infoOdbViewModel = new InfoObdViewModel();
			infoOdbViewModel.FuelTankLevelCommand.Value = "" + 48;
			infoOdbViewModel.FuelTankLevelCommand.Range = 271;
			infoOdbViewModel.FuelTankLevelCommand.RemainingFuelInLitres = 24;

			MainPage = new MainPage() { BindingContext = infoOdbViewModel };
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

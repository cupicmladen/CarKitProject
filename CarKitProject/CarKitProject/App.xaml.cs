using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarKitProject.OBD;
using Xamarin.Forms;

namespace CarKitProject
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			//MainPage = new MainPage();
			var navigationPage = new NavigationPage(new TestObdPage());
			MainPage = navigationPage;
			//MainPage = new TestObdPage();
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

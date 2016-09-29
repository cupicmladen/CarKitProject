using System;
using CarKitProject.ViewModels;
using Xamarin.Forms;

namespace CarKitProject.OBD
{
	public partial class InfoObdPage : ContentPage
	{
		public InfoObdPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			_viewModel = new ObdCommandsViewModel();
			BindingContext = _viewModel;
		}

		private void Log_OnClicked(object sender, EventArgs e)
		{
			var fileService = DependencyService.Get<IFileService>();
			fileService.SaveToSdCard(_viewModel.Log, "Log");

			//var duration = DateTime.Now.Subtract(_viewModel.StartTime).ToString("ss':'fff");

			//var textToSave = Environment.NewLine + "-----" + title + "-----" + duration + "-----" + Environment.NewLine + Environment.NewLine;

			//var stat = string.Empty;
			//stat += "CommandsSentCounter:            " + _viewModel.CommandsSentCounter + Environment.NewLine + Environment.NewLine;
			//stat += "ResponsesTotalLineCounter:      " + _viewModel.ResponsesTotalLineCounter + Environment.NewLine + Environment.NewLine;
			//stat += "ValidResponsesWithSplitCounter: " + _viewModel.ValidResponsesWithSplitCounter + Environment.NewLine + Environment.NewLine;
			//stat += "InvalidResponses:               " + _viewModel.InvalidResponses + Environment.NewLine + Environment.NewLine;
			//stat += "OuterCounter:                   " + _viewModel.OuterCounter + Environment.NewLine + Environment.NewLine;
			//stat += "InternalCounter:                " + _viewModel.InternalCounter + Environment.NewLine;

			//textToSave += stat;
			//fileService.SaveToSdCard(textToSave, "log");

			//string loopToSave = Environment.NewLine + "---------------------NOVI LOOP------------------------" + Environment.NewLine + Environment.NewLine;
			//loopToSave += _viewModel.TempResponseList + Environment.NewLine;
			//fileService.SaveToSdCard(loopToSave, "loop");
		}

		private void Connect_OnClicked(object sender, EventArgs e)
		{
			_viewModel.ConnectToObdAndInitialize();
		}

		private void Button_OnClicked(object sender, EventArgs e)
		{
			var command = (sender as Button).Text + "\r";
			if(command == "Send\r")
				command = Entry.Text + "\r";

			_viewModel.Log += "-->" + command + Environment.NewLine;
			_viewModel.SendCommand(command);
		}

		private void Button_OnClicked1(object sender, EventArgs e)
		{
			DisplayAlert("Hint", "ati, atl1, ath1, ats1, atal, atsp0, atma", "OK");
		}

		private ObdCommandsViewModel _viewModel;
	}
}

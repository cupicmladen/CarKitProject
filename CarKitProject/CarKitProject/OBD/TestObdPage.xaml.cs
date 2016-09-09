using System;
using CarKitProject.ViewModels;
using Xamarin.Forms;

namespace CarKitProject.OBD
{
	public partial class TestObdPage : ContentPage
	{
		public TestObdPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			_viewModel = new PidsViewModel();
			BindingContext = _viewModel;
		}

		private void Connect_OnClicked(object sender, EventArgs e)
		{
			_viewModel.TempResponseList = "Conecting..." + NewLine();
			var isConnected = _viewModel.ConnectToObd();

			_viewModel.TempResponseList += "" + isConnected + NewLine();
		}

		private void Button_OnClicked(object sender, EventArgs e)
		{
			_viewModel.TempResponseList += NewLine();
			var text = string.Empty;
			var command = (sender as Button).Text;
			_viewModel.TempResponseList += "---------- " + command + " ----------" + Environment.NewLine;

			if (command == "atat2")
				_viewModel.UseAtat2Command = true;

			command += "\r";

			_viewModel.TempResponseList += text;

			_viewModel.SendCommand(command);
		}

		private void ButtonCustom_OnClicked(object sender, EventArgs e)
		{
			_viewModel.TempResponseList += NewLine();
			var text = string.Empty;
			var command = CustomCommandText.Text;
			_viewModel.TempResponseList += "---------- " + command + " ----------" + Environment.NewLine;

			command += "\r";

			_viewModel.TempResponseList += text;

			_viewModel.SendCommand(command);
		}

		private void StartCommunicationg_OnClicked(object sender, EventArgs e)
		{
			_viewModel.LoadData();
		}

		private void StopCommunicationg_OnClicked(object sender, EventArgs e)
		{
			_viewModel.StopReadingData();
		}

		private void Log_OnClicked(object sender, EventArgs e)
		{
			var fileService = DependencyService.Get<IFileService>();

			var title = "Init";

			if (_viewModel.UseFirstResponse)
				title += "  1\\r";

			if(_viewModel.UseAtat2Command)
				title += " Atat2";

			var duration = DateTime.Now.Subtract(_viewModel.StartTime).ToString("ss':'fff");

			var textToSave = Environment.NewLine + "-----" + title + "-----" + duration + "-----" + Environment.NewLine + Environment.NewLine;

			var stat = string.Empty;
			stat += "CommandsSentCounter:            " + _viewModel.CommandsSentCounter + Environment.NewLine + Environment.NewLine;
			stat += "ResponsesTotalLineCounter:      " + _viewModel.ResponsesTotalLineCounter + Environment.NewLine + Environment.NewLine;
			stat += "ValidResponsesWithSplitCounter: " + _viewModel.ValidResponsesWithSplitCounter + Environment.NewLine + Environment.NewLine;
			stat += "InvalidResponses:               " + _viewModel.InvalidResponses + Environment.NewLine + Environment.NewLine;
			stat += "OuterCounter:                   " + _viewModel.OuterCounter + Environment.NewLine + Environment.NewLine;
			stat += "InternalCounter:                " + _viewModel.InternalCounter + Environment.NewLine;

			textToSave += stat;
			fileService.SaveToSdCard(textToSave, "log");
		}

		private void Pids_OnClicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new PidsListPage() {BindingContext = _viewModel});
		}

		private string NewLine()
		{
			return Environment.NewLine;
		}

		private void Switch_OnToggled(object sender, ToggledEventArgs e)
		{
			_viewModel.UseOneResponse(e.Value);
		}

		private PidsViewModel _viewModel;
	}
}

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
			var dateTime = DateTime.Now.ToString("f");
			var textToSave = Environment.NewLine + "-----" + dateTime + "-----" + Environment.NewLine;
			textToSave += _viewModel.TempResponseList;
			fileService.SaveToSdCard(textToSave, "log");
			_viewModel.TempResponseList = string.Empty;
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

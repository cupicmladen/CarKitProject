using System;
using Xamarin.Forms;

namespace CarKitProject.OBD
{
	public partial class TestObdPage : ContentPage
	{
		public TestObdPage()
		{
			InitializeComponent();
		}

		private void Connect_OnClicked(object sender, EventArgs e)
		{
			Editor.Text = "Conecting..." + NewLine();
			_btManager = DependencyService.Get<IBtConnectionManager>();
			_btManager.ConnectToObd();

			Editor.Text += "" + _btManager.IsConnected + NewLine();

			if (_btManager == null || !_btManager.IsConnected)
				return;

			_btManager.DataReceived += BtDataReceived;
			_btManager.StartReadingData();
		}

		private void BtDataReceived(string obj)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Editor.Text += obj + NewLine();
			});

		}

		private void Button_OnClicked(object sender, EventArgs e)
		{
			Editor.Text += NewLine();
			var text = string.Empty;
			var command = (sender as Button).Text;
			text += "---------- " + command + " ----------" + Environment.NewLine;

			command += "\r";

			Editor.Text += text;

			_btManager.SendCommand(command);
		}

		private void ButtonCustom_OnClicked(object sender, EventArgs e)
		{
			Editor.Text += NewLine();
			var text = string.Empty;
			var command = CustomCommandEntry.Text;
			text += "---------- " + command + " ----------" + Environment.NewLine;

			command += "\r";

			Editor.Text += text;

			_btManager.SendCommand(command);
		}

		private void Rpm_OnClicked(object sender, EventArgs e)
		{
			Editor.Text += NewLine();
			var text = string.Empty;
			var command = "010C";
			text += "---------- " + command + " ----------" + Environment.NewLine;

			command += "\r";

			Editor.Text += text;

			_btManager.SendCommand(command);
		}

		private void Log_OnClicked(object sender, EventArgs e)
		{
			var fileService = DependencyService.Get<IFileService>();
			var dateTime = DateTime.Now.ToString("f");
			var textToSave = Environment.NewLine + "-----" + dateTime + "-----" + Environment.NewLine;
			textToSave += Editor.Text;
			fileService.SaveToSdCard(textToSave, "log");
			Editor.Text = string.Empty;
		}

		private void Clear_OnClicked(object sender, EventArgs e)
		{
			Editor.Text = string.Empty;
		}

		private void Stop_OnClicked(object sender, EventArgs e)
		{
			_btManager.StopReadingData();
		}

		private string NewLine()
		{
			return Environment.NewLine;
		}

		private IBtConnectionManager _btManager;
	}
}

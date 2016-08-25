using System;
using Xamarin.Forms;

namespace CarKitProject.OBD
{
	public partial class TestObdPage : ContentPage
	{
		public TestObdPage()
		{
			InitializeComponent();
			_btManager = DependencyService.Get<IBtConnectionManager>();
			_btManager.DataReceived += BtDataReceived;
		}

		private void Connect_OnClicked(object sender, EventArgs e)
		{
			Editor.Text = "Conecting..." + NewLine();
			_btManager = DependencyService.Get<IBtConnectionManager>();
			var isConnected = _btManager.ConnectToObd();

			Editor.Text += "" + isConnected + NewLine();
		}

		private void StartMonitoring_OnClicked(object sender, EventArgs e)
		{
			if (_btManager != null)
			{
				_btManager.DataReceived += BtDataReceived;
				_btManager.StartReadingData();
			}
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

			if (_appendNewLine)
				command += "\r"; //command += "\r\n"; alternative

			Editor.Text += text;

			_btManager.SendCommand(command);
		}

		private void ButtonCustom_OnClicked(object sender, EventArgs e)
		{
			Editor.Text += NewLine();
			var text = string.Empty;
			var command = CustomCommandEntry.Text;
			text += "---------- " + command + " ----------" + Environment.NewLine;

			if (_appendNewLine)
				command += "\r";

			Editor.Text += text;
			CustomCommandEntry.Text = "";

			_btManager.SendCommand(command);
		}

		//private void Button_OnClicked(object sender, EventArgs e)
		//{
		//	Editor.Text += NewLine();
		//	var text = ">";
		//	var command = (sender as Button).Text;
		//	text += command + Environment.NewLine;

		//	if (_appendNewLine)
		//		command += "\r"; //command += "\r\n"; alternative

		//	Editor.Text += text;

		//	_btManager.SendCommand(command);
		//}

		//private void ButtonCustom_OnClicked(object sender, EventArgs e)
		//{
		//	Editor.Text += NewLine();
		//	var text = ">";
		//	var command = CustomCommandEntry.Text;
		//	text += command + NewLine();

		//	if (_appendNewLine)
		//		command += "\r";

		//	Editor.Text += text;
		//	CustomCommandEntry.Text = "";

		//	_btManager.SendCommand(command);
		//}

		private void Rpm_OnClicked(object sender, EventArgs e)
		{
			Editor.Text += NewLine();
			var text = ">";
			var command = "010C";
			text += command + NewLine();

			if (_appendNewLine)
				command += "\r";

			Editor.Text += text;

			_btManager.SendCommand(command);
		}

		private void Switch_OnToggled(object sender, ToggledEventArgs e)
		{
			_appendNewLine = e.Value;
		}

		private void Log_OnClicked(object sender, EventArgs e)
		{
			var fileService = DependencyService.Get<IFileService>();
			var dateTime = DateTime.Now.ToString("f");
			var textToSave = Environment.NewLine + "-----" + dateTime + "-----" + Environment.NewLine;
			textToSave += Editor.Text;
			fileService.SaveToSdCard(textToSave, "log_" + _counter);
			_counter++;
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
		private bool _appendNewLine;
		private int _counter = 1;
	}
}

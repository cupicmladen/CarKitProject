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
			var isConnected = _btManager.ConnectToObd();

			Editor.Text += "" + isConnected + NewLine();

			if (_btManager == null || !isConnected)
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
			//CustomCommandEntry.Text = "";

			_btManager.SendCommand(command);
		}

		private void ButtonCustom1_OnClicked(object sender, EventArgs e)
		{
			Editor.Text += NewLine();
			var text = string.Empty;
			var command = CustomCommandEntry1.Text;
			text += "---------- " + command + " ----------" + Environment.NewLine;

			if (_appendNewLine)
				command += "\r";

			Editor.Text += text;
			//CustomCommandEntry.Text = "";

			_btManager.SendCommand(command); ;
		}

		private void Rpm_OnClicked(object sender, EventArgs e)
		{
			Editor.Text += NewLine();
			var text = string.Empty;
			var command = "010C";
			text += "---------- " + command + " ----------" + Environment.NewLine;

			if (_appendNewLine)
				command += "\r";

			Editor.Text += text;

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

		//private void Rpm_OnClicked(object sender, EventArgs e)
		//{
		//	Editor.Text += NewLine();
		//	var text = ">";
		//	var command = "010C";
		//	text += command + NewLine();

		//	if (_appendNewLine)
		//		command += "\r";

		//	Editor.Text += text;

		//	_btManager.SendCommand(command);
		//}

		private void Switch_OnToggled(object sender, ToggledEventArgs e)
		{
			_appendNewLine = e.Value;
		}

		private void Switch1_OnToggled(object sender, ToggledEventArgs e)
		{
			_btManager.UseLineFormat(e.Value);
		}

		private void Log_OnClicked(object sender, EventArgs e)
		{
			var fileService = DependencyService.Get<IFileService>();
			var dateTime = DateTime.Now.ToString("f");
			var textToSave = Environment.NewLine + "-----" + dateTime + "-----" + Environment.NewLine;
			textToSave += Editor.Text;
			//fileService.SaveToSdCard(textToSave, "log_" + _counter);
			fileService.SaveToSdCard(textToSave, FileTitle.Text);
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

		private async void Hint_OnClicked(object sender, EventArgs e)
		{
			var action = await DisplayActionSheet("Title", "", "", "1 Init", "2 All default", "3 New line replace", "4 ATH0",
				"5 Multi commands", "6 ATMA");

			FileTitle.Text = action;
		}

		private IBtConnectionManager _btManager;
		private bool _appendNewLine;
		private int _counter = 1;
	}
}

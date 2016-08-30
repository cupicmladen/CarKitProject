using System;
using System.Threading;
using System.Threading.Tasks;
using CarKitProject.OBD;
using CarKitProject.OBD.Commands;
using Xamarin.Forms;

namespace CarKitProject.ViewModels
{
	public class PidsViewModel
	{
		public PidsViewModel()
		{
			_cancellationTokenSource = new CancellationTokenSource();
		}

		private void ConnectToObd()
		{
			_btManager = DependencyService.Get<IBtConnectionManager>();
			_btManager.ConnectToObd();

			if (_btManager == null || !_btManager.IsConnected)
				return;

			_btManager.DataReceived += BtDataReceived;
			_btManager.StartReadingData();

			LoadData();
		}

		private void BtDataReceived(string command)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
			});
		}

		private void LoadData()
		{
			Task.Factory.StartNew((o) =>
			{
				while (!_cancellationTokenSource.IsCancellationRequested)
				{
					_btManager.SendCommand(RpmCommand.FormattedCommand);
				}
			}, TaskCreationOptions.LongRunning, _cancellationTokenSource.Token);
		}

		#region ObdCommands

		public RpmCommand RpmCommand
		{
			get { return _rpmCommand; }
			set { _rpmCommand = value; }
		}

		#endregion

		private readonly CancellationTokenSource _cancellationTokenSource;
		private IBtConnectionManager _btManager;

		private RpmCommand _rpmCommand;
	}
}

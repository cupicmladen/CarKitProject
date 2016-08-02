using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Runtime;
using Android.Views;

namespace CarKitProject.Droid
{
	public static class Utils
	{
		public static Orientation GetScreenOrientation()
		{
			var windowManager = Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
			var rotation = windowManager.DefaultDisplay.Rotation;
			return rotation == SurfaceOrientation.Rotation90 ? Orientation.Landscape : Orientation.Portrait;
		}
	}
}
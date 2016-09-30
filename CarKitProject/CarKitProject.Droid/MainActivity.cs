using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Media;
using Android.Net;
using Android.OS;
using CarKitProject.Interfaces;
using Xamarin.Forms;

namespace CarKitProject.Droid
{
	[Activity(Label = "CarKitProject", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
	//[Activity(Label = "CarKitProject", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			Xamarin.Forms.Forms.Init(this, bundle);
			Xamarin.FormsMaps.Init(this, bundle);

			var currentLocationService = DependencyService.Get<ICurrentLocationService>();
			currentLocationService.InitializeLocationManager();

			LoadApplication(new App());
		}
	}
}


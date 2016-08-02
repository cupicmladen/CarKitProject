using System.ComponentModel;
using CarKitProject.Droid.Renderer;
using CarKitProject.Droid.Views;
using CarKitProject.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(OdometerControl), typeof(OdometerRenderer))]
namespace CarKitProject.Droid.Renderer
{
	public class OdometerRenderer : ViewRenderer<OdometerControl, OdometerView>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<OdometerControl> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || this.Element == null)
				return;

			SetNativeControl(new OdometerView(Resources.DisplayMetrics.Density, Context)
			{
				ShapeView = Element
			});
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == "Indicator" || e.PropertyName == "Speed")
				Control.Invalidate();
		}
	}
}
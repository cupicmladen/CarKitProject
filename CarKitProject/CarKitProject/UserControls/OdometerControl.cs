using Xamarin.Forms;

namespace CarKitProject.UserControls
{
	public class OdometerControl : BoxView
	{
		public static readonly BindableProperty CenterColorProperty = BindableProperty.Create<OdometerControl, Color>(s => s.CenterColor, Color.Default);

		public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create<OdometerControl, Color>(s => s.StrokeColor, Color.Default);

		public static readonly BindableProperty IndicatorStrokeColorProperty = BindableProperty.Create<OdometerControl, Color>(s => s.IndicatorStrokeColor, Color.Default);

		public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create<OdometerControl, float>(s => s.StrokeWidth, 1f);

		public static readonly BindableProperty IndicatorStrokeWidthProperty = BindableProperty.Create<OdometerControl, float>(s => s.StrokeWidth, 3f);

		public static readonly BindableProperty IndicatorProperty = BindableProperty.Create<OdometerControl, float>(s => s.Indicator, 0f);

		public static readonly BindableProperty MaxValueIndicatorProperty = BindableProperty.Create<OdometerControl, int>(s => s.MaxValueIndicator, 100);

		public static readonly BindableProperty PaddingProperty = BindableProperty.Create<OdometerControl, Thickness>(s => s.Padding, default(Thickness));

		public static readonly BindableProperty SpeedProperty = BindableProperty.Create<OdometerControl, int>(s => s.Speed, 0);

		public static readonly BindableProperty SpeedColorProperty = BindableProperty.Create<OdometerControl, Color>(s => s.SpeedColor, Color.Default);

		public Color CenterColor
		{
			get { return (Color)GetValue(CenterColorProperty); }
			set { SetValue(CenterColorProperty, value); }
		}

		public Color StrokeColor
		{
			get { return (Color)GetValue(StrokeColorProperty); }
			set { SetValue(StrokeColorProperty, value); }
		}

		public Color IndicatorStrokeColor
		{
			get { return (Color)GetValue(IndicatorStrokeColorProperty); }
			set { SetValue(IndicatorStrokeColorProperty, value); }
		}

		public float StrokeWidth
		{
			get { return (float)GetValue(StrokeWidthProperty); }
			set { SetValue(StrokeWidthProperty, value); }
		}

		public float IndicatorStrokeWidth
		{
			get { return (float)GetValue(IndicatorStrokeWidthProperty); }
			set { SetValue(IndicatorStrokeWidthProperty, value); }
		}

		public float Indicator
		{
			get { return (float)GetValue(IndicatorProperty); }
			set { SetValue(IndicatorProperty, value); }
		}

		public int MaxValueIndicator
		{
			get { return (int)GetValue(MaxValueIndicatorProperty); }
			set { SetValue(MaxValueIndicatorProperty, value); }
		}

		public Thickness Padding
		{
			get { return (Thickness)GetValue(PaddingProperty); }
			set { SetValue(PaddingProperty, value); }
		}

		public int Speed
		{
			get { return (int)GetValue(SpeedProperty); }
			set { SetValue(SpeedProperty, value); }
		}

		public Color SpeedColor
		{
			get { return (Color)GetValue(SpeedColorProperty); }
			set { SetValue(SpeedColorProperty, value); }
		}
	}
}

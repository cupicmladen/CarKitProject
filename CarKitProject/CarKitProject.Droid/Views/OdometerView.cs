using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Views;
using CarKitProject.UserControls;
using Xamarin.Forms.Platform.Android;

namespace CarKitProject.Droid.Views
{
	public class OdometerView : View
	{
		public OdometerView(float density, Context context) : base(context)
		{
			this._density = density;
			_screenOrientation = Utils.GetScreenOrientation();
		}

		public OdometerControl ShapeView { get; set; }

		public new int Width
		{
			get { return base.Width - (int)(Resize(ShapeView.Padding.HorizontalThickness)); }
		}

		public new int Height
		{
			get { return base.Height - (int)(Resize(ShapeView.Padding.VerticalThickness)); }
		}

		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);
			HandleShapeDraw(canvas);
		}

		protected virtual void HandleShapeDraw(Canvas canvas)
		{
			_x = GetX() + Resize(ShapeView.Padding.Left);
			_y = GetY() + Resize(ShapeView.Padding.Top);
			_cx = _x + Width / 2;
			_cy = _y + Height / 2;

			if (_screenOrientation == Orientation.Landscape)
			{
				_radius = (Height - 10) / 2;
				//HandleStandardDraw(p => canvas.DrawCircle(_cx, _cy, _radius, p), ShapeView.StrokeColor, drawFill: false);
				var rectF = GetLandscapeRectF();
				HandleRadialDraw(p => canvas.DrawCircle(_cx, _cy, _radius, p), ShapeView.CenterColor);
				HandleArcDraw(p => canvas.DrawArc(rectF, _odometerStartPosition, 270, false, p), ShapeView.StrokeColor, ShapeView.IndicatorStrokeWidth);
				HandleArcDraw(p => canvas.DrawArc(rectF, _odometerStartPosition, 270 * (ShapeView.Indicator / ShapeView.MaxValueIndicator), false, p), ShapeView.IndicatorStrokeColor, ShapeView.IndicatorStrokeWidth);
				HandleTextDraw(p => canvas.DrawText(ShapeView.Speed.ToString(), _cx, _cy - (p.Descent() + p.Ascent() / 2), p), ShapeView.SpeedColor, 300, 20);
				HandleTextDraw(p => canvas.DrawText("km/h", _cx, _cy - (p.Descent() + p.Ascent() / 2) + 150, p), ShapeView.SpeedColor, 50);
			}
			//else
			//{
			//	_radius = Width/2;
			//	HandleStandardDraw(p => canvas.DrawCircle(_cx, _cy, _radius, p), ShapeView.StrokeColor, drawFill: false);
			//	var rectF = GetPortraitRectF();
			//	HandleStandardDraw(p => canvas.DrawArc(rectF, _quarterTurnCounterClockwise, 270 * (ShapeView.Indicator / ShapeView.MaxValueIndicator), false, p), ShapeView.IndicatorStrokeColor, ShapeView.IndicatorStrokeWidth, false);
			//}
		}

		private void HandleArcDraw(Action<Paint> drawShape, Xamarin.Forms.Color strokeColor, float lineWidth)
		{
			var strokePaint = new Paint(PaintFlags.AntiAlias);
			strokePaint.SetStyle(Paint.Style.Stroke);
			strokePaint.StrokeWidth = Resize(lineWidth);
			strokePaint.StrokeCap = Paint.Cap.Round;
			strokePaint.Color = strokeColor.ToAndroid();
			drawShape(strokePaint);
		}

		private void HandleRadialDraw(Action<Paint> drawShape, Xamarin.Forms.Color centerColor)
		{
			var fillPaint = new Paint();
			fillPaint.SetStyle(Paint.Style.Fill);
			var radialGradient = new RadialGradient(_cx, _cy, _radius, centerColor.ToAndroid(), Color.Black, Shader.TileMode.Clamp);
			fillPaint.SetShader(radialGradient);
			drawShape(fillPaint);
		}

		private void HandleTextDraw(Action<Paint> drawShape, Xamarin.Forms.Color textColor, int textSize, int strokeWidth = 0)
		{
			var textPaint = new Paint
			{
				AntiAlias = true,
				Color = textColor.ToAndroid(),
				TextSize = textSize,
				StrokeWidth = strokeWidth,
				TextAlign = Paint.Align.Center,
			};

			textPaint.SetStyle(Paint.Style.FillAndStroke);
			drawShape(textPaint);
		}

		private RectF GetLandscapeRectF()
		{
			var circleDiameter = Height;

			var left = ((Width - circleDiameter) / 2) + _x;
			var right = left + circleDiameter;

			var top = _y;
			var bottom = top + circleDiameter;

			var rectF = new RectF(left, top, right, bottom);
			return rectF;
		}

		private RectF GetPortraitRectF()
		{
			var circleDiameter = Width;

			var left = _x;
			var right = left + circleDiameter;

			var top = ((Height - circleDiameter) / 2) + _y;
			var bottom = top + circleDiameter;

			var rectF = new RectF(left, top, right, bottom);
			return rectF;
		}

		private float Resize(float input)
		{
			return input * _density;
		}

		private float Resize(double input)
		{
			return Resize((float)input);
		}

		private readonly float _density;
		private readonly float _quarterTurnCounterClockwise = -90;
		private readonly float _odometerStartPosition = 135;
		private Orientation _screenOrientation;
		private float _x;
		private float _y;
		private float _cx;
		private float _cy;
		private float _radius;
	}
}
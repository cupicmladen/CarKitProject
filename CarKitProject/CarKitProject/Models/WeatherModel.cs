using System;
using CarKitProject.Enums;
using Xamarin.Forms;

namespace CarKitProject.Models
{
	public class WeatherModel
	{
		public string City
		{
			get { return _city; }
			set { _city = value; }
		}

		public string Country
		{
			get { return _country; }
			set { _country = value; }
		}

		public double CurrentTemp
		{
			get { return _currentTemp; }
			set { _currentTemp = value; }
		}

		public double MinTemp
		{
			get { return _minTemp; }
			set { _minTemp = value; }
		}

		public double MaxTemp
		{
			get { return _maxTemp; }
			set { _maxTemp = value; }
		}

		public double Humidity
		{
			get { return _humidity; }
			set { _humidity = value; }
		}

		public double WindSpeed
		{
			get { return _windSpeed; }
			set { _windSpeed = value; }
		}

		public DateTime Sunrise
		{
			get { return _sunrise; }
			set { _sunrise = value; }
		}

		public DateTime Sunset
		{
			get { return _sunset; }
			set { _sunset = value; }
		}

		public WeatherCondition WeatherCondition
		{
			get { return _weatherCondition; }
			set { _weatherCondition = value; }
		}

		public ImageSource WeatherImage
		{
			get { return _weatherImage; }
			set { _weatherImage = value; }
		}

		public string WeatherDescription
		{
			get { return _weatherDescription; }
			set { _weatherDescription = value; }
		}

		private string _city;
		private string _country;
		private double _currentTemp;
		private double _minTemp;
		private double _maxTemp;
		private double _humidity;
		private double _windSpeed;
		private DateTime _sunrise;
		private DateTime _sunset;
		private WeatherCondition _weatherCondition;
		private ImageSource _weatherImage;
		private string _weatherDescription;
	}
}

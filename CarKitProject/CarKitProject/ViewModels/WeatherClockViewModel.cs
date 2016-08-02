using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CarKitProject.Enums;
using CarKitProject.Helpers;
using CarKitProject.Models;
using CarKitProject.Properties;
using CarKitProject.Services;
using Xamarin.Forms;

namespace CarKitProject.ViewModels
{
	public class WeatherClockViewModel : INotifyPropertyChanged
	{
		public WeatherClockViewModel()
		{
			SetDateTime();
			Device.StartTimer(TimeSpan.FromMinutes(1), TimerTickCallback);

			GetWeatherByCityId("792680");
		}

		public WeatherModel Model { get; set; }


		public string Day
		{
			get { return _day; }
			set
			{
				_day = value;
				OnPropertyChanged("Day");
			}
		}

		public string Date
		{
			get { return _date; }
			set
			{
				_date = value;
				OnPropertyChanged("Date");
			}
		}

		public string Hour
		{
			get { return _hour; }
			set
			{
				_hour = value;
				OnPropertyChanged("Hour");
			}
		}

		public string Minutes
		{
			get { return _minutes; }
			set
			{
				_minutes = value;
				OnPropertyChanged("Minutes");
			}
		}

		public string DisplayLocation
		{
			get { return _displayLocation; }
			set
			{
				_displayLocation = value;
				OnPropertyChanged("DisplayLocation");
			}
		}

		public string City
		{
			get { return Model.City; }
			set
			{
				Model.City = value;
				OnPropertyChanged("City");
			}
		}

		public string Country
		{
			get { return Model.Country; }
			set
			{
				Model.Country = value;
				OnPropertyChanged("Country");
			}
		}

		public DateTime Sunrise
		{
			get { return Model.Sunrise; }
			set
			{
				Model.Sunrise = value;
				OnPropertyChanged("Sunrise");
			}
		}

		public DateTime Sunset
		{
			get { return Model.Sunset; }
			set
			{
				Model.Sunset = value;
				OnPropertyChanged("Sunset");
			}
		}

		public double CurrentTemp
		{
			get { return Model.CurrentTemp; }
			set
			{
				Model.CurrentTemp = value;
				OnPropertyChanged("CurrentTemp");
			}
		}

		public double MinTemp
		{
			get { return Model.MinTemp; }
			set
			{
				Model.MinTemp = value;
				OnPropertyChanged("MinTemp");
			}
		}

		public double MaxTemp
		{
			get { return Model.MaxTemp; }
			set
			{
				Model.MaxTemp = value;
				OnPropertyChanged("MaxTemp");
			}
		}

		public double Humidity
		{
			get { return Model.Humidity; }
			set
			{
				Model.Humidity = value;
				OnPropertyChanged("Humidity");
			}
		}

		public double WindSpeed
		{
			get { return Model.WindSpeed; }
			set
			{
				Model.WindSpeed = value;
				OnPropertyChanged("WindSpeed");
			}
		}

		public WeatherCondition WeatherCondition
		{
			get { return Model.WeatherCondition; }
			set
			{
				Model.WeatherCondition = value;
				OnPropertyChanged("WeatherCondition");
			}
		}

		public string WeatherDescription
		{
			get { return Model.WeatherDescription; }
			set
			{
				Model.WeatherDescription = value;
				OnPropertyChanged("WeatherDescription");
			}
		}

		public ImageSource WeatherImage
		{
			get { return Model.WeatherImage; }
			set
			{
				Model.WeatherImage = value;
				OnPropertyChanged("WeatherImage");
			}
		}

		private async void GetWeatherByCityId(string cityId)
		{
			if (_weatherService == null)
				_weatherService = new WeatherService();

			if (Model == null)
				Model = new WeatherModel();

			var weatherRoot = await _weatherService.GetWeatherByCurrentLocation();
			CreateModel(weatherRoot);

			DisplayLocation = Model.City + ", " + Model.Country.ToUpper();
			OnPropertyChanged();
		}

		public void CreateModel(WeatherRoot weatherRoot)
		{
			City = weatherRoot.Name;
			Country = weatherRoot.System.Country;
			CurrentTemp = weatherRoot.MainWeather.Temperature;
			MinTemp = weatherRoot.MainWeather.MinTemperature;
			MaxTemp = weatherRoot.MainWeather.MaxTemperature;
			Humidity = weatherRoot.MainWeather.Humidity;
			WindSpeed = weatherRoot.Wind.Speed;
			Sunrise = weatherRoot.System.DisplaySunrise;
			Sunset = weatherRoot.System.DisplaySunset;

			//START Weather.First()
			WeatherDescription = weatherRoot.Weather.First().Main;
			//var weatherCondition = (WeatherCondition?)Enum.Parse(typeof (WeatherCondition), weatherRoot.Weather.First().Id.ToString());
			var isDefined = Enum.IsDefined(typeof(WeatherCondition), weatherRoot.Weather.First().Id);
			if (isDefined)
				WeatherCondition = (WeatherCondition)weatherRoot.Weather.First().Id;

			var imageSource = ImageLoader.LoadImageFromResources(WeatherCondition);
			WeatherImage = imageSource;
			//END Weather.First()
		}

		private bool TimerTickCallback()
		{
			SetDateTime();
			return true;
		}

		private void SetDateTime()
		{
			var _dateTime = DateTime.Now;

			Day = _dateTime.DayOfWeek.ToString();
			Date = "" + _dateTime.Day + " " + _dateTime.ToString("MMMM"); ;
			Hour = _dateTime.Hour.ToString();
			Minutes = _dateTime.Minute.ToString();
		}

		private WeatherService _weatherService;
		private string _day;
		private string _date;
		private string _hour;
		private string _minutes;
		private string _displayLocation;

		public event PropertyChangedEventHandler PropertyChanged;
		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

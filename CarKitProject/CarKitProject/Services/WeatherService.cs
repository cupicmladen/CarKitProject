using System.Net.Http;
using System.Threading.Tasks;
using CarKitProject.Interfaces;
using CarKitProject.Models;
using Xamarin.Forms;
using static Newtonsoft.Json.JsonConvert;

namespace CarKitProject.Services
{
	public class WeatherService
	{
		const string WeatherCityUri = "http://api.openweathermap.org/data/2.5/weather?q={0}&units={1}&appid=d811ad211c30e75d499e4b39532cdd94";
		const string WeatherCoordinatesUri = "http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units={2}&appid=d811ad211c30e75d499e4b39532cdd94";
		const string WeatherCityIdUri = "http://api.openweathermap.org/data/2.5/weather?id={0}&units={1}&appid=d811ad211c30e75d499e4b39532cdd94";
		const string BelgradeJson = "{\"coord\":{\"lon\":20.47,\"lat\":44.8},\"weather\":[{\"id\":800,\"main\":\"Clear\",\"description\":\"clear sky\",\"icon\":\"01d\"}],\"base\":\"stations\",\"main\":{\"temp\":27.71,\"pressure\":1014,\"humidity\":45,\"temp_min\":26,\"temp_max\":29},\"visibility\":10000,\"wind\":{\"speed\":4.1,\"deg\":350},\"clouds\":{\"all\":0},\"dt\":1467806400,\"sys\":{\"type\":1,\"id\":5969,\"message\":0.0312,\"country\":\"RS\",\"sunrise\":1467773985,\"sunset\":1467829558},\"id\":792680,\"name\":\"Belgrade\",\"cod\":200}\n";

		public async Task<WeatherRoot> GetWeatherByCurrentLocation()
		{
			var currentLocationService = DependencyService.Get<ICurrentLocationService>();
			var currentLocation = currentLocationService.GetCurrentLocation();

			using (var client = new HttpClient())
			{
				var url = string.Format(WeatherCoordinatesUri, currentLocation.Latitude, currentLocation.Longitude, "metric");
				var json = await client.GetStringAsync(url);

				if (string.IsNullOrWhiteSpace(json))
					return null;

				return DeserializeObject<WeatherRoot>(json);
			}
		}

		public async Task<WeatherRoot> GetWeatherByCity(string city)
		{
			using (var client = new HttpClient())
			{
				var url = string.Format(WeatherCityUri, city, "metric");
				var json = await client.GetStringAsync(url);

				if (string.IsNullOrWhiteSpace(json))
					return null;

				return DeserializeObject<WeatherRoot>(json);
			}
		}

		public async Task<WeatherRoot> GetWeatherByCityId(string cityId)
		{
			using (var client = new HttpClient())
			{
				var url = string.Format(WeatherCityIdUri, cityId, "metric");
				var json = await client.GetStringAsync(url);

				if (string.IsNullOrWhiteSpace(json))
					return null;

				return DeserializeObject<WeatherRoot>(json);
			}

			//return DeserializeObject<WeatherRoot>(BelgradeJson);
		}
	}
}

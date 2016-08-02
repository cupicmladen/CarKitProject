using System;

namespace CarKitProject.Helpers
{
	public static class Utils
	{
		public static DateTime TimeSpanToDateTime(string ts)
		{
			var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

			double value;
			var success = double.TryParse(ts, out value);
			if (success)
				dateTime = dateTime.AddSeconds(value);

			return dateTime.ToLocalTime();
		}
	}
}

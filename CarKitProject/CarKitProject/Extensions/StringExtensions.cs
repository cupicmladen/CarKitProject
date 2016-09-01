using System.Collections.Generic;
using System.Linq;

namespace CarKitProject.Extensions
{
	public static class StringExtensions
	{
		public static IList<string> SplitAndRefactor(this string str, int chunkSize)
		{
			return Enumerable.Range(1, (str.Length - 2) / chunkSize)
				.Select(i => str.Substring(i * chunkSize, chunkSize)).ToList();
		}
	}
}

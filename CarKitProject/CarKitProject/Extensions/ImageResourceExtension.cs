using System;
using CarKitProject.Helpers;
using CarKitProject.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarKitProject.Extensions
{
	[ContentProperty("Source")]
	public class ImageResourceExtension : IMarkupExtension
	{
		public string Source { get; set; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			if (Source == null)
			{
				return null;
			}

			var imageSource = ImageLoader.LoadImageFromResources(Source);
			return imageSource;
		}
	}
}

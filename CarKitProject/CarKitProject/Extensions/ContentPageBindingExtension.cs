using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CarKitProject.Extensions
{
	public class ContentPageBindingExtension : IMarkupExtension
	{

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			if (serviceProvider == null) throw new ArgumentNullException("serviceProvider");
			IRootObjectProvider rootObjectProvider = serviceProvider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
			if (rootObjectProvider == null)
			{
				throw new ArgumentException("serviceProvider does not provide an IRootObjectProvider");
			}
			return rootObjectProvider.RootObject as Element ?? new object();
		}
	}
}

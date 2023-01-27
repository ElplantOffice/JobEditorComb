using JobEditor.Views.ProductData;
using System;

namespace JobEditor.Import
{
	public interface IImport
	{
		string ProductName
		{
			get;
		}

		bool Convert(ProductView productView);

		bool ReadFile();
	}
}
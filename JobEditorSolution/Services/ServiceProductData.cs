using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Services
{
	[Serializable]
	public class ServiceProductData
	{
		public string FileProduct
		{
			get;
			set;
		}

		public string PathProduct
		{
			get;
			set;
		}

		public ServiceProductData(string pathProduct, string fileProduct)
		{
			this.PathProduct = pathProduct;
			this.FileProduct = fileProduct;
		}

		public ServiceProductData(string fullPath)
		{
			this.PathProduct = Path.GetDirectoryName(fullPath);
			this.FileProduct = Path.GetFileName(fullPath);
		}
	}
}
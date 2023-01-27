using Patterns.EventAggregator;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace JobEditor
{
	internal class JobEditorFileResourceDataInfo : IFileResourceDataInfo
	{
		private System.Reflection.Assembly assembly;

		private Type dataType;

		private JobEditorFileResourceData data;

		public System.Reflection.Assembly Assembly
		{
			get
			{
				return this.assembly;
			}
		}

		public IFileResourceData Data
		{
			get
			{
				return this.data;
			}
		}

		public Type DataType
		{
			get
			{
				return this.dataType;
			}
		}

		internal JobEditorFileResourceDataInfo()
		{
			this.dataType = this.GetType();
			this.assembly = (
				from x in AppDomain.CurrentDomain.GetAssemblies()
				where x.FullName == this.dataType.Assembly.FullName
				select x).FirstOrDefault<System.Reflection.Assembly>();
			this.data = new JobEditorFileResourceData(this.assembly);
		}
	}
}
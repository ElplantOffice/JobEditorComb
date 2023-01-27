using Patterns.EventAggregator;
using System;

namespace JobEditor
{
	public class JobEditorFileResources
	{
		private FileResourceDataInfo dataInfo;

		public JobEditorFileResources()
		{
			if (this.dataInfo != null)
			{
				return;
			}
			this.dataInfo = SingletonProvider<FileResourceDataInfo>.Instance;
			this.dataInfo.Init(new JobEditorFileResourceDataInfo());
		}
	}
}
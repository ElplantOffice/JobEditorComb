using Patterns.EventAggregator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media;

namespace JobEditor
{
	internal class JobEditorFileResourceData : IFileResourceData
	{
		private FileResourceData data = new FileResourceData();

		internal JobEditorFileResourceData(Assembly assembly)
		{
			Dictionary<string, object> strs = null;
			ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
			assembly.GetManifestResourceNames();
			string str = string.Concat(assembly.GetName().Name, ".g.resources");
			using (ResourceReader resourceReaders = new ResourceReader(assembly.GetManifestResourceStream(str)))
			{
				strs = resourceReaders.Cast<DictionaryEntry>().Aggregate<DictionaryEntry, Dictionary<string, object>>(new Dictionary<string, object>(), (Dictionary<string, object> d, DictionaryEntry e) => {
					d.Add(e.Key.ToString(), e.Value);
					return d;
				});
			}
			foreach (KeyValuePair<string, object> keyValuePair in strs)
			{
				if (keyValuePair.Key.Contains(".png"))
				{
					string str1 = keyValuePair.Key.Split(new char[] { '/' }).Last<string>();
					string str2 = string.Concat("pack://application:,,,/", assembly.GetName().Name, ";component/", keyValuePair.Key);
					if (imageSourceConverter.IsValid(str2))
					{
						ImageSource imageSource = (ImageSource)imageSourceConverter.ConvertFromString(str2);
						this.data.Set<ImageSource>(str1, imageSource);
					}
				}
				if (!keyValuePair.Key.Contains(".xml"))
				{
					continue;
				}
				string str3 = keyValuePair.Key.Split(new char[] { '/' }).Last<string>();
				this.data.Set<Stream>(str3, (Stream)keyValuePair.Value);
			}
		}

		public T Get<T>(string name)
		{
			return this.data.Get<T>(name);
		}

		protected virtual void OnSetNotify(ResourceEventArgs e)
		{
			if (this.SetNotify != null)
			{
				this.SetNotify(this, e);
			}
		}

		public void Set<T>(string name, T value)
		{
			this.data.Set<T>(name, value);
			this.OnSetNotify(new ResourceEventArgs(name, (object)value));
		}

		public event ResourceEventHandler SetNotify;
	}
}
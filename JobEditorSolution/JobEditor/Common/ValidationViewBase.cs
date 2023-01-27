using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace JobEditor.Common
{
	public class ValidationViewBase : INotifyDataErrorInfo
	{
		private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();

		public bool HasErrors
		{
			get
			{
				if (this._validationErrors.Count > 0)
				{
					return true;
				}
				return false;
			}
		}

		public ValidationViewBase()
		{
		}

		public void CloneValidationDataFrom(ValidationViewBase source)
		{
			this._validationErrors.Clear();
			foreach (string key in source._validationErrors.Keys)
			{
				ICollection<string> item = source._validationErrors[key];
				ICollection<string> list = null;
				if (item != null)
				{
					list = item.ToList<string>();
				}
				this._validationErrors.Add(key, list);
			}
		}

		public IEnumerable GetErrors(string propertyName)
		{
			IEnumerable item = null;
			if (this._validationErrors.ContainsKey(propertyName))
			{
				item = this._validationErrors[propertyName];
			}
			return item;
		}

		protected bool GetValidState(string propertyName)
		{
			if (this._validationErrors.ContainsKey(propertyName))
			{
				return false;
			}
			return true;
		}

		private void RaiseErrorsChanged(string propertyName)
		{
			if (this.ErrorsChanged != null)
			{
				this.ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
			}
		}

		protected void SetValidState(string propertyName, bool state)
		{
			this._validationErrors.Remove(propertyName);
			if (!state)
			{
				this._validationErrors.Add(propertyName, new List<string>()
				{
					""
				});
			}
			this.RaiseErrorsChanged(propertyName);
		}

		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
	}
}
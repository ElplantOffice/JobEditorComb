
using System;
using System.Collections;
using System.Reflection;

namespace Models
{
  internal class ModelProvider
  {
    public IModel Create(UIProtoType uiProtoType)
    {
      object obj = (object) null;
      if (uiProtoType == null)
        return (IModel) obj;
      if (uiProtoType.Model == null)
        return (IModel) obj;
      if (uiProtoType.Model.Assembly == (Assembly) null)
        return (IModel) obj;
      object[] objArray = new object[1]
      {
        (object) uiProtoType
      };
      try
      {
        obj = Activator.CreateInstance(uiProtoType.Model.Type, objArray);
      }
      catch (Exception ex)
      {
        IDictionary data = ex.Data;
      }
      return (IModel) obj;
    }
  }
}

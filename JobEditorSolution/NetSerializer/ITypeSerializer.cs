
using System;
using System.Collections.Generic;

namespace NetSerializer
{
  public interface ITypeSerializer
  {
    bool Handles(Type type);

    IEnumerable<Type> GetSubtypes(Type type);
  }
}

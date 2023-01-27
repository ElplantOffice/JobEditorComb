
using System;
using System.Reflection;

namespace Patterns.EventAggregator
{
  public interface IFileResourceDataInfo
  {
    Assembly Assembly { get; }

    Type DataType { get; }

    IFileResourceData Data { get; }
  }
}

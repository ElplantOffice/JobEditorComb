
using System;
using System.Reflection.Emit;

namespace NetSerializer
{
  public interface IDynamicTypeSerializer : ITypeSerializer
  {
    void GenerateWriterMethod(Type type, CodeGenContext ctx, ILGenerator il);

    void GenerateReaderMethod(Type type, CodeGenContext ctx, ILGenerator il);
  }
}

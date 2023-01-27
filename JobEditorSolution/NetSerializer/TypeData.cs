
using System.Reflection;
using System.Reflection.Emit;

namespace NetSerializer
{
  public sealed class TypeData
  {
    public readonly ushort TypeID;
    public readonly IDynamicTypeSerializer TypeSerializer;
    public MethodInfo WriterMethodInfo;
    public MethodInfo ReaderMethodInfo;
    public ILGenerator WriterILGen;
    public ILGenerator ReaderILGen;

    public TypeData(ushort typeID, IDynamicTypeSerializer serializer)
    {
      this.TypeID = typeID;
      this.TypeSerializer = serializer;
    }

    public TypeData(ushort typeID, MethodInfo writer, MethodInfo reader)
    {
      this.TypeID = typeID;
      this.WriterMethodInfo = writer;
      this.ReaderMethodInfo = reader;
    }

    public bool IsGenerated
    {
      get
      {
        return this.TypeSerializer != null;
      }
    }
  }
}

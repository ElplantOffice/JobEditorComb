
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace NetSerializer
{
  internal static class SerializerCodegen
  {
    public static DynamicMethod GenerateDynamicSerializerStub(Type type)
    {
      DynamicMethod dynamicMethod = new DynamicMethod("Serialize", (Type) null, new Type[2]
      {
        typeof (Stream),
        type
      }, typeof (Serializer), true);
      dynamicMethod.DefineParameter(1, ParameterAttributes.None, "stream");
      dynamicMethod.DefineParameter(2, ParameterAttributes.None, "value");
      return dynamicMethod;
    }

    public static void GenerateSerializerSwitch(CodeGenContext ctx, ILGenerator il, IDictionary<Type, TypeData> map)
    {
      LocalBuilder local = il.DeclareLocal(typeof (ushort));
      il.Emit(OpCodes.Ldarg_1);
      il.EmitCall(OpCodes.Call, Helpers.GetTypeIDMethodInfo, (Type[]) null);
      il.Emit(OpCodes.Stloc_S, local);
      il.Emit(OpCodes.Ldarg_0);
      il.Emit(OpCodes.Ldloc_S, local);
      il.EmitCall(OpCodes.Call, ctx.GetWriterMethodInfo(typeof (ushort)), (Type[]) null);
      Label[] labels = new Label[map.Count + 1];
      labels[0] = il.DefineLabel();
      foreach (KeyValuePair<Type, TypeData> keyValuePair in (IEnumerable<KeyValuePair<Type, TypeData>>) map)
        labels[(int) keyValuePair.Value.TypeID] = il.DefineLabel();
      il.Emit(OpCodes.Ldloc_S, local);
      il.Emit(OpCodes.Switch, labels);
      il.Emit(OpCodes.Newobj, Helpers.ExceptionCtorInfo);
      il.Emit(OpCodes.Throw);
      il.MarkLabel(labels[0]);
      il.Emit(OpCodes.Ret);
      foreach (KeyValuePair<Type, TypeData> keyValuePair in (IEnumerable<KeyValuePair<Type, TypeData>>) map)
      {
        Type key = keyValuePair.Key;
        TypeData typeData = keyValuePair.Value;
        il.MarkLabel(labels[(int) typeData.TypeID]);
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(key.IsValueType ? OpCodes.Unbox_Any : OpCodes.Castclass, key);
        il.EmitCall(OpCodes.Call, typeData.WriterMethodInfo, (Type[]) null);
        il.Emit(OpCodes.Ret);
      }
    }
  }
}

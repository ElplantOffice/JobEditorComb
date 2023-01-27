
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace NetSerializer
{
  internal static class DeserializerCodegen
  {
    public static DynamicMethod GenerateDynamicDeserializerStub(Type type)
    {
      DynamicMethod dynamicMethod = new DynamicMethod("Deserialize", (Type) null, new Type[2]
      {
        typeof (Stream),
        type.MakeByRefType()
      }, typeof (Serializer), true);
      dynamicMethod.DefineParameter(1, ParameterAttributes.None, "stream");
      dynamicMethod.DefineParameter(2, ParameterAttributes.Out, "value");
      return dynamicMethod;
    }

    public static void GenerateDeserializerSwitch(CodeGenContext ctx, ILGenerator il, IDictionary<Type, TypeData> map)
    {
      LocalBuilder local1 = il.DeclareLocal(typeof (ushort));
      il.Emit(OpCodes.Ldarg_0);
      il.Emit(OpCodes.Ldloca_S, local1);
      il.EmitCall(OpCodes.Call, ctx.GetReaderMethodInfo(typeof (ushort)), (Type[]) null);
      Label[] labels = new Label[map.Count + 1];
      labels[0] = il.DefineLabel();
      foreach (KeyValuePair<Type, TypeData> keyValuePair in (IEnumerable<KeyValuePair<Type, TypeData>>) map)
        labels[(int) keyValuePair.Value.TypeID] = il.DefineLabel();
      il.Emit(OpCodes.Ldloc_S, local1);
      il.Emit(OpCodes.Switch, labels);
      il.Emit(OpCodes.Newobj, Helpers.ExceptionCtorInfo);
      il.Emit(OpCodes.Throw);
      il.MarkLabel(labels[0]);
      il.Emit(OpCodes.Ldarg_1);
      il.Emit(OpCodes.Ldnull);
      il.Emit(OpCodes.Stind_Ref);
      il.Emit(OpCodes.Ret);
      foreach (KeyValuePair<Type, TypeData> keyValuePair in (IEnumerable<KeyValuePair<Type, TypeData>>) map)
      {
        Type key = keyValuePair.Key;
        TypeData typeData = keyValuePair.Value;
        il.MarkLabel(labels[(int) typeData.TypeID]);
        LocalBuilder local2 = il.DeclareLocal(key);
        il.Emit(OpCodes.Ldarg_0);
        if (local2.LocalIndex < 256)
          il.Emit(OpCodes.Ldloca_S, local2);
        else
          il.Emit(OpCodes.Ldloca, local2);
        il.EmitCall(OpCodes.Call, typeData.ReaderMethodInfo, (Type[]) null);
        il.Emit(OpCodes.Ldarg_1);
        if (local2.LocalIndex < 256)
          il.Emit(OpCodes.Ldloc_S, local2);
        else
          il.Emit(OpCodes.Ldloc, local2);
        if (key.IsValueType)
          il.Emit(OpCodes.Box, key);
        il.Emit(OpCodes.Stind_Ref);
        il.Emit(OpCodes.Ret);
      }
    }
  }
}

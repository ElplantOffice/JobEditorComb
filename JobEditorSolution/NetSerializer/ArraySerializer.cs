
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace NetSerializer
{
  internal class ArraySerializer : IDynamicTypeSerializer, ITypeSerializer
  {
    public bool Handles(Type type)
    {
      if (!type.IsArray)
        return false;
      if (type.GetArrayRank() != 1)
        throw new NotSupportedException(string.Format("Multi-dim arrays not supported: {0}", (object) type.FullName));
      return true;
    }

    public IEnumerable<Type> GetSubtypes(Type type)
    {
      yield return type.GetElementType();
    }

    public void GenerateWriterMethod(Type type, CodeGenContext ctx, ILGenerator il)
    {
      Type elementType = type.GetElementType();
      Label label1 = il.DefineLabel();
      il.Emit(OpCodes.Ldarg_1);
      il.Emit(OpCodes.Brtrue_S, label1);
      il.Emit(OpCodes.Ldarg_0);
      il.Emit(OpCodes.Ldc_I4_0);
      il.EmitCall(OpCodes.Call, ctx.GetWriterMethodInfo(typeof (uint)), (Type[]) null);
      il.Emit(OpCodes.Ret);
      il.MarkLabel(label1);
      il.Emit(OpCodes.Ldarg_0);
      il.Emit(OpCodes.Ldarg_1);
      il.Emit(OpCodes.Ldlen);
      il.Emit(OpCodes.Ldc_I4_1);
      il.Emit(OpCodes.Add);
      il.EmitCall(OpCodes.Call, ctx.GetWriterMethodInfo(typeof (uint)), (Type[]) null);
      LocalBuilder local = il.DeclareLocal(typeof (int));
      il.Emit(OpCodes.Ldc_I4_0);
      il.Emit(OpCodes.Stloc_S, local);
      Label label2 = il.DefineLabel();
      Label label3 = il.DefineLabel();
      il.Emit(OpCodes.Br_S, label3);
      il.MarkLabel(label2);
      il.Emit(OpCodes.Ldarg_0);
      il.Emit(OpCodes.Ldarg_1);
      il.Emit(OpCodes.Ldloc_S, local);
      il.Emit(OpCodes.Ldelem, elementType);
      Helpers.GenSerializerCall(ctx, il, elementType);
      il.Emit(OpCodes.Ldloc_S, local);
      il.Emit(OpCodes.Ldc_I4_1);
      il.Emit(OpCodes.Add);
      il.Emit(OpCodes.Stloc_S, local);
      il.MarkLabel(label3);
      il.Emit(OpCodes.Ldloc_S, local);
      il.Emit(OpCodes.Ldarg_1);
      il.Emit(OpCodes.Ldlen);
      il.Emit(OpCodes.Conv_I4);
      il.Emit(OpCodes.Clt);
      il.Emit(OpCodes.Brtrue_S, label2);
      il.Emit(OpCodes.Ret);
    }

    public void GenerateReaderMethod(Type type, CodeGenContext ctx, ILGenerator il)
    {
      Type elementType = type.GetElementType();
      LocalBuilder local1 = il.DeclareLocal(typeof (uint));
      il.Emit(OpCodes.Ldarg_0);
      il.Emit(OpCodes.Ldloca_S, local1);
      il.EmitCall(OpCodes.Call, ctx.GetReaderMethodInfo(typeof (uint)), (Type[]) null);
      Label label1 = il.DefineLabel();
      il.Emit(OpCodes.Ldloc_S, local1);
      il.Emit(OpCodes.Brtrue_S, label1);
      il.Emit(OpCodes.Ldarg_1);
      il.Emit(OpCodes.Ldnull);
      il.Emit(OpCodes.Stind_Ref);
      il.Emit(OpCodes.Ret);
      il.MarkLabel(label1);
      LocalBuilder local2 = il.DeclareLocal(type);
      il.Emit(OpCodes.Ldloc_S, local1);
      il.Emit(OpCodes.Ldc_I4_1);
      il.Emit(OpCodes.Sub);
      il.Emit(OpCodes.Newarr, elementType);
      il.Emit(OpCodes.Stloc_S, local2);
      LocalBuilder local3 = il.DeclareLocal(typeof (int));
      il.Emit(OpCodes.Ldc_I4_0);
      il.Emit(OpCodes.Stloc_S, local3);
      Label label2 = il.DefineLabel();
      Label label3 = il.DefineLabel();
      il.Emit(OpCodes.Br_S, label3);
      il.MarkLabel(label2);
      il.Emit(OpCodes.Ldarg_0);
      il.Emit(OpCodes.Ldloc_S, local2);
      il.Emit(OpCodes.Ldloc_S, local3);
      il.Emit(OpCodes.Ldelema, elementType);
      Helpers.GenDeserializerCall(ctx, il, elementType);
      il.Emit(OpCodes.Ldloc_S, local3);
      il.Emit(OpCodes.Ldc_I4_1);
      il.Emit(OpCodes.Add);
      il.Emit(OpCodes.Stloc_S, local3);
      il.MarkLabel(label3);
      il.Emit(OpCodes.Ldloc_S, local3);
      il.Emit(OpCodes.Ldloc_S, local2);
      il.Emit(OpCodes.Ldlen);
      il.Emit(OpCodes.Conv_I4);
      il.Emit(OpCodes.Clt);
      il.Emit(OpCodes.Brtrue_S, label2);
      il.Emit(OpCodes.Ldarg_1);
      il.Emit(OpCodes.Ldloc_S, local2);
      il.Emit(OpCodes.Stind_Ref);
      il.Emit(OpCodes.Ret);
    }
  }
}

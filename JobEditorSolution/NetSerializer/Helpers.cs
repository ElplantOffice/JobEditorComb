
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace NetSerializer
{
  internal static class Helpers
  {
    public static readonly ConstructorInfo ExceptionCtorInfo = typeof (Exception).GetConstructor(BindingFlags.Instance | BindingFlags.Public, (Binder) null, new Type[0], (ParameterModifier[]) null);
    public static readonly MethodInfo GetTypeIDMethodInfo = typeof (Serializer).GetMethod("GetTypeID", BindingFlags.Static | BindingFlags.NonPublic, (Binder) null, new Type[1]
    {
      typeof (object)
    }, (ParameterModifier[]) null);

    public static IEnumerable<FieldInfo> GetFieldInfos(Type type)
    {
      IOrderedEnumerable<FieldInfo> orderedEnumerable = ((IEnumerable<FieldInfo>) type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<FieldInfo>((Func<FieldInfo, bool>) (fi => (fi.Attributes & FieldAttributes.NotSerialized) == FieldAttributes.PrivateScope)).OrderBy<FieldInfo, string>((Func<FieldInfo, string>) (f => f.Name), (IComparer<string>) StringComparer.Ordinal);
      if (type.BaseType == (Type) null)
        return (IEnumerable<FieldInfo>) orderedEnumerable;
      return Helpers.GetFieldInfos(type.BaseType).Concat<FieldInfo>((IEnumerable<FieldInfo>) orderedEnumerable);
    }

    public static void GenSerializerCall(CodeGenContext ctx, ILGenerator il, Type type)
    {
            bool flag;
            MethodInfo methodInfo;
            if (type.IsValueType || type.IsArray)
            {
                flag = true;
            }
            else
            {
                flag = (!type.IsSealed || ctx.IsGenerated(type) ? false : true);
            }
            methodInfo = (flag ? ctx.GetWriterMethodInfo(type) : ctx.SerializerSwitchMethodInfo);
            il.EmitCall(OpCodes.Call, methodInfo, null);
        }

    public static void GenDeserializerCall(CodeGenContext ctx, ILGenerator il, Type type)
    {
            bool flag;
            MethodInfo methodInfo;
            if (type.IsValueType || type.IsArray)
            {
                flag = true;
            }
            else
            {
                flag = (!type.IsSealed || ctx.IsGenerated(type) ? false : true);
            }
            methodInfo = (flag ? ctx.GetReaderMethodInfo(type) : ctx.DeserializerSwitchMethodInfo);
            il.EmitCall(OpCodes.Call, methodInfo, null);
        }
  }
}

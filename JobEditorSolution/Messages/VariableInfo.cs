
using System;
using System.Linq.Expressions;

namespace Messages
{
  public static class VariableInfo
  {
    public static string GetName(LambdaExpression expression)
    {
      string str = (string) null;
      if (!(expression.Body is MethodCallExpression) && !(expression.Body is MemberExpression))
        throw new ArgumentException("The body of the expression must be a methodecall or memberref", nameof (expression));
      Expression expression1 = expression.Body;
      MemberExpression memberExpression;
      if (expression1.NodeType == ExpressionType.Call)
      {
        string name = ((MethodCallExpression) expression1).Method.Name;
        str = str != null ? str.Insert(0, name + ".") : name;
      }
      else
      {
        for (; expression1.NodeType == ExpressionType.MemberAccess; expression1 = memberExpression.Expression)
        {
          memberExpression = (MemberExpression) expression1;
          string name = memberExpression.Member.Name;
          str = str != null ? str.Insert(0, name + ".") : name;
        }
      }
      return str;
    }
  }
}

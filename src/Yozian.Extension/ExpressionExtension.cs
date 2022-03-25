﻿using System;
using System.Linq.Expressions;

namespace Yozian.Extension
{
    public static class ExpressionExtension
    {
        public static string GetMemberName(this LambdaExpression memberSelector)
        {
            Func<Expression, string> nameSelector = null; //recursive func
            nameSelector = e =>                           //or move the entire thing to a separate recursive method
            {
                switch (e.NodeType)
                {
                    case ExpressionType.Parameter:
                        return ((ParameterExpression) e).Name;

                    case ExpressionType.MemberAccess:
                        return ((MemberExpression) e).Member.Name;

                    case ExpressionType.Call:
                        return ((MethodCallExpression) e).Method.Name;

                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                        return nameSelector(((UnaryExpression) e).Operand);

                    case ExpressionType.Invoke:
                        return nameSelector(((InvocationExpression) e).Expression);

                    case ExpressionType.ArrayLength:
                        return "Length";

                    default:
                        throw new Exception("Not a proper member selector");
                }
            };

            return nameSelector(memberSelector.Body);
        }
    }
}

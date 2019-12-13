using System;
using System.Linq.Expressions;

namespace Serilog.Sanitizer.Extensions
{
    internal static class ExpressionExtensions
    {
        public static string GetPropertyNameFromExpression<T>(this Expression<Func<T, object>> expression)
        {
            var expressionBody = expression.Body;

            var memberExpression = GetMemberExpression(expressionBody);

            if (memberExpression == null || GetMemberExpression(memberExpression.Expression) != null)
            {
                throw new ArgumentException
                (
                    $"{expressionBody.GetType().Name} does not resolve to a property of form x => x.Property", 
                    nameof(expression)
                );
            }

            return memberExpression.Member.Name;
        }

        private static MemberExpression GetMemberExpression(Expression expression)
        {
            return 
                expression is UnaryExpression bodyOfExpression
                    ? bodyOfExpression.Operand as MemberExpression
                    : expression as MemberExpression;
        }
    }
}
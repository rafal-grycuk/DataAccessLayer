using System;
using System.Linq.Expressions;

namespace DataAccessLayer.Core.Interfaces.Infrastructure
{
    public class ExpressionMap<T>
    {
        public Expression<Func<T, object>> IncludeExpression { get; }

        public Expression<Func<T, object>>[] ThenIncludeExpression { get; set; }

        public ExpressionMap(Expression<Func<T, object>> includeExpression, params Expression<Func<T, object>>[] thenIncludeExpression)
        {
            IncludeExpression = includeExpression;
            ThenIncludeExpression = thenIncludeExpression;
        }


    }
}

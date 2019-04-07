using System;
using System.Collections.Generic;
using MongoDB.Driver;
using ProfileApi.Models;
using ProfileApi.Models.Query;

namespace ProfileApi.Services.QueryService
{
    public class QueryBuilder<T> : IQueryBuilder<T>
    {
        private readonly IRepository<T> _Repository;
        public QueryBuilder(IRepository<T> Repository)
        {
            _Repository = Repository;
        }

        public FilterDefinition<T> Build(IList<Expression> expressions)
        {
            var filters = new List<FilterDefinition<T>>();
            FilterDefinition<T> filter = null;

            for (int i = 0; i < expressions.Count; i++)
            {
                var currentExpr = expressions[i];
                if (i % 2 != 0)
                {
                    var prevFilter = filters[i - 1];
                    var prevExpr = expressions[i - 1];
                    if (String.Equals(prevExpr.LogicalCondition, "and", StringComparison.OrdinalIgnoreCase))
                    {
                        filter = prevFilter & GetFilter(currentExpr);
                    }
                    else if (String.Equals(prevExpr.LogicalCondition, "or", StringComparison.OrdinalIgnoreCase))
                    {
                        filter = filter | prevFilter;
                    }
                }
                else
                {
                    filter = GetFilter(currentExpr);
                }
                filters.Add(filter);
            }

            return filter;

        }

        private FilterDefinition<T> GetFilter(Expression expr)
        {
            var builder = Builders<T>.Filter;
            FilterDefinition<T> filter = null;

            switch (expr.Operator)
            {
                case "=":
                    filter = builder.Eq(expr.Operand, expr.Value);
                    break;
                case ">":
                    filter = builder.Gt(expr.Operand, expr.Value);
                    break;
                case "<":
                    filter = builder.Lt(expr.Operand, expr.Value);
                    break;
                default:
                    break;
            }

            return filter;

        }
    }
}
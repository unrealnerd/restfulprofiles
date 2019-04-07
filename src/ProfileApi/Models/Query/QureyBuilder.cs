using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using ProfileApi.Services;
using System.Linq;

namespace ProfileApi.Models.Query
{
    public class QueryBuilder
    {
        private readonly IRepository<Profile> _profileRepository;
        public QueryBuilder(IRepository<Profile> profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public FilterDefinition<Profile> Build(Query query)
        {
            var filters = new List<FilterDefinition<Profile>>();
            FilterDefinition<Profile> filter = null;

            for (int i = 0; i < query.Expressions.Length; i++)
            {
                var currentExpr = query.Expressions[i];
                if (i % 2 != 0)
                {
                    var prevFilter = filters[i - 1];
                    var prevExpr = query.Expressions[i - 1];
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

        private FilterDefinition<Profile> GetFilter(Expression expr)
        {
            var builder = Builders<Profile>.Filter;
            FilterDefinition<Profile> filter = null;

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
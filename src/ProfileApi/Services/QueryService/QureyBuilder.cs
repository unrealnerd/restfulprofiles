using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ProfileApi.Models;
using ProfileApi.Models.Query;
using ProfileApi.Services.LoggerService;

namespace ProfileApi.Services.QueryService
{
    public class QueryBuilder<T> : IQueryBuilder<T>
    {
        private readonly IRepository<T> _Repository;
        private readonly ILogger _logger;
        public QueryBuilder(IRepository<T> Repository, ILogger<T> logger)
        {
            _Repository = Repository;
            _logger = logger;
        }

        public FilterDefinition<T> Build(IList<Expression> expressions)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("There is some error while building query filters. Message:{message}", ex.ToErrorResponse(LoggingEvents.UnkownError));
                return null;
            }

        }

        private FilterDefinition<T> GetFilter(Expression expr)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError("There is some error while getting filters query filters. Message:{message}", ex.ToErrorResponse(LoggingEvents.UnkownError));
                return null;
            }

        }
    }
}
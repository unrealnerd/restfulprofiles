using System.Collections.Generic;
using MongoDB.Driver;
using ProfileApi.Models;
using ProfileApi.Models.Query;

namespace ProfileApi.Services.QueryService
{
    public interface IQueryBuilder<T>
    {
        FilterDefinition<T> Build(IList<Expression> expressions);
    }
}
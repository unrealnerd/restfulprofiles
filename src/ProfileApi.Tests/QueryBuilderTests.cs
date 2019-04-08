using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using ProfileApi.Models;
using ProfileApi.Models.Query;
using ProfileApi.Services;
using ProfileApi.Services.QueryService;
using Xunit;

namespace ProfileApi.Tests
{
    public class QueryBuilderTests
    {
        private readonly IRepository<Profile> _Repository = new Mock<IRepository<Profile>>().Object;
        private readonly ILogger<Profile> _logger = new Mock<ILogger<Profile>>().Object;

        [Fact]
        public void Build_Returns_Filter_When_InputExpressions_AreValid()
        {
            //Arrange
            var queryBuilder = new QueryBuilder<Profile>(_Repository, _logger);

            var inputExpression = new Expression
            {
                Operand = "",
                Operator = "=",
                Value = "",
                LogicalCondition = "and"
            };
            var inputExpression2 = new Expression
            {
                Operand = "",
                Operator = "<",
                Value = "",
                LogicalCondition = ""
            };

            //Act
            var filter = queryBuilder.Build(new List<Expression> { inputExpression, inputExpression2 });

            //Assert
            Assert.NotNull(filter);
            Assert.Matches("^MongoDB.Driver.AndFilterDefinition*", filter.ToString());// an AND filter expression is returned
        }

        [Fact]
        public void Build_Returns_null_When_One_of_The_InputExpressions_Are_MissingOperator()
        {
            //Arrange
            var queryBuilder = new QueryBuilder<Profile>(_Repository, _logger);

            var inputExpression = new Expression
            {
                Operand = "",
                Operator = "",
                Value = "",
                LogicalCondition = "and"
            };
            var inputExpression2 = new Expression
            {
                Operand = "",
                Operator = "<",
                Value = "",
                LogicalCondition = ""
            };

            //Act
            var filter = queryBuilder.Build(new List<Expression> { inputExpression, inputExpression2 });

            //Assert
            Assert.Null(filter);
        }
    }
}
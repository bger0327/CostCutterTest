using Dapper;
using ui;
using Xunit;

namespace unit_tests
{
    public class ExampleTest
    {
        [Fact]
        public void ShouldPass()
        {
            var dbConnection = new Database().GetConnection;
            dbConnection.Open();

            var sql = "SELECT 1";
            var expectedResult = 1;
            var actualResult = dbConnection.QueryFirst<int>(sql);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
using Frends.ODBC.ExecuteQuery.Definitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Odbc;

namespace Frends.ODBC.ExecuteQuery.Tests;

[TestClass]
public class UnitTests
{
    /// <summary>
    /// docker-compose -f DB/docker-compose.yml up -d && sudo -i && sudo apt-get update && sudo apt-get -y install dos2unix && sudo dos2unix ./_build/prebuild_script.sh && chmod 777 ./_build/prebuild_script.sh && ./_build/prebuild_script.sh
    /// </summary>
    private static readonly string _connString = "Driver={ODBC Driver 17 for SQL Server};Server=127.0.0.1,1433; Database=UnitTests;DSN=ODBC_testDB;Uid=sa;Pwd=yourStrong!Password;";
    private static readonly string _tableName = "AnimalTypes";

    [TestCleanup]
    public void CleanUp()
    {
        using var connection = new OdbcConnection(_connString);
        connection.Open();
        var createTable = connection.CreateCommand();
        createTable.CommandText = $@"DELETE FROM AnimalTypes WHERE ID > 2";
        createTable.ExecuteNonQuery();
        connection.Close();
        connection.Dispose();
    }

    [TestMethod]
    public async Task ShouldReadFromMsSQLViaOdbc_ExecuteTypes_Auto()
    {
        var input = new Input
        {
            ConnectionString = _connString,
            ExecuteType = ExecuteTypes.Auto,
            ParametersInOrder = new[] { new QueryParameter { Value = "Mammal" }, new QueryParameter { Value = "Bird" } },
            Query = "SELECT Animal FROM AnimalTypes WHERE Animal = ? OR Animal = ?"
        };

        var options = new Options
        {
            CommandTimeoutSeconds = 30,
            ThrowErrorOnFailure = true,
        };

        var result = await ODBC.ExecuteQuery(input, options, default);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(-1, result.RecordsAffected);
        Assert.AreEqual("Mammal", result.Data.First.Animal.ToString());
        Assert.AreEqual("Bird", result.Data.Last.Animal.ToString());
    }

    [TestMethod]
    public async Task ShouldReadFromMsSQLViaOdbc_ExecuteTypes_Scalar()
    {
        var input = new Input
        {
            ConnectionString = _connString,
            ExecuteType = ExecuteTypes.Scalar,
            ParametersInOrder = new[] { new QueryParameter { Value = "Mammal" }, new QueryParameter { Value = "Bird" } },
            Query = "SELECT Animal FROM AnimalTypes WHERE Animal = ? OR Animal = ?"
        };

        var options = new Options
        {
            CommandTimeoutSeconds = 30,
            ThrowErrorOnFailure = true,
        };

        var result = await ODBC.ExecuteQuery(input, options, default);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(1, result.RecordsAffected);
        Assert.AreEqual("Mammal", result.Data.Value.ToString());
    }

    [TestMethod]
    public async Task ShouldReadFromMsSQLViaOdbc_ExecuteTypes_ExecuteReader()
    {
        var input = new Input
        {
            ConnectionString = _connString,
            ExecuteType = ExecuteTypes.ExecuteReader,
            ParametersInOrder = new[] { new QueryParameter { Value = "Mammal" }, new QueryParameter { Value = "Bird" } },
            Query = "SELECT Animal FROM AnimalTypes WHERE Animal = ? OR Animal = ?"
        };

        var options = new Options
        {
            CommandTimeoutSeconds = 30,
            ThrowErrorOnFailure = true,
        };

        var result = await ODBC.ExecuteQuery(input, options, default);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(-1, result.RecordsAffected);
        Assert.AreEqual("Mammal", result.Data.First.Animal.ToString());
        Assert.AreEqual("Bird", result.Data.Last.Animal.ToString());
    }

    [TestMethod]
    public async Task ShouldReadFromMsSQLViaOdbc_ExecuteTypes_NonQuery()
    {
        var selectInput = new Input
        {
            ConnectionString = _connString,
            ExecuteType = ExecuteTypes.NonQuery,
            ParametersInOrder = new[] { new QueryParameter { Value = "Mammal" }, new QueryParameter { Value = "Bird" } },
            Query = "SELECT Animal FROM AnimalTypes WHERE Animal = ? OR Animal = ?"
        };

        var insertInput = new Input
        {
            ConnectionString = _connString,
            ExecuteType = ExecuteTypes.NonQuery,
            ParametersInOrder = new[] { new QueryParameter { Value = "Fish" } },
            Query = $@"INSERT INTO AnimalTypes VALUES (3, ?)"
        };

        var updateInput = new Input
        {
            ConnectionString = _connString,
            ExecuteType = ExecuteTypes.NonQuery,
            ParametersInOrder = new[] { new QueryParameter { Value = "Reptile" }, new QueryParameter { Value = "Fish" } },
            Query = "UPDATE AnimalTypes SET Animal = ? WHERE Animal = ?"
        };

        var deleteInput = new Input
        {
            ConnectionString = _connString,
            ExecuteType = ExecuteTypes.NonQuery,
            ParametersInOrder = new[] { new QueryParameter { Value = "Reptile" } },
            Query = "DELETE FROM AnimalTypes WHERE Animal = ?"
        };

        var options = new Options
        {
            CommandTimeoutSeconds = 30,
            ThrowErrorOnFailure = true,
        };

        var queryResult = await ODBC.ExecuteQuery(selectInput, options, default);
        Assert.IsNotNull(queryResult);
        Assert.IsTrue(queryResult.Success);
        Assert.AreEqual(-1, queryResult.RecordsAffected);
        Assert.AreEqual(-1, int.Parse(queryResult.Data.AffectedRows.ToString()));

        var insertResult = await ODBC.ExecuteQuery(insertInput, options, default);
        Assert.IsNotNull(insertResult);
        Assert.IsTrue(insertResult.Success);
        Assert.AreEqual(1, insertResult.RecordsAffected);
        Assert.AreEqual(1, int.Parse(insertResult.Data.AffectedRows.ToString()));

        Assert.AreEqual(3, GetCount());

        var updateResult = await ODBC.ExecuteQuery(updateInput, options, default);
        Assert.IsNotNull(updateResult);
        Assert.IsTrue(updateResult.Success);
        Assert.AreEqual(1, updateResult.RecordsAffected);
        Assert.AreEqual(1, int.Parse(updateResult.Data.AffectedRows.ToString()));

        var deleteResult = await ODBC.ExecuteQuery(deleteInput, options, default);
        Assert.IsNotNull(deleteResult);
        Assert.IsTrue(deleteResult.Success);
        Assert.AreEqual(1, deleteResult.RecordsAffected);
        Assert.AreEqual(1, int.Parse(deleteResult.Data.AffectedRows.ToString()));

        Assert.AreEqual(2, GetCount());
    }

    private static int GetCount()
    {
        using var connection = new OdbcConnection(_connString);
        connection.Open();
        var getRows = connection.CreateCommand();
        getRows.CommandText = $"SELECT COUNT(*) FROM {_tableName}";
        var count = getRows.ExecuteScalar();
        connection.Close();
        connection.Dispose();
        return count != null ? (int)count : -1;
    }
}
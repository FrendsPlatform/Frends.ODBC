using Frends.ODBC.ExecuteQuery.Definitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Odbc;

namespace Frends.ODBC.ExecuteQuery.Tests;

[TestClass]
public class UnitTests
{
    // Create OdbcDsn using PowerShell as admin (check file location):
    //Add-OdbcDsn -Name "ODBC_testDB" -DriverName "SQL Server" -DsnType "User" -Platform "64-bit" -SetPropertyValue @("Name=ODBC_testDB", "Server=localhost", "Trusted_Connection=Yes", "Database=UnitTests")
    // or 
    //Add-OdbcDsn -Name "ODBC_testDB" -DriverName "Microsoft Access Driver (*.mdb, *.accdb)" -DsnType "User" -Platform "32-bit" -SetPropertyValue "Dbq=$pwd\TestFiles\ODBC_testDB_32.mdb"  

    // Remove
    //Remove-OdbcDsn -Name "ODBC_testDB" -DsnType "User" -Platform "64-bit"

    private static readonly string _connString = "Driver={ODBC Driver 17 for SQL Server};Server=localhost;Database=UnitTests;DSN=ODBC_testDB;Uid=sa;Pwd=yourStrong!Password;";
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
        Assert.AreEqual("{\r\n  \"Animal\": \"Mammal\"\r\n}", result.Data.First.ToString());
        Assert.AreEqual("{\r\n  \"Animal\": \"Bird\"\r\n}", result.Data.Last.ToString());
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
        Assert.AreEqual("{\r\n  \"Value\": \"Mammal\"\r\n}", result.Data.ToString());
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
        Assert.AreEqual("{\r\n  \"Animal\": \"Mammal\"\r\n}", result.Data.First.ToString());
        Assert.AreEqual("{\r\n  \"Animal\": \"Bird\"\r\n}", result.Data.Last.ToString());
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
        Assert.AreEqual("{\r\n  \"AffectedRows\": -1\r\n}", queryResult.Data.ToString());

        var insertResult = await ODBC.ExecuteQuery(insertInput, options, default);
        Assert.IsNotNull(insertResult);
        Assert.IsTrue(insertResult.Success);
        Assert.AreEqual(1, insertResult.RecordsAffected);
        Assert.AreEqual("{\r\n  \"AffectedRows\": 1\r\n}", insertResult.Data.ToString());

        Assert.AreEqual(3, GetCount());

        var updateResult = await ODBC.ExecuteQuery(updateInput, options, default);
        Assert.IsNotNull(updateResult);
        Assert.IsTrue(updateResult.Success);
        Assert.AreEqual(1, updateResult.RecordsAffected);
        Assert.AreEqual("{\r\n  \"AffectedRows\": 1\r\n}", updateResult.Data.ToString());

        var deleteResult = await ODBC.ExecuteQuery(deleteInput, options, default);
        Assert.IsNotNull(deleteResult);
        Assert.IsTrue(deleteResult.Success);
        Assert.AreEqual(1, deleteResult.RecordsAffected);
        Assert.AreEqual("{\r\n  \"AffectedRows\": 1\r\n}", deleteResult.Data.ToString());

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

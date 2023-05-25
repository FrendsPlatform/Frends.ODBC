using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.ODBC.ExecuteQuery.Definitions;

/// <summary>
/// Input parameters.
/// </summary>
public class Input
{
    /// <summary>
    /// Connection string.
    /// </summary>
    /// <example>Driver={driver};Server=127.0.0.1,1433; Database=UnitTests;DSN=ODBC_testDB;Uid=sa;Pwd=yourStrong!Password;</example>
    [DisplayFormat(DataFormatString = "Text")]
    [PasswordPropertyText]
    public string ConnectionString { get; set; }

    /// <summary>
    /// Query to be executed in string format.
    /// </summary>
    /// <example>
    /// SELECT * FROM MyTable WHERE ID = ?,
    /// INSERT INTO MyTable (id, first_name, last_name) VALUES (?, ?, ?),
    /// </example>
    [DisplayFormat(DataFormatString = "Sql")]
    public string Query { get; set; }

    /// <summary>
    /// Parameters for the database query.
    /// </summary>
    /// <example>
    /// [
    ///     { Value = "1" },
    ///     { Value = "Foo" }
    ///     { Value = "Bar" }
    /// ]
    /// </example>
    public QueryParameter[] ParametersInOrder { get; set; }

    /// <summary>
    /// Specifies how a command string is interpreted.
    /// Auto: ExecuteReader for SELECT-query and NonQuery for UPDATE, INSERT, or DELETE statements.
    /// ExecuteReader: Use this operation to execute any arbitrary SQL statements in SQL Server if you want the result set to be returned.
    /// NonQuery: Use this operation to execute any arbitrary SQL statements in SQL Server if you do not want any result set to be returned. You can use this operation to create database objects or change data in a database by executing UPDATE, INSERT, or DELETE statements. The return value of this operation is of Int32 data type, and For the UPDATE, INSERT, and DELETE statements, the return value is the number of rows affected by the SQL statement. For all other types of statements, the return value is -1.
    /// Scalar: Use this operation to execute any arbitrary SQL statements in SQL Server to return a single value. This operation returns the value only in the first column of the first row in the result set returned by the SQL statement.
    /// </summary>
    /// <example>ExecuteType.ExecuteReader</example>
    [DefaultValue(ExecuteTypes.ExecuteReader)]
    public ExecuteTypes ExecuteType { get; set; }
}
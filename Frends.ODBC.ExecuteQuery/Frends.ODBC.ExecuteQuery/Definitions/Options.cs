using System.ComponentModel;

namespace Frends.ODBC.ExecuteQuery.Definitions;

/// <summary>
/// Optional parameters.
/// </summary>
public class Options
{
    /// <summary>
    /// (true) Throw an exception or (false) stop the Task and return result object containing Result.Success = false and Result.ErrorMessage = 'exception message'.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; }

    /// <summary>
    /// Number of seconds for the operation to complete before it times out.
    /// </summary>
    /// <example>60</example>
    [DefaultValue(60)]
    public int CommandTimeoutSeconds { get; set; }

    /// <summary>
    /// Output mode. Default is JToken.
    /// DataReader is valid only for ExecuteReader queries.
    /// For JToken output, the data is returned in Data property as a dynamic JToken type.
    /// For DataReader output, the data is returned in DataReader property as OdbcDataReader type.
    /// If using DataReader, remember to dispose task result at end of scope.
    /// </summary>
    /// <example>JToken</example>
    public OutputMode OutputMode { get; set; }
}
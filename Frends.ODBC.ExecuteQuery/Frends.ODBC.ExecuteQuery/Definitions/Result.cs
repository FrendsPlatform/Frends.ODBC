using System;
using System.Data.Odbc;
using System.Threading.Tasks;

namespace Frends.ODBC.ExecuteQuery.Definitions;

/// <summary>
/// Task's result.
/// </summary>
public class Result : IAsyncDisposable
{
    private OdbcConnection _disposableConnection;
    private OdbcCommand _disposableCommand;

    /// <summary>
    /// Operation complete without errors.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; private set; }

    /// <summary>
    /// Records affected.
    /// Some statements will return -1. See documentation of Input.ExecuteType for more information.
    /// </summary>
    /// <example>100</example>
    public int RecordsAffected { get; private set; }

    /// <summary>
    /// Error details. Null when Success is true.
    /// </summary>
    /// <example>null</example>
    public Error Error { get; private set; }

    /// <summary>
    /// Query result as JToken.
    /// </summary>
    /// <example>
    /// Input.ExecuteType = ExecuteReader: [{"ID": "1","FIRST_NAME": "Saija","LAST_NAME": "Saijalainen","START_DATE": ""}],
    /// Input.ExecuteType = NonQuery: {{  "AffectedRows": -1 }},
    /// Input.ExecuteType = Scalar: {{  "Value": 1 }}
    /// </example>
    public dynamic Data { get; private set; }

    /// <summary>
    /// Gets the DataReaderWrapper instance when OutputMode is set to DataReader.
    /// </summary>
    /// <example>DataReaderWrapper object</example>
    public DataReaderWrapper DataReader { get; init; }

    internal Result(bool success, int recordsAffected, dynamic data)
    {
        Success = success;
        RecordsAffected = recordsAffected;
        Data = data;
    }

    internal Result(bool success, int recordsAffected, DataReaderWrapper dataReader)
    {
        Success = success;
        RecordsAffected = recordsAffected;
        DataReader = dataReader;
    }

    internal Result(bool success, Error error)
    {
        Success = success;
        Error = error;
    }

    internal void SetDisposableResources(OdbcConnection connection, OdbcCommand command)
    {
        _disposableConnection = connection;
        _disposableCommand = command;
    }

    /// <summary>
    /// Disposes the connection, command and data reader if OutputMode is DataReader.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (DataReader != null)
        {
            await DataReader.DisposeAsync();
        }
        if (_disposableCommand != null)
        {
            await _disposableCommand.DisposeAsync();
        }
        if (_disposableConnection != null)
        {
            await _disposableConnection.DisposeAsync();
        }
        OdbcConnection.ReleaseObjectPool();
        GC.SuppressFinalize(this);
    }
}

﻿using System;
using System.Data.Common;
using System.Data.Odbc;
using System.Threading.Tasks;

namespace Frends.ODBC.ExecuteQuery.Definitions;

/// <summary>
/// Task's result.
/// </summary>
public class Result : IDisposable
{
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
    /// Error message.
    /// This value is generated when an exception occurs and Options.ThrowErrorOnFailure = false.
    /// </summary>
    /// <example>Error occured...</example>
    public string ErrorMessage { get; private set; }

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
    /// OdbsDataReader object that is returned only if OutputMode is set to DataReader.
    /// </summary>
    /// <example>OdbcDataReader object</example>
    public OdbcDataReader DataReader { get; init; }

    /// <summary>
    /// This is used to dispose the connection and command if OutputMode is DataReader.
    /// </summary>
    internal OdbcConnection DisposableConnection { get; set; }

    /// <summary>
    /// This is used to dispose the connection and command if OutputMode is DataReader.
    /// </summary>
    internal OdbcCommand DisposableCommand { get; set; }

    internal Result(bool success, int recordsAffected, string errorMessage, dynamic data)
    {
        Success = success;
        RecordsAffected = recordsAffected;
        ErrorMessage = errorMessage;
        Data = data;
    }

    internal Result(bool success, int recordsAffected, OdbcDataReader dataReader)
    {
        Success = success;
        RecordsAffected = recordsAffected;
        DataReader = dataReader;
    }

    /// <summary>
    /// Disposes the connection, command and data reader if OutputMode is DataReader.
    /// </summary>
    public void Dispose()
    {
        DisposableConnection?.Dispose();
        DisposableCommand?.Dispose();
        DataReader?.Dispose();

        OdbcConnection.ReleaseObjectPool();
    }
}
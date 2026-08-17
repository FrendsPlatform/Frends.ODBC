using System;

namespace Frends.ODBC.ExecuteQuery.Definitions;

/// <summary>
/// Error details.
/// </summary>
public class Error
{
    /// <summary>
    /// Error message.
    /// </summary>
    /// <example>ExecuteQuery exception: ...</example>
    public string Message { get; init; }

    /// <summary>
    /// The exception that caused the error.
    /// </summary>
    /// <example>null</example>
    public Exception AdditionalInfo { get; init; }
}

using Frends.ODBC.ExecuteQuery.Definitions;
using NUnit.Framework;
using System;
using System.Threading;

namespace Frends.ODBC.ExecuteQuery.Tests;

[TestFixture]
internal class ErrorHandlerTest
{
    private const string CustomErrorMessage = "CustomErrorMessage";

    private static Input InvalidInput() => new Input
    {
        ConnectionString = "Driver={Invalid Driver};Server=invalid-host;",
        ExecuteType = ExecuteTypes.Auto,
        Query = "SELECT 1",
        ParametersInOrder = null,
    };

    private static Options DefaultOptions() => new Options
    {
        CommandTimeoutSeconds = 5,
        ThrowErrorOnFailure = true,
        ErrorMessageOnFailure = string.Empty,
    };

    [Test]
    public void Should_Throw_Error_When_ThrowErrorOnFailure_Is_True()
    {
        var ex = Assert.Throws<AggregateException>(() =>
            ODBC.ExecuteQuery(InvalidInput(), DefaultOptions(), CancellationToken.None).Wait());
        Assert.That(ex, Is.Not.Null);
    }

    [Test]
    public void Should_Return_Failed_Result_When_ThrowErrorOnFailure_Is_False()
    {
        var options = DefaultOptions();
        options.ThrowErrorOnFailure = false;
        var result = ODBC.ExecuteQuery(InvalidInput(), options, CancellationToken.None).Result;
        Assert.That(result.Success, Is.False);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error.Message, Is.Not.Null.Or.Empty);
    }

    [Test]
    public void Should_Use_Custom_ErrorMessageOnFailure()
    {
        var options = DefaultOptions();
        options.ErrorMessageOnFailure = CustomErrorMessage;
        var ex = Assert.Throws<AggregateException>(() =>
            ODBC.ExecuteQuery(InvalidInput(), options, CancellationToken.None).Wait());
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex.InnerException!.Message, Does.Contain(CustomErrorMessage));
    }
}

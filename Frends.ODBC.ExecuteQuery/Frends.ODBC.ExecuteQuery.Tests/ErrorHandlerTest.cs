using Frends.ODBC.ExecuteQuery.Definitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Frends.ODBC.ExecuteQuery.Tests;

[TestClass]
public class ErrorHandlerTest
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

    [TestMethod]
    public async Task Should_Throw_Error_When_ThrowErrorOnFailure_Is_True()
    {
        await Assert.ThrowsExceptionAsync<Exception>(() =>
            ODBC.ExecuteQuery(InvalidInput(), DefaultOptions(), CancellationToken.None));
    }

    [TestMethod]
    public async Task Should_Return_Failed_Result_When_ThrowErrorOnFailure_Is_False()
    {
        var options = DefaultOptions();
        options.ThrowErrorOnFailure = false;
        var result = await ODBC.ExecuteQuery(InvalidInput(), options, CancellationToken.None);
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
        Assert.IsNotNull(result.Error.Message);
    }

    [TestMethod]
    public async Task Should_Use_Custom_ErrorMessageOnFailure()
    {
        var options = DefaultOptions();
        options.ErrorMessageOnFailure = CustomErrorMessage;
        var ex = await Assert.ThrowsExceptionAsync<Exception>(() =>
            ODBC.ExecuteQuery(InvalidInput(), options, CancellationToken.None));
        Assert.IsNotNull(ex);
        StringAssert.Contains(ex.Message, CustomErrorMessage);
    }
}

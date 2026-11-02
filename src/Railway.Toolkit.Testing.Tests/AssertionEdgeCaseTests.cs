namespace Railway.Toolkit.Testing.Tests;

public class AssertionEdgeCaseTests
{
    private static async Task AssertAsync(int assertion, Task<Result<int>> task)
    {
        switch (assertion)
        {
            case 0: await task.ShouldBeOkAsync(); break;
            case 1: await task.ShouldBeOkAsync(42); break;
            case 2: await task.ShouldBeOkWhereAsync(value => value == 42); break;
            case 3: await task.ShouldBeFailAsync(); break;
            case 4: await task.ShouldBeFailAsync("CODE"); break;
            case 5: await task.ShouldBeFailAsync("CODE", "message"); break;
            case 6: await task.ShouldBeFailWhereAsync(error => error.Code == "CODE"); break;
            default: throw new ArgumentOutOfRangeException(nameof(assertion));
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public async Task AsyncAssertions_RejectNullTasks(int assertion)
    {
        ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => AssertAsync(assertion, null!));
        Assert.Equal("resultTask", exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public async Task AsyncAssertions_PreserveTaskFaultsAndCancellation(int assertion)
    {
        InvalidOperationException failure = new("pipeline failed");
        Exception actual = await Assert.ThrowsAsync<InvalidOperationException>(
            () => AssertAsync(assertion, Task.FromException<Result<int>>(failure)));
        Assert.Same(failure, actual);

        using CancellationTokenSource cancellation = new();
        cancellation.Cancel();
        OperationCanceledException canceled = await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => AssertAsync(assertion, Task.FromCanceled<Result<int>>(cancellation.Token)));
        Assert.Equal(cancellation.Token, canceled.CancellationToken);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public async Task AsyncAssertions_ReportWrongResultState(int assertion)
    {
        Result<int> result = assertion < 3 ? Result.Fail<int>("message", "CODE") : Result.Ok(42);
        RailwayAssertionException exception = await Assert.ThrowsAsync<RailwayAssertionException>(
            () => AssertAsync(assertion, Task.FromResult(result)));
        Assert.Equal(assertion < 3
            ? "Expected Result<Int32> to be Ok, but it was Fail with Error [CODE] message."
            : "Expected Result<Int32> to be Fail, but it was Ok with value '42'.", exception.Message);
    }

    [Fact]
    public async Task ExpectedMessage_RejectsNullSynchronouslyAndAsynchronously()
    {
        Result<int> result = Result.Fail<int>("message", "CODE");
        Assert.Equal("expectedMessage", Assert.Throws<ArgumentNullException>(
            () => result.ShouldBeFail("CODE", null!)).ParamName);
        ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => Task.FromResult(result).ShouldBeFailAsync("CODE", null!));
        Assert.Equal("expectedMessage", exception.ParamName);
    }

    [Fact]
    public async Task ExpectedValue_UsesCustomComparerAndReturnsActualValue()
    {
        Result<string> result = Result.Ok("VALUE");
        Assert.Equal("VALUE", result.ShouldBeOk("value", StringComparer.OrdinalIgnoreCase));
        Assert.Equal("VALUE", await Task.FromResult(result).ShouldBeOkAsync("value", StringComparer.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task Ok_CanContainNull()
    {
        Result<string?> result = Result.Ok<string?>(null);
        Assert.Null(result.ShouldBeOk());
        Assert.Null(result.ShouldBeOk(null));
        Assert.Null(await Task.FromResult(result).ShouldBeOkAsync((string?)null));
    }

    [Fact]
    public async Task Predicates_ReportReasonAndPreserveUserExceptions()
    {
        Result<int> ok = Result.Ok(42);
        Result<int> fail = Result.Fail<int>("message", "CODE");
        const string okMessage = "Expected Result<Int32> to be Ok with a value matching the predicate (reason), but the value was '42'.";
        const string failMessage = "Expected Result<Int32> to be Fail with an error matching the predicate (reason), but the error was [CODE] message.";
        Assert.Equal(okMessage, Assert.Throws<RailwayAssertionException>(() => ok.ShouldBeOkWhere(_ => false, "reason")).Message);
        Assert.Equal(failMessage, Assert.Throws<RailwayAssertionException>(() => fail.ShouldBeFailWhere(_ => false, "reason")).Message);
        Assert.Equal(okMessage, (await Assert.ThrowsAsync<RailwayAssertionException>(() => Task.FromResult(ok).ShouldBeOkWhereAsync(_ => false, "reason"))).Message);
        Assert.Equal(failMessage, (await Assert.ThrowsAsync<RailwayAssertionException>(() => Task.FromResult(fail).ShouldBeFailWhereAsync(_ => false, "reason"))).Message);
        InvalidOperationException failure = new("predicate failed");
        Assert.Same(failure, Assert.Throws<InvalidOperationException>(() => ok.ShouldBeOkWhere(_ => throw failure)));
        Assert.Same(failure, Assert.Throws<InvalidOperationException>(() => fail.ShouldBeFailWhere(_ => throw failure)));
        Assert.Same(failure, await Assert.ThrowsAsync<InvalidOperationException>(() => Task.FromResult(ok).ShouldBeOkWhereAsync(_ => throw failure)));
        Assert.Same(failure, await Assert.ThrowsAsync<InvalidOperationException>(() => Task.FromResult(fail).ShouldBeFailWhereAsync(_ => throw failure)));
    }

    [Fact]
    public void ExceptionAssertion_AcceptsDerivedTypesAndReportsMismatch()
    {
        InvalidOperationException failure = new("failure");
        Error error = Error.FromException(failure);
        Assert.Same(failure, error.ShouldHaveException<Exception>());
        Assert.Equal("Expected Error to have an exception of type 'ArgumentException', but it had InvalidOperationException ([InvalidOperationException] failure).",
            Assert.Throws<RailwayAssertionException>(() => error.ShouldHaveException<ArgumentException>()).Message);
    }

    [Fact]
    public async Task MessageMismatch_HasSameDiagnosticSynchronouslyAndAsynchronously()
    {
        Result<int> result = Result.Fail<int>("message", "CODE");
        const string expected = "Expected Result<Int32> to be Fail with message 'other', but the message was 'message' ([CODE] message).";
        Assert.Equal(expected, Assert.Throws<RailwayAssertionException>(() => result.ShouldBeFail("CODE", "other")).Message);
        Assert.Equal(expected, (await Assert.ThrowsAsync<RailwayAssertionException>(() => Task.FromResult(result).ShouldBeFailAsync("CODE", "other"))).Message);
    }
}

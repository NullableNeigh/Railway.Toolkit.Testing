namespace Railway.Toolkit.Testing;

/// <summary>
/// Async counterparts of <see cref="ResultAssertionExtensions"/> for awaiting a pipeline and asserting on its outcome in one step.
/// </summary>
public static class ResultAssertionAsyncExtensions
{
    public static async Task<T> ShouldBeOkAsync<T>(this Task<Result<T>> resultTask)
    {
        Result<T> result = await resultTask.ConfigureAwait(false);
        return result.ShouldBeOk();
    }

    public static async Task<T> ShouldBeOkAsync<T>(this Task<Result<T>> resultTask, T expectedValue, IEqualityComparer<T>? comparer = null)
    {
        Result<T> result = await resultTask.ConfigureAwait(false);
        return result.ShouldBeOk(expectedValue, comparer);
    }

    public static async Task<T> ShouldBeOkWhereAsync<T>(this Task<Result<T>> resultTask, Func<T, bool> predicate, string? because = null)
    {
        Result<T> result = await resultTask.ConfigureAwait(false);
        return result.ShouldBeOkWhere(predicate, because);
    }

    public static async Task<Error> ShouldBeFailAsync<T>(this Task<Result<T>> resultTask)
    {
        Result<T> result = await resultTask.ConfigureAwait(false);
        return result.ShouldBeFail();
    }

    public static async Task<Error> ShouldBeFailAsync<T>(this Task<Result<T>> resultTask, string expectedCode)
    {
        Result<T> result = await resultTask.ConfigureAwait(false);
        return result.ShouldBeFail(expectedCode);
    }

    public static async Task<Error> ShouldBeFailAsync<T>(this Task<Result<T>> resultTask, string expectedCode, string expectedMessage)
    {
        Result<T> result = await resultTask.ConfigureAwait(false);
        return result.ShouldBeFail(expectedCode, expectedMessage);
    }

    public static async Task<Error> ShouldBeFailWhereAsync<T>(this Task<Result<T>> resultTask, Func<Error, bool> predicate, string? because = null)
    {
        Result<T> result = await resultTask.ConfigureAwait(false);
        return result.ShouldBeFailWhere(predicate, because);
    }
}

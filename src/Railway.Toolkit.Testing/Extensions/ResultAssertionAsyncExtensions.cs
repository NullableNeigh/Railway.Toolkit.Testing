namespace Railway.Toolkit.Testing;

/// <summary>
/// Async counterparts of <see cref="ResultAssertionExtensions"/> for awaiting a pipeline and asserting on its outcome in one step.
/// </summary>
public static class ResultAssertionAsyncExtensions
{
    /// <summary>Awaits the result and asserts that the result is Ok.</summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <param name="resultTask">The task producing the result to inspect.</param>
    /// <returns>A task whose result is the original success value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="resultTask"/> or the result it produces is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The result does not satisfy the assertion.</exception>
    /// <remarks>Argument and assertion exceptions are delivered through the returned task. Source task faults and cancellation propagate unchanged; other arguments are checked after the source task completes successfully.</remarks>
    public static async Task<T> ShouldBeOkAsync<T>(this Task<Result<T>> resultTask)
    {
        ArgumentNullException.ThrowIfNull(resultTask);

        Result<T> result = await resultTask.ConfigureAwait(false);
        return result.ShouldBeOk();
    }

    /// <summary>Awaits the result and asserts that the result is Ok and its value equals the expected value.</summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <param name="resultTask">The task producing the result to inspect.</param>
    /// <param name="expectedValue">The expected success value.</param>
    /// <param name="comparer">The equality comparer, or <see langword="null"/> to use <see cref="EqualityComparer{T}.Default"/>.</param>
    /// <returns>A task whose result is the original success value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="resultTask"/> or the result it produces is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The result does not satisfy the assertion.</exception>
    /// <remarks>Comparer exceptions propagate unchanged. Argument and assertion exceptions are delivered through the returned task. Source task faults and cancellation propagate unchanged; other arguments are checked after the source task completes successfully.</remarks>
    public static async Task<T> ShouldBeOkAsync<T>(this Task<Result<T>> resultTask, T expectedValue, IEqualityComparer<T>? comparer = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);

        Result<T> result = await resultTask.ConfigureAwait(false);
        return result.ShouldBeOk(expectedValue, comparer);
    }

    /// <summary>Awaits the result and asserts that the result is Ok and its value matches the predicate.</summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <param name="resultTask">The task producing the result to inspect.</param>
    /// <param name="predicate">The condition the value must satisfy.</param>
    /// <param name="because">An optional reason included in the assertion failure message.</param>
    /// <returns>A task whose result is the original success value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="resultTask"/>, the result it produces or <paramref name="predicate"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The result does not satisfy the assertion.</exception>
    /// <remarks>The predicate runs only for the expected result state. Predicate exceptions propagate unchanged. Argument and assertion exceptions are delivered through the returned task. Source task faults and cancellation propagate unchanged; other arguments are checked after the source task completes successfully.</remarks>
    public static async Task<T> ShouldBeOkWhereAsync<T>(this Task<Result<T>> resultTask, Func<T, bool> predicate, string? because = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);

        Result<T> result = await resultTask.ConfigureAwait(false);
        return result.ShouldBeOkWhere(predicate, because);
    }

    /// <summary>Awaits the result and asserts that the result is Fail.</summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <param name="resultTask">The task producing the result to inspect.</param>
    /// <returns>A task whose result is the original error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="resultTask"/> or the result it produces is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The result does not satisfy the assertion.</exception>
    /// <remarks>Argument and assertion exceptions are delivered through the returned task. Source task faults and cancellation propagate unchanged; other arguments are checked after the source task completes successfully.</remarks>
    public static async Task<Error> ShouldBeFailAsync<T>(this Task<Result<T>> resultTask)
    {
        ArgumentNullException.ThrowIfNull(resultTask);

        Result<T> result = await resultTask.ConfigureAwait(false);
        return result.ShouldBeFail();
    }

    /// <summary>Awaits the result and asserts that the result is Fail with the expected error code.</summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <param name="resultTask">The task producing the result to inspect.</param>
    /// <param name="expectedCode">The expected error code, compared using ordinal, case-sensitive equality.</param>
    /// <returns>A task whose result is the original error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="resultTask"/>, the result it produces or <paramref name="expectedCode"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The result does not satisfy the assertion.</exception>
    /// <remarks>Argument and assertion exceptions are delivered through the returned task. Source task faults and cancellation propagate unchanged; other arguments are checked after the source task completes successfully.</remarks>
    public static async Task<Error> ShouldBeFailAsync<T>(this Task<Result<T>> resultTask, string expectedCode)
    {
        ArgumentNullException.ThrowIfNull(resultTask);

        Result<T> result = await resultTask.ConfigureAwait(false);
        return result.ShouldBeFail(expectedCode);
    }

    /// <summary>Awaits the result and asserts that the result is Fail with the expected error code and message.</summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <param name="resultTask">The task producing the result to inspect.</param>
    /// <param name="expectedCode">The expected error code, compared using ordinal, case-sensitive equality.</param>
    /// <param name="expectedMessage">The expected error message, compared using ordinal, case-sensitive equality.</param>
    /// <returns>A task whose result is the original error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="resultTask"/>, the result it produces, <paramref name="expectedCode"/> or <paramref name="expectedMessage"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The result does not satisfy the assertion.</exception>
    /// <remarks>Argument and assertion exceptions are delivered through the returned task. Source task faults and cancellation propagate unchanged; other arguments are checked after the source task completes successfully.</remarks>
    public static async Task<Error> ShouldBeFailAsync<T>(this Task<Result<T>> resultTask, string expectedCode, string expectedMessage)
    {
        ArgumentNullException.ThrowIfNull(resultTask);

        Result<T> result = await resultTask.ConfigureAwait(false);
        return result.ShouldBeFail(expectedCode, expectedMessage);
    }

    /// <summary>Awaits the result and asserts that the result is Fail and its error matches the predicate.</summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <param name="resultTask">The task producing the result to inspect.</param>
    /// <param name="predicate">The condition the error must satisfy.</param>
    /// <param name="because">An optional reason included in the assertion failure message.</param>
    /// <returns>A task whose result is the original error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="resultTask"/>, the result it produces or <paramref name="predicate"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The result does not satisfy the assertion.</exception>
    /// <remarks>The predicate runs only for the expected result state. Predicate exceptions propagate unchanged. Argument and assertion exceptions are delivered through the returned task. Source task faults and cancellation propagate unchanged; other arguments are checked after the source task completes successfully.</remarks>
    public static async Task<Error> ShouldBeFailWhereAsync<T>(this Task<Result<T>> resultTask, Func<Error, bool> predicate, string? because = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);

        Result<T> result = await resultTask.ConfigureAwait(false);
        return result.ShouldBeFailWhere(predicate, because);
    }
}

namespace Railway.Toolkit.Testing;

/// <summary>
/// Assertion extensions for verifying the shape and content of <see cref="Result{T}"/> values in tests.
/// Failures throw <see cref="RailwayAssertionException"/> so they work with any test runner.
/// </summary>
public static class ResultAssertionExtensions
{
    /// <summary>
    /// Asserts that the result is Ok and returns its value for further assertions.
    /// </summary>
    public static T ShouldBeOk<T>(this Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result is Result<T>.Ok ok)
        {
            return ok.Value;
        }

        Result<T>.Fail fail = (Result<T>.Fail)result;
        throw new RailwayAssertionException(
            $"Expected Result<{typeof(T).Name}> to be Ok, but it was Fail with Error {fail.Error}.");
    }

    /// <summary>
    /// Asserts that the result is Ok and its value equals <paramref name="expectedValue"/>.
    /// </summary>
    public static T ShouldBeOk<T>(this Result<T> result, T expectedValue, IEqualityComparer<T>? comparer = null)
    {
        T value = result.ShouldBeOk();
        comparer ??= EqualityComparer<T>.Default;

        if (!comparer.Equals(value, expectedValue))
        {
            throw new RailwayAssertionException(
                $"Expected Result<{typeof(T).Name}> to be Ok with value '{expectedValue}', but the value was '{value}'.");
        }

        return value;
    }

    /// <summary>
    /// Asserts that the result is Ok and its value matches <paramref name="predicate"/>.
    /// </summary>
    public static T ShouldBeOkWhere<T>(this Result<T> result, Func<T, bool> predicate, string? because = null)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        T value = result.ShouldBeOk();

        if (!predicate(value))
        {
            string reason = because is null ? string.Empty : $" ({because})";
            throw new RailwayAssertionException(
                $"Expected Result<{typeof(T).Name}> to be Ok with a value matching the predicate{reason}, but the value was '{value}'.");
        }

        return value;
    }

    /// <summary>
    /// Asserts that the result is Fail and returns its <see cref="Error"/> for further assertions.
    /// </summary>
    public static Error ShouldBeFail<T>(this Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result is Result<T>.Fail fail)
        {
            return fail.Error;
        }

        Result<T>.Ok ok = (Result<T>.Ok)result;
        throw new RailwayAssertionException(
            $"Expected Result<{typeof(T).Name}> to be Fail, but it was Ok with value '{ok.Value}'.");
    }

    /// <summary>
    /// Asserts that the result is Fail with the given error code.
    /// </summary>
    public static Error ShouldBeFail<T>(this Result<T> result, string expectedCode)
    {
        ArgumentNullException.ThrowIfNull(expectedCode);

        Error error = result.ShouldBeFail();

        if (error.Code != expectedCode)
        {
            throw new RailwayAssertionException(
                $"Expected Result<{typeof(T).Name}> to be Fail with code '{expectedCode}', but the code was '{error.Code}' ({error}).");
        }

        return error;
    }

    /// <summary>
    /// Asserts that the result is Fail with the given error code and message.
    /// </summary>
    public static Error ShouldBeFail<T>(this Result<T> result, string expectedCode, string expectedMessage)
    {
        Error error = result.ShouldBeFail(expectedCode);

        if (error.Message != expectedMessage)
        {
            throw new RailwayAssertionException(
                $"Expected Result<{typeof(T).Name}> to be Fail with message '{expectedMessage}', but the message was '{error.Message}' ({error}).");
        }

        return error;
    }

    /// <summary>
    /// Asserts that the result is Fail and its error matches <paramref name="predicate"/>.
    /// </summary>
    public static Error ShouldBeFailWhere<T>(this Result<T> result, Func<Error, bool> predicate, string? because = null)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        Error error = result.ShouldBeFail();

        if (!predicate(error))
        {
            string reason = because is null ? string.Empty : $" ({because})";
            throw new RailwayAssertionException(
                $"Expected Result<{typeof(T).Name}> to be Fail with an error matching the predicate{reason}, but the error was {error}.");
        }

        return error;
    }
}

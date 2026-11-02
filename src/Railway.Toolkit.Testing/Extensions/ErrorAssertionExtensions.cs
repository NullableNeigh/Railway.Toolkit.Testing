namespace Railway.Toolkit.Testing;

/// <summary>
/// Assertion extensions for inspecting the structured content of an <see cref="Error"/> in tests.
/// </summary>
public static class ErrorAssertionExtensions
{
    /// <summary>Asserts that the error has the expected code.</summary>
    /// <param name="error">The error to inspect.</param>
    /// <param name="expectedCode">The expected code, compared using ordinal, case-sensitive equality.</param>
    /// <returns>The original error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="error"/> or <paramref name="expectedCode"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The error code differs from the expected code.</exception>
    public static Error ShouldHaveCode(this Error error, string expectedCode)
    {
        ArgumentNullException.ThrowIfNull(error);
        ArgumentNullException.ThrowIfNull(expectedCode);

        if (error.Code != expectedCode)
        {
            throw new RailwayAssertionException(
                $"Expected Error to have code '{expectedCode}', but it was '{error.Code}' ({error}).");
        }

        return error;
    }

    /// <summary>Asserts that the error has the expected message.</summary>
    /// <param name="error">The error to inspect.</param>
    /// <param name="expectedMessage">The expected message, compared using ordinal, case-sensitive equality.</param>
    /// <returns>The original error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="error"/> or <paramref name="expectedMessage"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The error message differs from the expected message.</exception>
    public static Error ShouldHaveMessage(this Error error, string expectedMessage)
    {
        ArgumentNullException.ThrowIfNull(error);
        ArgumentNullException.ThrowIfNull(expectedMessage);

        if (error.Message != expectedMessage)
        {
            throw new RailwayAssertionException(
                $"Expected Error to have message '{expectedMessage}', but it was '{error.Message}' ({error}).");
        }

        return error;
    }

    /// <summary>Asserts that a field in <see cref="Error.Details"/> contains the expected message.</summary>
    /// <param name="error">The error to inspect.</param>
    /// <param name="fieldName">The field key, looked up using the details dictionary's comparer.</param>
    /// <param name="expectedMessage">The expected field message, compared using ordinal, case-sensitive equality.</param>
    /// <returns>The original error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="error"/>, <paramref name="fieldName"/>, or <paramref name="expectedMessage"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The field is absent or does not contain the expected message.</exception>
    public static Error ShouldHaveDetail(this Error error, string fieldName, string expectedMessage)
    {
        ArgumentNullException.ThrowIfNull(error);
        ArgumentNullException.ThrowIfNull(fieldName);
        ArgumentNullException.ThrowIfNull(expectedMessage);

        if (error.Details is null || !error.Details.TryGetValue(fieldName, out string[]? messages))
        {
            throw new RailwayAssertionException(
                $"Expected Error to have Details for field '{fieldName}', but no details were present for that field ({error}).");
        }

        if (Array.IndexOf(messages, expectedMessage) < 0)
        {
            throw new RailwayAssertionException(
                $"Expected Error Details['{fieldName}'] to contain '{expectedMessage}', but it contained [{string.Join(", ", messages)}].");
        }

        return error;
    }

    /// <summary>Asserts the number of immediate errors in <see cref="Error.InnerErrors"/>.</summary>
    /// <param name="error">The error to inspect.</param>
    /// <param name="expectedCount">The expected number of immediate inner errors.</param>
    /// <returns>The original error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The inner error count differs from the expected count.</exception>
    /// <remarks>An absent inner error collection counts as zero. Nested errors are not counted recursively.</remarks>
    public static Error ShouldHaveInnerErrorCount(this Error error, int expectedCount)
    {
        ArgumentNullException.ThrowIfNull(error);

        int actualCount = error.InnerErrors?.Count ?? 0;

        if (actualCount != expectedCount)
        {
            throw new RailwayAssertionException(
                $"Expected Error to have {expectedCount} inner error(s), but it had {actualCount} ({error}).");
        }

        return error;
    }

    /// <summary>Asserts that the error wraps an exception assignable to the requested type.</summary>
    /// <typeparam name="TException">The expected exception type; derived types are accepted.</typeparam>
    /// <param name="error">The error to inspect.</param>
    /// <returns>The original wrapped exception.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="error"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">There is no wrapped exception, or its type is not assignable to <typeparamref name="TException"/>.</exception>
    public static TException ShouldHaveException<TException>(this Error error)
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(error);

        if (error.Exception is TException exception)
        {
            return exception;
        }

        string actual = error.Exception is null ? "no exception" : error.Exception.GetType().Name;
        throw new RailwayAssertionException(
            $"Expected Error to have an exception of type '{typeof(TException).Name}', but it had {actual} ({error}).");
    }
}

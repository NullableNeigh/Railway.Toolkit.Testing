namespace Railway.Toolkit.Testing;

/// <summary>
/// Assertion extensions for inspecting the structured content of an <see cref="Error"/> in tests.
/// </summary>
public static class ErrorAssertionExtensions
{
    /// <summary>
    /// Asserts that the error has the given code.
    /// </summary>
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

    /// <summary>
    /// Asserts that the error has the given message.
    /// </summary>
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

    /// <summary>
    /// Asserts that the error's field-level <see cref="Error.Details"/> contains <paramref name="expectedMessage"/> for <paramref name="fieldName"/>.
    /// </summary>
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

    /// <summary>
    /// Asserts that the error's <see cref="Error.InnerErrors"/> contains the given number of errors.
    /// </summary>
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

    /// <summary>
    /// Asserts that the error wraps an exception of type <typeparamref name="TException"/> and returns it.
    /// </summary>
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

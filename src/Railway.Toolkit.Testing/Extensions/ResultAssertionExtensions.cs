namespace Railway.Toolkit.Testing;

/// <summary>
/// Assertion extensions for verifying the shape and content of <see cref="Result{T}"/> values in tests.
/// Failures throw <see cref="RailwayAssertionException"/> so they work with any test runner.
/// </summary>
public static class ResultAssertionExtensions
{
    /// <summary>Asserts that the result is Ok.</summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <param name="result">The result to inspect.</param>
    /// <returns>The original success value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="result"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The result does not satisfy the assertion.</exception>
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

    /// <summary>Asserts that the result is Ok and its value equals the expected value.</summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <param name="result">The result to inspect.</param>
    /// <param name="expectedValue">The expected success value.</param>
    /// <param name="comparer">The equality comparer, or <see langword="null"/> to use <see cref="EqualityComparer{T}.Default"/>.</param>
    /// <returns>The original success value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="result"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The result does not satisfy the assertion.</exception>
    /// <remarks>Comparer exceptions propagate unchanged.</remarks>
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

    /// <summary>Asserts that the result is Ok and its value matches the predicate.</summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <param name="result">The result to inspect.</param>
    /// <param name="predicate">The condition the value must satisfy.</param>
    /// <param name="because">An optional reason included in the assertion failure message.</param>
    /// <returns>The original success value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="result"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The result does not satisfy the assertion.</exception>
    /// <remarks>The predicate runs only for the expected result state. Predicate exceptions propagate unchanged.</remarks>
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

    /// <summary>Asserts that the result is Fail.</summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <param name="result">The result to inspect.</param>
    /// <returns>The original error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="result"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The result does not satisfy the assertion.</exception>
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

    /// <summary>Asserts that the result is Fail with the expected error code.</summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <param name="result">The result to inspect.</param>
    /// <param name="expectedCode">The expected error code, compared using ordinal, case-sensitive equality.</param>
    /// <returns>The original error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="result"/> or <paramref name="expectedCode"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The result does not satisfy the assertion.</exception>
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

    /// <summary>Asserts that the result is Fail with the expected error code and message.</summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <param name="result">The result to inspect.</param>
    /// <param name="expectedCode">The expected error code, compared using ordinal, case-sensitive equality.</param>
    /// <param name="expectedMessage">The expected error message, compared using ordinal, case-sensitive equality.</param>
    /// <returns>The original error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="result"/>, <paramref name="expectedCode"/> or <paramref name="expectedMessage"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The result does not satisfy the assertion.</exception>
    public static Error ShouldBeFail<T>(this Result<T> result, string expectedCode, string expectedMessage)
    {
        ArgumentNullException.ThrowIfNull(expectedMessage);

        Error error = result.ShouldBeFail(expectedCode);

        if (error.Message != expectedMessage)
        {
            throw new RailwayAssertionException(
                $"Expected Result<{typeof(T).Name}> to be Fail with message '{expectedMessage}', but the message was '{error.Message}' ({error}).");
        }

        return error;
    }

    /// <summary>Asserts that the result is Fail and its error matches the predicate.</summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <param name="result">The result to inspect.</param>
    /// <param name="predicate">The condition the error must satisfy.</param>
    /// <param name="because">An optional reason included in the assertion failure message.</param>
    /// <returns>The original error.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="result"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
    /// <exception cref="RailwayAssertionException">The result does not satisfy the assertion.</exception>
    /// <remarks>The predicate runs only for the expected result state. Predicate exceptions propagate unchanged.</remarks>
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

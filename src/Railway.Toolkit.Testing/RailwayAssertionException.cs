namespace Railway.Toolkit.Testing;

/// <summary>
/// Thrown by Railway.Toolkit.Testing assertions when a result or error does not satisfy an assertion.
/// Framework-agnostic: works with xUnit, NUnit, MSTest, or any runner that fails a test on an unhandled exception.
/// </summary>
public sealed class RailwayAssertionException : Exception
{
    /// <summary>Initializes an assertion failure with a diagnostic message.</summary>
    /// <param name="message">The message describing the unmet expectation.</param>
    public RailwayAssertionException(string message) : base(message)
    {
    }

    /// <summary>Initializes an assertion failure with a diagnostic message and its underlying cause.</summary>
    /// <param name="message">The message describing the unmet expectation.</param>
    /// <param name="innerException">The exception that caused the assertion failure.</param>
    public RailwayAssertionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

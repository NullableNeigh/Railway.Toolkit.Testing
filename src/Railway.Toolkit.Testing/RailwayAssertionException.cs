namespace Railway.Toolkit.Testing;

/// <summary>
/// Thrown by Railway.Toolkit.Testing assertions when a Result does not match the expected shape or value.
/// Framework-agnostic: works with xUnit, NUnit, MSTest, or any runner that fails a test on an unhandled exception.
/// </summary>
public sealed class RailwayAssertionException : Exception
{
    public RailwayAssertionException(string message) : base(message)
    {
    }

    public RailwayAssertionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

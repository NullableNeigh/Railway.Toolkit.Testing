using Railway.Toolkit.Testing;

namespace Railway.Toolkit.Testing.Tests;

public class ErrorAssertionExtensionsTests
{
    [Fact]
    public void ShouldHaveCode_ReturnsError_WhenCodeMatches()
    {
        Error error = Error.Create("Boom", "TEST_ERROR");

        Error result = error.ShouldHaveCode("TEST_ERROR");

        Assert.Same(error, result);
    }

    [Fact]
    public void ShouldHaveCode_Throws_WhenCodeDiffers()
    {
        Error error = Error.Create("Boom", "TEST_ERROR");

        Assert.Throws<RailwayAssertionException>(() => error.ShouldHaveCode("OTHER_CODE"));
    }

    [Fact]
    public void ShouldHaveMessage_Throws_WhenMessageDiffers()
    {
        Error error = Error.Create("Boom", "TEST_ERROR");

        Assert.Throws<RailwayAssertionException>(() => error.ShouldHaveMessage("Different message"));
    }

    [Fact]
    public void ShouldHaveDetail_ReturnsError_WhenFieldContainsMessage()
    {
        ErrorDetailsBuilder builder = ErrorDetailsBuilder.Create();
        builder.AddDetail("Email", "Invalid format");

        Error error = new()
        {
            Message = "Validation failed",
            Code = "Validation.Failed",
            Details = builder.Build(),
        };

        error.ShouldHaveDetail("Email", "Invalid format");
    }

    [Fact]
    public void ShouldHaveDetail_Throws_WhenFieldMissing()
    {
        Error error = Error.Create("Boom", "TEST_ERROR");

        Assert.Throws<RailwayAssertionException>(() => error.ShouldHaveDetail("Email", "Invalid format"));
    }

    [Fact]
    public void ShouldHaveDetail_Throws_WhenMessageMissingForField()
    {
        ErrorDetailsBuilder builder = ErrorDetailsBuilder.Create();
        builder.AddDetail("Email", "Invalid format");

        Error error = new()
        {
            Message = "Validation failed",
            Code = "Validation.Failed",
            Details = builder.Build(),
        };

        Assert.Throws<RailwayAssertionException>(() => error.ShouldHaveDetail("Email", "Already registered"));
    }

    [Fact]
    public void ShouldHaveInnerErrorCount_ReturnsError_WhenCountMatches()
    {
        Error error = Error.Aggregate(
            new[] { Error.Create("First", "A"), Error.Create("Second", "B") });

        error.ShouldHaveInnerErrorCount(2);
    }

    [Fact]
    public void ShouldHaveInnerErrorCount_Throws_WhenCountDiffers()
    {
        Error error = Error.Create("Boom", "TEST_ERROR");

        Assert.Throws<RailwayAssertionException>(() => error.ShouldHaveInnerErrorCount(1));
    }

    [Fact]
    public void ShouldHaveException_ReturnsException_WhenTypeMatches()
    {
        InvalidOperationException exception = new("bad state");
        Error error = Error.FromException(exception);

        InvalidOperationException result = error.ShouldHaveException<InvalidOperationException>();

        Assert.Same(exception, result);
    }

    [Fact]
    public void ShouldHaveException_Throws_WhenNoExceptionPresent()
    {
        Error error = Error.Create("Boom", "TEST_ERROR");

        Assert.Throws<RailwayAssertionException>(() => error.ShouldHaveException<InvalidOperationException>());
    }
}

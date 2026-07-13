using Railway.Toolkit.Testing;

namespace Railway.Toolkit.Testing.Tests;

public class ResultAssertionExtensionsTests
{
    [Fact]
    public void ShouldBeOk_ReturnsValue_WhenResultIsOk()
    {
        Result<int> result = Result.Ok(42);

        int value = result.ShouldBeOk();

        Assert.Equal(42, value);
    }

    [Fact]
    public void ShouldBeOk_Throws_WhenResultIsFail()
    {
        Result<int> result = Result.Fail<int>("Boom", "TEST_ERROR");

        RailwayAssertionException exception = Assert.Throws<RailwayAssertionException>(() => result.ShouldBeOk());

        Assert.Contains("TEST_ERROR", exception.Message);
    }

    [Fact]
    public void ShouldBeOk_WithExpectedValue_ReturnsValue_WhenValuesMatch()
    {
        Result<int> result = Result.Ok(42);

        int value = result.ShouldBeOk(42);

        Assert.Equal(42, value);
    }

    [Fact]
    public void ShouldBeOk_WithExpectedValue_Throws_WhenValuesDiffer()
    {
        Result<int> result = Result.Ok(42);

        Assert.Throws<RailwayAssertionException>(() => result.ShouldBeOk(1));
    }

    [Fact]
    public void ShouldBeOkWhere_ReturnsValue_WhenPredicateMatches()
    {
        Result<int> result = Result.Ok(42);

        int value = result.ShouldBeOkWhere(x => x > 0);

        Assert.Equal(42, value);
    }

    [Fact]
    public void ShouldBeOkWhere_Throws_WhenPredicateDoesNotMatch()
    {
        Result<int> result = Result.Ok(42);

        Assert.Throws<RailwayAssertionException>(() => result.ShouldBeOkWhere(x => x < 0));
    }

    [Fact]
    public void ShouldBeFail_ReturnsError_WhenResultIsFail()
    {
        Result<int> result = Result.Fail<int>("Boom", "TEST_ERROR");

        Error error = result.ShouldBeFail();

        Assert.Equal("TEST_ERROR", error.Code);
    }

    [Fact]
    public void ShouldBeFail_Throws_WhenResultIsOk()
    {
        Result<int> result = Result.Ok(42);

        RailwayAssertionException exception = Assert.Throws<RailwayAssertionException>(() => result.ShouldBeFail());

        Assert.Contains("42", exception.Message);
    }

    [Fact]
    public void ShouldBeFail_WithExpectedCode_Throws_WhenCodeDiffers()
    {
        Result<int> result = Result.Fail<int>("Boom", "TEST_ERROR");

        Assert.Throws<RailwayAssertionException>(() => result.ShouldBeFail("OTHER_CODE"));
    }

    [Fact]
    public void ShouldBeFail_WithExpectedCodeAndMessage_ReturnsError_WhenBothMatch()
    {
        Result<int> result = Result.Fail<int>("Boom", "TEST_ERROR");

        Error error = result.ShouldBeFail("TEST_ERROR", "Boom");

        Assert.Equal("Boom", error.Message);
    }

    [Fact]
    public void ShouldBeFailWhere_ReturnsError_WhenPredicateMatches()
    {
        Result<int> result = Result.Fail<int>("Boom", "TEST_ERROR");

        Error error = result.ShouldBeFailWhere(e => e.Code == "TEST_ERROR");

        Assert.Equal("TEST_ERROR", error.Code);
    }

    [Fact]
    public async Task ShouldBeOkAsync_ReturnsValue_WhenResultIsOk()
    {
        Task<Result<int>> resultTask = Task.FromResult(Result.Ok(42));

        int value = await resultTask.ShouldBeOkAsync();

        Assert.Equal(42, value);
    }

    [Fact]
    public async Task ShouldBeFailAsync_ReturnsError_WhenResultIsFail()
    {
        Task<Result<int>> resultTask = Task.FromResult(Result.Fail<int>("Boom", "TEST_ERROR"));

        Error error = await resultTask.ShouldBeFailAsync("TEST_ERROR");

        Assert.Equal("Boom", error.Message);
    }
}

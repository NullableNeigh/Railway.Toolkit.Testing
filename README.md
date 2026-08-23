# Railway.Toolkit.Testing

Assertion extensions for testing `Result<T>` pipelines built with [Railway.Toolkit](https://github.com/NullableNeigh/Railway.Toolkit).

Railway.Toolkit deliberately has no escape hatches (`IsOk`, `Unwrap()`, `GetValueOrDefault()`) on `Result<T>` itself, so this
package gives you framework-agnostic ways to assert on the outcome of a pipeline in your tests without reaching for
pattern matching every time.

## Installation

Not yet published to NuGet. Once available:

```bash
dotnet add package Railway.Toolkit.Testing
```

Until then, reference the project directly:

```bash
dotnet add reference ../Railway.Toolkit.Testing/src/Railway.Toolkit.Testing/Railway.Toolkit.Testing.csproj
```

**Versioning:** this package tracks `Railway.Toolkit`'s version exactly (e.g. core `2.3.3` requires plugin `2.3.3`) —
always pin both to the same version.

## Usage

```csharp
using Railway.Toolkit;
using Railway.Toolkit.Testing;

Result<User> result = Railway.Start(input).Bind(ValidateUser);

// Assert Ok and get the value back for further assertions
User user = result.ShouldBeOk();

// Assert Ok with an expected value
result.ShouldBeOk(expectedUser);

// Assert Ok where the value matches a predicate
result.ShouldBeOkWhere(u => u.Age >= 18, because: "underage users should have been rejected upstream");

// Assert Fail and get the Error back
Error error = result.ShouldBeFail();

// Assert Fail with a specific code, or code + message
result.ShouldBeFail("Validation.Underage");
result.ShouldBeFail("Validation.Underage", "Must be 18 or older");

// Assert Fail where the error matches a predicate
result.ShouldBeFailWhere(e => e.Code.StartsWith("Validation."));

// Assert against the Error's code, message, structured details, aggregation, or wrapped exception
error.ShouldHaveCode("Validation.Underage");
error.ShouldHaveMessage("Must be 18 or older");
error.ShouldHaveDetail("Email", "Invalid email format");
error.ShouldHaveInnerErrorCount(3);
error.ShouldHaveException<HttpRequestException>();

// Async pipelines - every assertion has a Task<Result<T>> counterpart
User user = await Railway.Start(input).BindAsync(FetchUserAsync).ShouldBeOkAsync();
await Railway.Start(input).BindAsync(FetchUserAsync).ShouldBeOkWhereAsync(u => u.IsActive);
await Railway.Start(input).BindAsync(FetchUserAsync).ShouldBeFailAsync("NotFound");
await Railway.Start(input).BindAsync(FetchUserAsync).ShouldBeFailWhereAsync(e => e.Code == "NotFound");
```

Every assertion throws `RailwayAssertionException` with a message describing the expected vs. actual state, so it works
the same way under xUnit, NUnit, MSTest, or any runner that fails a test on an unhandled exception.

## Testing

The plugin has its own test suite covering every assertion, happy path and failure path:

```bash
dotnet test
```

## License

MIT License - see [LICENSE](LICENSE) file for details.

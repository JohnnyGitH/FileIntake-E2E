namespace FileIntake.E2E.Config;

public static class TestConfig
{
    public static string? BaseUrl =>
        Environment.GetEnvironmentVariable("FILEINTAKE_BASE_URL");

    public static string GetRequiredBaseUrlOrSkip()
    {
        var url = BaseUrl;

        if (string.IsNullOrWhiteSpace(url))
        {
            NUnit.Framework.Assert.Ignore(
                "FILEINTAKE_BASE_URL is not set. Set it to run E2E tests"
            );
        }

        return url!;
    }
}
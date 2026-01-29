using Microsoft.Playwright;
using NUnit.Framework;

namespace FileIntake.E2E.Tests.Smoke;

[TestFixture]
public class HomePageSmokeTests
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IBrowserContext _context;
    private IPage _page;

    // UAT base URL (Hardcoded for now, will env-var this later)
    private static string BaseUrl =>
    Environment.GetEnvironmentVariable("FILEINTAKE_BASE_URL")
    ?? throw new InvalidOperationException(
        "FILEINTAKE_BASE_URL environment variable is not set");

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });

        _context = await _browser.NewContextAsync();
        _page = await _context.NewPageAsync();
    }

    [Test]
    public async Task HomePage_Loads_Successfully()
    {
        // Act
        var response = await _page.GotoAsync(BaseUrl);

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Ok, Is.True);

        var title = await _page.TitleAsync();
        Assert.That(title, Is.Not.Empty);
    }
}
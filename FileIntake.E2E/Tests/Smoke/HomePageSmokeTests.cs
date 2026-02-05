using FileIntake.E2E.Config;
using Microsoft.Playwright;

namespace FileIntake.E2E.Tests.Smoke;

[TestFixture]
public class HomePageSmokeTests
{
    private IPlaywright _playwright;
    private IBrowser _browser;

    // UAT base URL (Hardcoded for now, will env-var this later)
    private  string BaseUrl => TestConfig.GetRequiredBaseUrlOrSkip().TrimEnd('/');

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (_browser != null) await _browser.CloseAsync();
        _playwright?.Dispose();
    }

    private async Task<IPage> NewPageAsync()
    {
        if(_browser == null) throw new InvalidOperationException("Browser not initialized");

        var context = await _browser.NewContextAsync();

        return await context.NewPageAsync();
    }

    [Test]
    public async Task HomePage_Loads_Successfully()
    {

        // Arrange
        var page = await NewPageAsync();
        
        // Act
        var response = await page.GotoAsync(BaseUrl, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle});

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Ok, Is.True);

        var title = await page.TitleAsync();
        Assert.That(title, Is.Not.Empty);
        Assert.That(title, Does.Contain("FileIntake"));
    }

    [Test]
    public async Task PrivacyPageLoads()
    {
        // Arrange
        var page = await NewPageAsync();

        // Act
        await page.GotoAsync($"{BaseUrl}/Home/Privacy", new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle});

        // Assert
        var body = await page.TextContentAsync("body");
        Assert.That(body, Is.Not.Null);
        Assert.That(body, Does.Contain("Privacy").Or.Contain("privacy"));
    }

    [Test]
    public async Task FileIntakePage_RequiresLogin_RedirectToLoginPage()
    {
        // Arrange
        var page = await NewPageAsync();

        var response = await page.GotoAsync($"{BaseUrl}/FileIntake", new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded});

        Assert.That(response, Is.Not.Null);

        // Confirm Redirect Happened
        Assert.That(page.Url, Does.Contain("/Identity/Account/Login"));
        // Confirm UI is correct
        await page.Locator("input[name='Input.Email']").WaitForAsync();
        await page.Locator("input[name='Input.Password']").WaitForAsync();
    }

    // [Test] NEED TO FIGURE OUT LOGIN SETUP TO GET HERE
    // public async Task FileIntakePageLoads()
    // {
    //     // Arrange
    //     var page = await NewPageAsync();

    //     // Act
    //     await page.GotoAsync($"{BaseUrl}/FileIntake", new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle});

    //     // Assert
    //     var fileInputs = await page.Locator("input[type='file']").CountAsync();
    //     Assert.That(fileInputs, Is.GreaterThanOrEqualTo(1));
    // }
}
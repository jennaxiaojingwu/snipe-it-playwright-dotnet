using Microsoft.Playwright;
using SnipeItAutomation.Configuration;

namespace SnipeItAutomation.Pages;

public class LoginPage
{
    private readonly IPage _page;

    public LoginPage(IPage page)
    {
        _page = page;
    }

    // Navigate to the login page
    public async Task NavigateAsync()
    {
        await _page.GotoAsync(
            $"{TestConfig.BaseUrl}/login");
    }

    // Login with provided credentials
    public async Task LoginAsync(
        string username,
        string password)
    {
        await _page.Locator("#username")
            .FillAsync(username);

        await _page.Locator("#password-field")
            .FillAsync(password);

        await _page.GetByRole(
            AriaRole.Button,
            new() { Name = "Login" })
            .ClickAsync();
    }
}

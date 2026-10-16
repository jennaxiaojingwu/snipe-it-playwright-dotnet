using Microsoft.Playwright;
using System.Text.RegularExpressions;

namespace SnipeItAutomation.Pages;

public class CreateAssetPage
{
    private readonly IPage _page;

    public CreateAssetPage(IPage page)
    {
        _page = page;
    }

    // Asset Tag
    private ILocator AssetTagInput =>
        _page.Locator("#asset_tag");

    // Model
    private ILocator ModelDropdown =>
        _page.Locator("#select2-model_select_id-container");

    private ILocator ModelOptions =>
        _page.Locator("#select2-model_select_id-results");

    // Status
    private ILocator StatusDropdown =>
        _page.Locator("#select2-status_select_id-container");

    private ILocator StatusOptions =>
        _page.Locator("#select2-status_select_id-results");

    // Checkout User
    private ILocator UserDropdown =>
        _page.Locator("#select2-assigned_user_select-container");

    private ILocator UserOptions =>
        _page.Locator("#select2-assigned_user_select-results");

    // Save
    private ILocator SaveButton =>
        _page.Locator("button[name='submit']");

    // Expose elements for assertions
    public ILocator SelectedModel => ModelDropdown;
    public ILocator SelectedStatus => StatusDropdown;
    public ILocator SelectedUser => UserDropdown;

    // Get the auto-generated Asset Tag
    public async Task<string> GetAssetTagAsync()
    {
        return (await AssetTagInput.InputValueAsync()).Trim();
    }

    // Select Model
    public async Task SelectModelAsync(string model)
    {
        await ModelDropdown.ClickAsync();

        await ModelOptions
            .GetByRole(AriaRole.Option)
            .Filter(new() { HasText = model })
            .ClickAsync();
    }

    // Select Status
    public async Task SelectStatusAsync(string status)
    {
        await StatusDropdown.ClickAsync();

        await StatusOptions
            .GetByRole(AriaRole.Option, new()
            {
                Name = status,
                Exact = true
            })
            .ClickAsync();
    }

    // Select a random available user
    public async Task<string> SelectRandomUserAsync()
    {
        await UserDropdown.ClickAsync();

        // Filter valid user options
        var users = UserOptions
            .GetByRole(AriaRole.Option)
            .Filter(new()
            {
                HasTextRegex = new Regex(@"#\d+")
            });

        // Wait for user options to load
        await users.First.WaitForAsync();

        int count = await users.CountAsync();

        if (count == 0)
        {
            throw new Exception(
                "No users available for checkout.");
        }

        // Pick a random user
        int randomIndex = Random.Shared.Next(count);

        var selectedOption = users.Nth(randomIndex);

        string userName =
            (await selectedOption.InnerTextAsync()).Trim();

        await selectedOption.ClickAsync();

        return userName;
    }

    // Save Asset
    public async Task SaveAsync()
    {
        await SaveButton.ClickAsync();
    }
}

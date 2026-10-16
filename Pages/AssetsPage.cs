using Microsoft.Playwright;
using SnipeItAutomation.Configuration;

namespace SnipeItAutomation.Pages;

public class AssetsPage
{
    private readonly IPage _page;

    public AssetsPage(IPage page)
    {
        _page = page;
    }

    // Locators
    private ILocator SearchInput =>
        _page.Locator(
            "div[name='assets'] input[aria-label='Search']");

    private ILocator AssetRows =>
        _page.Locator(
            "div[name='assets'] table tbody tr");

    // Navigate to Assets List
    public async Task NavigateAsync()
    {
        await _page.GotoAsync(
            $"{TestConfig.BaseUrl}/hardware");
    }

    // Search for an asset by Asset Tag
    public async Task SearchAssetAsync(string assetTag)
    {
        await SearchInput.FillAsync(assetTag);
        await SearchInput.PressAsync("Enter");
    }

    // Find the asset row by Asset Tag
    public ILocator GetAssetRow(string assetTag)
    {
        return AssetRows.Filter(new()
        {
            HasText = assetTag
        });
    }

    // Open Asset Details from the list
    public async Task OpenAssetAsync(string assetTag)
    {
        var assetRow = GetAssetRow(assetTag);

        await assetRow
            .GetByRole(AriaRole.Link, new()
            {
                Name = assetTag,
                Exact = true
            })
            .ClickAsync();
    }
}

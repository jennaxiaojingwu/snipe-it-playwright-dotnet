using Microsoft.Playwright;

namespace SnipeItAutomation.Pages;

public class AssetDetailsPage
{
    private readonly IPage _page;

    public AssetDetailsPage(IPage page)
    {
        _page = page;
    }

    // Locators - Details
    private ILocator DetailsPane =>
        _page.Locator("#details");

    // Status and assigned user summary
    public ILocator StatusSummary =>
        DetailsPane.Locator(".well").First;

    // Locators - History
    private ILocator HistoryTab =>
        _page.Locator("a[href='#history']");

    private ILocator HistoryPane =>
        _page.Locator("#history");

    private ILocator HistoryRows =>
        HistoryPane.Locator("table tbody tr");

    // Asset Tag
    public ILocator AssetTagValue(string assetTag)
    {
        return DetailsPane.GetByText(
            assetTag,
            new() { Exact = true });
    }

    // Asset Model
    public ILocator ModelValue(string model)
    {
        return _page.GetByRole(
            AriaRole.Link,
            new()
            {
                Name = model,
                Exact = true
            });
    }

    // Open History tab
    public async Task OpenHistoryAsync()
    {
        await HistoryTab.ClickAsync();
    }

    // Find a history row by asset tag and action
    private ILocator GetHistoryRow(string assetTag, string action)
    {
        return HistoryRows
            .Filter(new()
            {
                HasText = assetTag
            })
            .Filter(new()
            {
                Has = _page.Locator("td:nth-child(4)")
                    .GetByText(action, new()
                    {
                        Exact = true
                    })
            });
    }

    // Find the asset creation history record
    public ILocator GetCreationHistoryRow(string assetTag)
    {
        return GetHistoryRow(assetTag, "create new");
    }

    // Find the asset checkout history record
    public ILocator GetCheckoutHistoryRow(string assetTag)
    {
        return GetHistoryRow(assetTag, "checkout");
    }
}

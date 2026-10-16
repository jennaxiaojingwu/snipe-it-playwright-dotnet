using Microsoft.Playwright;

namespace SnipeItAutomation.Components;

public class HeaderComponent
{
    private readonly IPage _page;

    public HeaderComponent(IPage page)
    {
        _page = page;
    }

    public async Task OpenCreateAssetAsync()
    {
        await _page.GetByText("Create New", new() { Exact = true })
            .ClickAsync();

        await _page.GetByText("Asset", new() { Exact = true })
            .ClickAsync();
    }
}

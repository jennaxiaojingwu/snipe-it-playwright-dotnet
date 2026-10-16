using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using SnipeItAutomation.Components;
using SnipeItAutomation.Configuration;
using SnipeItAutomation.Pages;

namespace SnipeItAutomation.Tests;

[TestFixture]
public class AssetTests : BaseTest
{
    [Test]
    public async Task CreateCheckoutAndVerifyAsset()
    {
        // Test data
        string model = "Macbook Pro 13\"";
        string status = "Ready to Deploy";

        // Step 1: Login
        var loginPage = new LoginPage(Page);

        await loginPage.NavigateAsync();

        await loginPage.LoginAsync(
            TestConfig.Username,
            TestConfig.Password);

        // Verify successful login
        await Expect(
            Page.GetByText("Create New", new() { Exact = true })
        ).ToBeVisibleAsync();

        // Step 2: Navigate to Create Asset
        var header = new HeaderComponent(Page);

        await header.OpenCreateAssetAsync();

        var createAssetPage = new CreateAssetPage(Page);

        // Step 3: Capture auto-generated Asset Tag
        string assetTag =
            await createAssetPage.GetAssetTagAsync();

        Assert.That(assetTag, Is.Not.Empty);

        // Step 4: Select Model
        await createAssetPage.SelectModelAsync(model);

        await Expect(createAssetPage.SelectedModel)
            .ToContainTextAsync(model);

        // Step 5: Select Status
        await createAssetPage.SelectStatusAsync(status);

        await Expect(createAssetPage.SelectedStatus)
            .ToContainTextAsync(status);

        // Step 6: Select random User
        string selectedUser =
            await createAssetPage.SelectRandomUserAsync();

        await Expect(createAssetPage.SelectedUser)
            .ToContainTextAsync(selectedUser);

        // Extract the user display name
        string userDisplayName = selectedUser
            .Split(" (", StringSplitOptions.None)[0]
            .Trim();

        // Step 7: Save Asset
        string createPageUrl = Page.Url;

        await createAssetPage.SaveAsync();

        await Expect(Page).Not.ToHaveURLAsync(
            createPageUrl,
            new() { Timeout = 15000 });

        // Step 8: Navigate to Assets List
        var assetsPage = new AssetsPage(Page);

        await assetsPage.NavigateAsync();

        // Step 9: Search for the created asset
        await assetsPage.SearchAssetAsync(assetTag);

        // Step 10: Verify asset in the list
        var assetRow = assetsPage.GetAssetRow(assetTag);

        // Verify exactly one matching asset
        await Expect(assetRow)
            .ToHaveCountAsync(1);

        await Expect(assetRow)
            .ToBeVisibleAsync();

        await Expect(assetRow)
            .ToContainTextAsync(model);

        await Expect(assetRow)
            .ToContainTextAsync(status);

        await Expect(assetRow)
            .ToContainTextAsync(userDisplayName);

        // Step 11: Open Asset Details
        await assetsPage.OpenAssetAsync(assetTag);

        var detailsPage = new AssetDetailsPage(Page);

        // Step 12: Verify Asset Details
        await Expect(detailsPage.AssetTagValue(assetTag))
            .ToBeVisibleAsync();

        await Expect(detailsPage.ModelValue(model))
            .ToBeVisibleAsync();

        await Expect(detailsPage.StatusSummary)
            .ToContainTextAsync(status);

        await Expect(detailsPage.StatusSummary)
            .ToContainTextAsync(userDisplayName);

        // Step 13: Open History tab
        await detailsPage.OpenHistoryAsync();

        // Step 14: Verify creation history
        var creationRow =
            detailsPage.GetCreationHistoryRow(assetTag);

        await Expect(creationRow)
            .ToBeVisibleAsync();

        // Step 15: Verify checkout history
        var checkoutRow =
            detailsPage.GetCheckoutHistoryRow(assetTag);

        await Expect(checkoutRow)
            .ToBeVisibleAsync();

        // Verify checkout target user
        await Expect(checkoutRow)
            .ToContainTextAsync(userDisplayName);

        // Verify checkout notes
        await Expect(checkoutRow)
            .ToContainTextAsync(
                "Checked out on asset creation");

        // Test output
        TestContext.Out.WriteLine(
            $"Asset Tag: {assetTag}");

        TestContext.Out.WriteLine(
            $"Model: {model}");

        TestContext.Out.WriteLine(
            $"Status: {status}");

        TestContext.Out.WriteLine(
            $"Checked Out To: {userDisplayName}");

        TestContext.Out.WriteLine(
            "Asset creation and history verified successfully.");
    }
}

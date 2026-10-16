using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace SnipeItAutomation.Tests;

public class BaseTest : PageTest
{
    // Start Playwright tracing before each test
    [SetUp]
    public async Task StartTracingAsync()
    {
        await Context.Tracing.StartAsync(new()
        {
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
    }

    // Capture diagnostics after each test
    [TearDown]
    public async Task CaptureDiagnosticsAsync()
    {
        // Check whether the test failed
        bool failed =
            TestContext.CurrentContext.Result.Outcome.Status
            == TestStatus.Failed;

        // Store results in the project root directory
        string outputDirectory = Path.GetFullPath(
            Path.Combine(
                TestContext.CurrentContext.TestDirectory,
                "..", "..", "..",
                "TestResults"));

        // Generate a unique file name
        string fileName =
            $"{TestContext.CurrentContext.Test.Name}_" +
            $"{DateTime.UtcNow:yyyyMMdd_HHmmss_fff}";

        if (failed)
        {
            Directory.CreateDirectory(outputDirectory);

            // Capture screenshot on failure
            string screenshotPath = Path.Combine(
                outputDirectory,
                $"{fileName}.png");

            try
            {
                await Page.ScreenshotAsync(new()
                {
                    Path = screenshotPath,
                    FullPage = true
                });

                TestContext.Out.WriteLine(
                    $"Failure screenshot: {screenshotPath}");
            }
            catch (Exception ex)
            {
                TestContext.Out.WriteLine(
                    $"Screenshot failed: {ex.Message}");
            }
        }

        // Stop tracing and save only on failure
        try
        {
            if (failed)
            {
                string tracePath = Path.Combine(
                    outputDirectory,
                    $"{fileName}.zip");

                await Context.Tracing.StopAsync(new()
                {
                    Path = tracePath
                });

                TestContext.Out.WriteLine(
                    $"Failure trace: {tracePath}");
            }
            else
            {
                // Discard trace when the test passes
                await Context.Tracing.StopAsync();
            }
        }
        catch (Exception ex)
        {
            TestContext.Out.WriteLine(
                $"Trace collection failed: {ex.Message}");
        }
    }
}

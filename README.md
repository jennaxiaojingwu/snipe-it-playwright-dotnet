# Snipe-IT Asset Automation

An end-to-end UI test for the [Snipe-IT demo](https://demo.snipeitapp.com), built with **C#**, **.NET 10**, **Playwright for .NET**, and **NUnit**.

## Test scenario

The `CreateCheckoutAndVerifyAsset` test:

1. Logs in to the Snipe-IT demo.
2. Creates a **Macbook Pro 13"** asset with **Ready to Deploy** status and checks it out to a randomly selected user.
3. Searches for the asset by its auto-generated Asset Tag and verifies its Model, Status, and assigned user in the Assets list.
4. Opens the asset details and validates the Asset Tag, Model, Status, and assigned user.
5. Verifies the **create new** and **checkout** records in the History tab, including the checkout user and notes.

## Tech stack

- C# / .NET 10
- Playwright for .NET and NUnit
- Page Object Model (POM) with a reusable header component

## Project structure

```text
SnipeItAutomation/
├── Components/       # Shared UI components
├── Configuration/    # Environment configuration
├── Pages/            # Page Objects
├── Tests/            # E2E test and BaseTest
├── .gitignore
├── README.md
└── SnipeItAutomation.csproj
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [PowerShell (`pwsh`)](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell) to install Playwright browsers
- Internet access to the Snipe-IT demo

## Setup and run

From the project root, restore dependencies, build, and install Chromium:

```bash
dotnet restore
dotnet build
pwsh bin/Debug/net10.0/playwright.ps1 install chromium
```

Run the test:

```bash
dotnet test --filter "Name=CreateCheckoutAndVerifyAsset" -- Playwright.BrowserName=chromium
```

To watch the browser during execution:

```bash
dotnet test --filter "Name=CreateCheckoutAndVerifyAsset" -- Playwright.BrowserName=chromium Playwright.LaunchOptions.Headless=false
```

To run with Firefox or WebKit, install the browsers and select one:

```bash
pwsh bin/Debug/net10.0/playwright.ps1 install firefox webkit

dotnet test --filter "Name=CreateCheckoutAndVerifyAsset" -- Playwright.BrowserName=firefox
dotnet test --filter "Name=CreateCheckoutAndVerifyAsset" -- Playwright.BrowserName=webkit
```

The test has been run successfully locally with Chromium, Firefox, and WebKit. Playwright WebKit is not identical to testing the installed Safari application.

## Configuration

`Configuration/TestConfig.cs` uses the public demo URL and demo login credentials by default. Override them with environment variables if needed:

- `SNIPEIT_BASE_URL`
- `SNIPEIT_USERNAME`
- `SNIPEIT_PASSWORD`

Do not commit private credentials to the repository.

## Test design and diagnostics

Page interactions are organised using POM. The test reuses the generated Asset Tag and selected user for verification, uses Playwright auto-waiting and assertions, and does not rely on fixed sleeps. Random user selection is from the currently loaded eligible dropdown options; the same user can be selected on separate runs.

On failure, `BaseTest` saves a screenshot (`.png`) and Playwright trace (`.zip`) in `TestResults/`. To inspect a trace:

```bash
pwsh bin/Debug/net10.0/playwright.ps1 show-trace TestResults/your-trace-file.zip
```

`TestResults/` is excluded from Git because diagnostics may contain page or session data.

## Notes

The Snipe-IT demo is a shared public environment, so data may be changed or reset. Each run creates a new asset; automatic test data cleanup is not implemented.

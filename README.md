# Snipe-IT Asset Automation

An end-to-end UI automation test for the [Snipe-IT demo](https://demo.snipeitapp.com), built with **C#**, **.NET 10**, **Playwright for .NET**, and **NUnit**.

## Scenario covered

The `CreateCheckoutAndVerifyAsset` test:

1. Logs in to the Snipe-IT demo.
2. Creates a **Macbook Pro 13"** asset with **Ready to Deploy** status.
3. Checks out the asset to a randomly selected available user.
4. Searches the Assets list using the auto-generated Asset Tag and verifies that exactly one matching row is found.
5. Opens the asset from the list and validates its details (Asset Tag, Model, Status, and assigned user).
6. Verifies **create new** and **checkout** entries in the asset's **History** tab, including the assigned user and checkout notes.

## Technology

- .NET 10 (`net10.0`)
- C# and Playwright for .NET (`Microsoft.Playwright.NUnit`)
- NUnit test framework
- Page Object Model (POM), with a reusable header component

## Project structure

```text
SnipeItAutomation/
├── Components/
│   └── HeaderComponent.cs
├── Configuration/
│   └── TestConfig.cs
├── Pages/
│   ├── LoginPage.cs
│   ├── CreateAssetPage.cs
│   ├── AssetsPage.cs
│   └── AssetDetailsPage.cs
├── Tests/
│   ├── BaseTest.cs
│   └── AssetTests.cs
├── .gitignore
├── README.md
└── SnipeItAutomation.csproj
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [PowerShell (`pwsh`)](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell) to run the Playwright browser installer script
- Internet connection to access the public Snipe-IT demo

## Setup

From the project directory:

```bash
dotnet restore
dotnet build
pwsh bin/Debug/net10.0/playwright.ps1 install chromium
```

To also run Firefox and WebKit tests, install their browser binaries:

```bash
pwsh bin/Debug/net10.0/playwright.ps1 install firefox webkit
```

## Run the test

Run the test with Chromium (headless, default):

```bash
dotnet test --filter "Name=CreateCheckoutAndVerifyAsset" -- Playwright.BrowserName=chromium
```

To see the browser during execution:

```bash
dotnet test --filter "Name=CreateCheckoutAndVerifyAsset" -- Playwright.BrowserName=chromium Playwright.LaunchOptions.Headless=false
```

To run the same test on other browser engines:

```bash
dotnet test --filter "Name=CreateCheckoutAndVerifyAsset" -- Playwright.BrowserName=firefox
dotnet test --filter "Name=CreateCheckoutAndVerifyAsset" -- Playwright.BrowserName=webkit
```

**Note:** Playwright WebKit is not the same as testing the installed Safari application.

## Configuration

`Configuration/TestConfig.cs` reads optional environment variables and uses the public demo defaults if they are not set:

| Variable | Purpose |
| --- | --- |
| `SNIPEIT_BASE_URL` | Snipe-IT environment base URL |
| `SNIPEIT_USERNAME` | Login username |
| `SNIPEIT_PASSWORD` | Login password |

No additional environment variables are required for the public demo. For other environments, supply credentials through environment variables rather than committing them to the repository.

## Design choices

- **POM:** Page interactions are separated from the test's business-level assertions.
- **Reusable navigation:** `HeaderComponent` handles shared header actions.
- **Dynamic test data:** The test captures the automatically generated Asset Tag and reuses it for subsequent validations.
- **Random user:** A user is randomly selected from the currently loaded eligible dropdown options. The same user may be selected on different runs.
- **Auto-waiting:** Uses Playwright locators and web-first assertions instead of fixed sleeps.
- **Failure diagnostics:** `BaseTest` captures a screenshot and Playwright trace on failure, helping investigate failed steps.

## Failure diagnostics

When a test fails, `BaseTest` saves a full-page screenshot (`.png`) and a Playwright trace (`.zip`) in the root `TestResults/` directory. Successful test runs do not retain these artifacts.

To open a generated trace in Playwright Trace Viewer, run the following command from the project root, replacing the placeholder with the actual ZIP filename:

```bash
pwsh bin/Debug/net10.0/playwright.ps1 show-trace TestResults/your-trace-file.zip
```

Do not commit screenshots or trace archives, as they may contain session or page data. The `TestResults/` directory is listed in `.gitignore`.

## Local test execution results

The following runs were completed locally on 16 October 2026:

| Browser engine | Runs | Passed |
| --- | ---: | ---: |
| Chromium | 3 | 3 |
| Firefox | 1 | 1 |
| WebKit | 1 | 1 |

These results are a snapshot of local execution, not a guarantee that the shared demo will always be available.

## Notes and limitations

- The Snipe-IT demo is a **shared public environment**. Its data can change or be reset, so occasional external-environment failures are possible.
- Each test run creates a new asset. No automatic cleanup is performed; created records may remain available for inspection until the public demo data changes or is reset.
- Serial number is left empty because it is not required for the requested scenario.

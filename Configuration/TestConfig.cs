namespace SnipeItAutomation.Configuration;

public static class TestConfig
{
    public static string BaseUrl =>
        (Environment.GetEnvironmentVariable("SNIPEIT_BASE_URL")
        ?? "https://demo.snipeitapp.com").TrimEnd('/');

    public static string Username =>
        Environment.GetEnvironmentVariable("SNIPEIT_USERNAME")
        ?? "admin";

    public static string Password =>
        Environment.GetEnvironmentVariable("SNIPEIT_PASSWORD")
        ?? "password";
}

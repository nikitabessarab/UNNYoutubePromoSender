using System.Text.Json;

namespace UNNYoutubePromoSender;

public static class AppSettingsStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private static string SettingsPath => Path.Combine(FoundContactsStore.AppFolder, "ui_settings.json");

    public static void EnsureAppFolder() => Directory.CreateDirectory(FoundContactsStore.AppFolder);

    public static AppUiSettings Load()
    {
        try
        {
            if (!File.Exists(SettingsPath))
                return new AppUiSettings();
            var json = File.ReadAllText(SettingsPath);
            return JsonSerializer.Deserialize<AppUiSettings>(json, JsonOptions) ?? new AppUiSettings();
        }
        catch
        {
            return new AppUiSettings();
        }
    }

    public static void Save(AppUiSettings settings)
    {
        EnsureAppFolder();
        var json = JsonSerializer.Serialize(settings, JsonOptions);
        File.WriteAllText(SettingsPath, json);
    }
}

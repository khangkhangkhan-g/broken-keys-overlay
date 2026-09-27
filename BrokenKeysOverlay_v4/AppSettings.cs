using System.Text.Json;

namespace BrokenKeysOverlay;

public sealed class AppSettings
{
    public const int CurrentSchemaVersion = 3;

    public int SchemaVersion { get; set; } = CurrentSchemaVersion;
    public List<string> SelectedKeyIds { get; set; } = new();
    public int OverlayX { get; set; } = -1;
    public int OverlayY { get; set; } = -1;

    // Compact by default. The old MVP default was 46 px, which made a tiny
    // replacement palette feel much larger than Windows' small touch keyboard.
    public int KeySize { get; set; } = 34;
    public double Opacity { get; set; } = 0.96;
}

public static class SettingsService
{
    private static readonly string Folder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "BrokenKeysOverlay");

    private static readonly string FilePath = Path.Combine(Folder, "settings.json");

    public static AppSettings Load()
    {
        try
        {
            if (!File.Exists(FilePath)) return new AppSettings();

            var json = File.ReadAllText(FilePath);
            var settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();

            // v1/v2 used a much larger default. Migrate those settings once so
            // upgrading actually produces the compact overlay the user expects.
            if (settings.SchemaVersion < AppSettings.CurrentSchemaVersion)
            {
                settings.KeySize = 34;
                settings.SchemaVersion = AppSettings.CurrentSchemaVersion;
            }

            settings.KeySize = Math.Clamp(settings.KeySize, 30, 48);
            settings.Opacity = Math.Clamp(settings.Opacity, 0.45, 1.0);
            return settings;
        }
        catch
        {
            return new AppSettings();
        }
    }

    public static void Save(AppSettings settings)
    {
        settings.SchemaVersion = AppSettings.CurrentSchemaVersion;
        settings.KeySize = Math.Clamp(settings.KeySize, 30, 48);
        settings.Opacity = Math.Clamp(settings.Opacity, 0.45, 1.0);

        Directory.CreateDirectory(Folder);
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}

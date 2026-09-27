namespace BrokenKeysOverlay;

public sealed record KeyDefinition(
    string Id,
    string Label,
    ushort VirtualKey,
    string Category,
    bool IsModifier = false,
    bool IsExtended = false,
    string? Hint = null,
    string? OverlayText = null)
{
    public string OverlayLabel => string.IsNullOrWhiteSpace(OverlayText) ? Label : OverlayText;
}

public static class KeyCatalog
{
    public static readonly IReadOnlyList<KeyDefinition> All = Build();

    public static KeyDefinition? Find(string id) =>
        All.FirstOrDefault(k => string.Equals(k.Id, id, StringComparison.OrdinalIgnoreCase));

    private static List<KeyDefinition> Build()
    {
        var keys = new List<KeyDefinition>();

        for (char c = 'A'; c <= 'Z'; c++)
            keys.Add(new(c.ToString(), c.ToString(), c, "Letters"));

        for (ushort i = 0; i <= 9; i++)
            keys.Add(new($"D{i}", i.ToString(), (ushort)(0x30 + i), "Numbers & symbols"));

        keys.AddRange(new[]
        {
            new KeyDefinition("Oem3", "` ~", 0xC0, "Numbers & symbols"),
            new KeyDefinition("OemMinus", "- _", 0xBD, "Numbers & symbols"),
            new KeyDefinition("OemPlus", "= +", 0xBB, "Numbers & symbols"),
            new KeyDefinition("Oem4", "[ {", 0xDB, "Numbers & symbols"),
            new KeyDefinition("Oem6", "] }", 0xDD, "Numbers & symbols"),
            new KeyDefinition("Oem5", "\\ |", 0xDC, "Numbers & symbols"),
            new KeyDefinition("Oem1", "; :", 0xBA, "Numbers & symbols"),
            new KeyDefinition("Oem7", "' \"", 0xDE, "Numbers & symbols"),
            new KeyDefinition("OemComma", ", <", 0xBC, "Numbers & symbols"),
            new KeyDefinition("OemPeriod", ". >", 0xBE, "Numbers & symbols"),
            new KeyDefinition("Oem2", "/ ?", 0xBF, "Numbers & symbols"),
            new KeyDefinition("Oem102", "OEM 102", 0xE2, "Numbers & symbols", Hint: "Extra key on some ISO keyboards", OverlayText: "OEM"),
        });

        keys.AddRange(new[]
        {
            new KeyDefinition("Backspace", "Backspace", 0x08, "Editing", OverlayText: "⌫"),
            new KeyDefinition("Tab", "Tab", 0x09, "Editing"),
            new KeyDefinition("Enter", "Enter", 0x0D, "Editing", OverlayText: "↵"),
            new KeyDefinition("Escape", "Esc", 0x1B, "Editing"),
            new KeyDefinition("Space", "Space", 0x20, "Editing"),
            new KeyDefinition("Insert", "Insert", 0x2D, "Editing", IsExtended: true, OverlayText: "Ins"),
            new KeyDefinition("Delete", "Delete", 0x2E, "Editing", IsExtended: true, OverlayText: "Del"),
        });

        keys.AddRange(new[]
        {
            new KeyDefinition("Home", "Home", 0x24, "Navigation", IsExtended: true),
            new KeyDefinition("End", "End", 0x23, "Navigation", IsExtended: true),
            new KeyDefinition("PageUp", "PgUp", 0x21, "Navigation", IsExtended: true),
            new KeyDefinition("PageDown", "PgDn", 0x22, "Navigation", IsExtended: true),
            new KeyDefinition("Left", "←", 0x25, "Navigation", IsExtended: true),
            new KeyDefinition("Up", "↑", 0x26, "Navigation", IsExtended: true),
            new KeyDefinition("Right", "→", 0x27, "Navigation", IsExtended: true),
            new KeyDefinition("Down", "↓", 0x28, "Navigation", IsExtended: true),
        });

        keys.AddRange(new[]
        {
            new KeyDefinition("LCtrl", "Ctrl", 0xA2, "Modifiers", IsModifier: true),
            new KeyDefinition("RCtrl", "R Ctrl", 0xA3, "Modifiers", IsModifier: true, IsExtended: true, OverlayText: "R-Ctrl"),
            new KeyDefinition("LShift", "Shift", 0xA0, "Modifiers", IsModifier: true),
            new KeyDefinition("RShift", "R Shift", 0xA1, "Modifiers", IsModifier: true, OverlayText: "R-Shift"),
            new KeyDefinition("LAlt", "Alt", 0xA4, "Modifiers", IsModifier: true),
            new KeyDefinition("RAlt", "AltGr", 0xA5, "Modifiers", IsModifier: true, IsExtended: true),
            new KeyDefinition("LWin", "Win", 0x5B, "Modifiers", IsModifier: true, IsExtended: true),
            new KeyDefinition("RWin", "R Win", 0x5C, "Modifiers", IsModifier: true, IsExtended: true, OverlayText: "R-Win"),
            new KeyDefinition("CapsLock", "Caps", 0x14, "Modifiers"),
            new KeyDefinition("NumLock", "Num", 0x90, "Modifiers", IsExtended: true),
            new KeyDefinition("ScrollLock", "Scroll", 0x91, "Modifiers"),
        });

        for (ushort i = 1; i <= 24; i++)
            keys.Add(new($"F{i}", $"F{i}", (ushort)(0x6F + i), "Function"));

        keys.AddRange(new[]
        {
            new KeyDefinition("PrintScreen", "PrtSc", 0x2C, "Function", IsExtended: true),
            new KeyDefinition("Pause", "Pause", 0x13, "Function"),
            new KeyDefinition("Apps", "Menu", 0x5D, "Function", IsExtended: true),
        });

        for (ushort i = 0; i <= 9; i++)
            keys.Add(new($"NumPad{i}", $"Num {i}", (ushort)(0x60 + i), "Numpad", OverlayText: $"N{i}"));

        keys.AddRange(new[]
        {
            new KeyDefinition("Multiply", "Num ×", 0x6A, "Numpad", OverlayText: "N×"),
            new KeyDefinition("Add", "Num +", 0x6B, "Numpad", OverlayText: "N+"),
            new KeyDefinition("Subtract", "Num -", 0x6D, "Numpad", OverlayText: "N−"),
            new KeyDefinition("Decimal", "Num .", 0x6E, "Numpad", OverlayText: "N."),
            new KeyDefinition("Divide", "Num ÷", 0x6F, "Numpad", IsExtended: true, OverlayText: "N÷"),
        });

        keys.AddRange(new[]
        {
            new KeyDefinition("VolumeMute", "Mute", 0xAD, "Media"),
            new KeyDefinition("VolumeDown", "Vol -", 0xAE, "Media", OverlayText: "Vol−"),
            new KeyDefinition("VolumeUp", "Vol +", 0xAF, "Media", OverlayText: "Vol+"),
            new KeyDefinition("MediaNext", "Next", 0xB0, "Media", OverlayText: "Next"),
            new KeyDefinition("MediaPrevious", "Prev", 0xB1, "Media", OverlayText: "Prev"),
            new KeyDefinition("MediaStop", "Stop", 0xB2, "Media"),
            new KeyDefinition("MediaPlayPause", "Play/Pause", 0xB3, "Media", OverlayText: "Play"),
            new KeyDefinition("BrowserBack", "Browser ←", 0xA6, "Media", OverlayText: "B←"),
            new KeyDefinition("BrowserForward", "Browser →", 0xA7, "Media", OverlayText: "B→"),
            new KeyDefinition("BrowserRefresh", "Refresh", 0xA8, "Media", OverlayText: "↻"),
            new KeyDefinition("BrowserHome", "Browser Home", 0xAC, "Media", OverlayText: "B Home"),
        });

        return keys;
    }
}

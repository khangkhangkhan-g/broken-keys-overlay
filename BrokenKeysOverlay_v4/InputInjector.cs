using System.ComponentModel;
using System.Runtime.InteropServices;

namespace BrokenKeysOverlay;

public static class InputInjector
{
    private const uint INPUT_MOUSE = 0;
    private const uint INPUT_KEYBOARD = 1;
    private const uint INPUT_HARDWARE = 2;

    private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
    private const uint KEYEVENTF_KEYUP = 0x0002;

    // INPUT contains a native union. Even though this app only sends keyboard
    // input, the managed union MUST still be large enough for MOUSEINPUT.
    // Otherwise sizeof(INPUT) is wrong (32 instead of 40 bytes on x64) and
    // SendInput rejects every request.
    [StructLayout(LayoutKind.Sequential)]
    private struct INPUT
    {
        public uint type;
        public InputUnion U;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct InputUnion
    {
        [FieldOffset(0)] public MOUSEINPUT mi;
        [FieldOffset(0)] public KEYBDINPUT ki;
        [FieldOffset(0)] public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct HARDWAREINPUT
    {
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    static InputInjector()
    {
        // Guard against a broken P/Invoke layout ever shipping again.
        int expected = IntPtr.Size == 8 ? 40 : 28;
        int actual = Marshal.SizeOf<INPUT>();
        if (actual != expected)
        {
            throw new InvalidOperationException(
                $"Internal SendInput layout error: INPUT is {actual} bytes, expected {expected} bytes.");
        }
    }

    public static void Press(KeyDefinition key) => SendChord(Array.Empty<KeyDefinition>(), key);

    public static void SendChord(IReadOnlyCollection<KeyDefinition> modifiers, KeyDefinition key)
    {
        var inputs = new List<INPUT>();

        foreach (var modifier in modifiers)
            inputs.Add(MakeInput(modifier, keyUp: false));

        inputs.Add(MakeInput(key, keyUp: false));
        inputs.Add(MakeInput(key, keyUp: true));

        foreach (var modifier in modifiers.Reverse())
            inputs.Add(MakeInput(modifier, keyUp: true));

        Send(inputs);
    }

    public static void TapModifier(KeyDefinition modifier)
    {
        Send(new[] { MakeInput(modifier, false), MakeInput(modifier, true) });
    }

    public static void SendChordFromPhysicalKey(
        IReadOnlyCollection<KeyDefinition> modifiers,
        ushort virtualKey,
        bool extended)
    {
        var physical = new KeyDefinition(
            $"Physical_{virtualKey}",
            $"VK {virtualKey}",
            virtualKey,
            "Physical",
            IsExtended: extended);

        SendChord(modifiers, physical);
    }

    private static INPUT MakeInput(KeyDefinition key, bool keyUp)
    {
        uint flags = 0;
        if (key.IsExtended) flags |= KEYEVENTF_EXTENDEDKEY;
        if (keyUp) flags |= KEYEVENTF_KEYUP;

        return new INPUT
        {
            type = INPUT_KEYBOARD,
            U = new InputUnion
            {
                ki = new KEYBDINPUT
                {
                    wVk = key.VirtualKey,
                    wScan = 0,
                    dwFlags = flags,
                    time = 0,
                    dwExtraInfo = UIntPtr.Zero
                }
            }
        };
    }

    private static void Send(IEnumerable<INPUT> inputs)
    {
        var array = inputs.ToArray();
        if (array.Length == 0) return;

        int inputSize = Marshal.SizeOf<INPUT>();
        var sent = SendInput((uint)array.Length, array, inputSize);
        if (sent == array.Length) return;

        int error = Marshal.GetLastWin32Error();
        string reason;

        if (error != 0)
        {
            reason = $"Win32 error {error}: {new Win32Exception(error).Message}";
        }
        else
        {
            // SendInput is subject to UIPI. Microsoft notes that a UIPI block is
            // not necessarily reported through GetLastError.
            reason = "Windows returned no Win32 error. If the focused app is running as Administrator or is on a secure screen, Windows may be blocking lower-integrity input.";
        }

        throw new InvalidOperationException(
            $"SendInput sent {sent}/{array.Length} key events. {reason}");
    }
}

[[ENG] README.txt](https://github.com/user-attachments/files/32696721/ENG.README.txt)
Broken Keys Overlay

A compact Windows utility that temporarily replaces one or more broken keyboard keys.

Instead of opening the full on-screen keyboard and covering a large part of the screen, Broken Keys Overlay lets you choose only the keys you actually need. These keys appear as a small floating bar that stays above other applications while keeping the current text field or application focused.

The app is suitable for Windows laptops and desktop keyboards when one or more physical keys are broken, missing, or unreliable.

Developer
Nguyen Khang Pham
GitHub: https://github.com/khangkhangkhan-g
Email: nguyenkhangpham1306@gmail.com



	Features
• Select only the broken or missing keys you want to replace.
• Compact floating key bar instead of a full on-screen keyboard.
• Always stays visible above other windows.
• Does not intentionally steal focus from the current typing target.
• Works with common Windows applications such as browsers, Notepad, Microsoft Word, search boxes, and standard text fields.
• Supports letters, numbers, punctuation, navigation keys, function keys, modifier keys, and selected media/browser keys.
• Supports combinations using virtual Ctrl, Shift, Alt, and Win.
• Supports physical modifier + virtual key combinations.
• Supports virtual modifier + physical key combinations.
• Supports hold-to-repeat for keys such as Backspace, arrow keys, and letters.
• The floating bar can be dragged to any desired position.
• Key size and opacity can be adjusted.
• Remembers selected keys and overlay settings.

	System requirements
• Windows 10 or Windows 11
• 64-bit Windows (x64) for the current build script
• .NET 8 SDK when building from source code

Python is not required.
Broken Keys Overlay is written in C# / .NET 8 using Windows Forms and Win32 APIs.


Step-by-step installation guide

	1. Download the project

Download the project ZIP file from the project/repository page, then extract it.
Do not run the project directly while it is still inside the ZIP file.

	2. Install .NET 8 SDK

Download the official .NET 8 SDK for Windows x64 from Microsoft:
https://dotnet.microsoft.com/en-us/download/dotnet/8.0

Choose the SDK, not just the Runtime.
After installation, close and reopen File Explorer or Command Prompt if necessary.
To verify that .NET was installed correctly, open Command Prompt and run:

dotnet --version

If the displayed version starts with 8., the .NET 8 SDK has been installed successfully.

	3. Build the application

Open the extracted Broken Keys Overlay folder and double-click:

build.bat

A Command Prompt window will open and start building the Windows application.

Wait until you see:

Done.

The build script will create a self-contained Windows .exe file.

	4. Find the application file

After the build is complete, open:

bin\Release\net8.0-windows\win-x64\publish\

Run:

BrokenKeysOverlay.exe (or Run-dev)

Windows may hide the .exe extension, so the file may appear simply as BrokenKeysOverlay with the file type Application.

	5. First-time setup
1. Open BrokenKeysOverlay.exe. (or Run-dev)
2. Select the physical keyboard keys that are broken or missing.
3. Adjust the overlay size if needed.
4. Click Save overlay.
5. Click the application or text field where you want to type.
6. Click the replacement key on the floating overlay.

	Example:

Physical W key is broken
→ Select W in Broken Keys Overlay
→ Place the cursor in Chrome / Word / Notepad
→ Click W on the overlay
→ The W key is sent to the currently active typing target in Windows

Recommended overlay size

30–36 px   Compact for laptop / mouse / trackpad use
40–44 px   Easier to use with touch
45–48 px   Larger targets for touchscreens

The default size is designed to behave like a small floating keyboard strip rather than a full-screen on-screen keyboard.

	Keyboard shortcut examples

Physical Ctrl + virtual A
→ Ctrl+A

Virtual Ctrl + physical A
→ Ctrl+A

Virtual Ctrl + virtual A
→ Ctrl+A

Virtual Ctrl + virtual Shift + physical S
→ Ctrl+Shift+S

Virtual modifier keys use a one-shot mechanism, so you do not need to hold them with the mouse while waiting to press the next key.

	Reopen Settings

You can reopen Settings using the overlay grip or the system tray icon.

Depending on the current version, you can:
• Double-click the ⋮⋮ grip.
• Right-click the grip.
• Open the Broken Keys Overlay icon in the Windows system tray.

	Important limitations and disclaimer

Broken Keys Overlay is a Windows-only utility.

The application does not support:
• iOS / iPhone
• iPadOS / iPad
• macOS
• Android
• Linux
• ChromeOS

The current build configuration targets Windows x64. Windows ARM64 is not officially supported by the included build script.

	Fn key

On most laptops, the Fn key is handled directly by firmware, the embedded controller, or a manufacturer-specific hardware layer. Windows usually does not receive Fn as a normal virtual key.

Therefore, Broken Keys Overlay cannot guarantee replacement of the Fn key.

Manufacturer-specific keys such as brightness controls, keyboard backlight, airplane mode, performance mode, or other hardware-specific combinations may have the same limitation.

Running applications as Administrator

Windows restricts simulated input between applications running at different integrity levels.

If the target application is running with Run as Administrator while Broken Keys Overlay is running normally, Windows may block the injected input.

For normal applications, do not run the app as Administrator unless necessary.

	Games and special software

Some games, anti-cheat systems, Remote Desktop sessions, virtual machines, secure desktop environments, applications using Raw Input, or software with custom keyboard handling may ignore simulated input.

Compatibility with these cases is not guaranteed.

Windows security screens

The application is not designed to control Windows secure screens such as:
• Windows sign-in screen
• UAC secure desktop
• Ctrl+Alt+Delete screen

	Not a hardware repair tool

Broken Keys Overlay is only a temporary software workaround. It does not repair the physical keyboard and does not diagnose hardware faults.

If the keyboard is damaged by liquid, oxidation, a loose keyboard cable, a failed switch, or a motherboard issue, hardware repair may still be required.

	Troubleshooting

Error: '.NET 8 SDK was not found'

Install the official .NET 8 SDK for Windows x64:

https://dotnet.microsoft.com/en-us/download/dotnet/8.0

Then close and reopen the project folder and run build.bat again.

The overlay opens, but pressing a key does not type anything

Check the following:
1. Make sure a normal text field is focused before clicking the overlay.
2. Test with Notepad first.
3. Check whether the target application is running as Administrator while the overlay is running normally.
4. Test a simple letter key before trying shortcuts or special keys.
5. Restart the overlay if a previous test left a modifier in an active state.

Overlay is too large

Open Settings and reduce Overlay size to around 30–36 px for normal laptop use.
A special laptop key does not work
The key may be handled through firmware or a manufacturer-specific HID interface instead of a normal Windows virtual key. Such keys are not guaranteed to work with Broken Keys Overlay.


Manual build

	Normally, you only need to run:

build.bat

The build script uses a process equivalent to:
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true

The final .exe file is created at:
bin\Release\net8.0-windows\win-x64\publish\BrokenKeysOverlay.exe

	Project status

Broken Keys Overlay is an independent utility project. It is not affiliated with, sponsored by, or developed by Microsoft.

Windows, .NET, and Microsoft are trademarks of Microsoft Corporation.

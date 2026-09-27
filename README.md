from pathlib import Path

readme = r'''<div align="center">

# ⌨️ Broken Keys Overlay

### Replace only the keyboard keys that are broken — without opening a full on-screen keyboard.

A lightweight Windows utility that creates a compact floating key palette for broken, missing, or unreliable keyboard keys.

<br>

![Windows](https://img.shields.io/badge/Windows-10%20%7C%2011-0078D6?logo=windows&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![C%23](https://img.shields.io/badge/C%23-Windows%20Forms-239120?logo=csharp&logoColor=white)
![Python](https://img.shields.io/badge/Python-Not%20Required-3776AB?logo=python&logoColor=white)

</div>

---

## Overview

**Broken Keys Overlay** is designed for Windows laptops and desktop keyboards with one or more broken keys.

Instead of displaying the entire Windows on-screen keyboard, the app lets you select **only the keys you need** and places them in a small floating overlay.

The overlay stays above other applications while keeping the current text field focused, so you can continue typing in Chrome, Word, Notepad, search bars, and other standard Windows input fields.

> Example: if your physical **W** key is broken, select **W** in Broken Keys Overlay and use the floating replacement key whenever you need it.

---

## Features

- **Only show broken keys** — no full keyboard taking over your screen.
- **Compact floating overlay** that can be moved anywhere.
- **Always on top** while remaining unobtrusive.
- **Focus-preserving input** for normal Windows text fields.
- Supports:
  - Letters and numbers
  - Punctuation
  - Arrow/navigation keys
  - Function keys
  - `Ctrl`, `Shift`, `Alt`, and `Win`
  - Selected media/browser keys
- **Physical modifier + virtual key** combinations.
- **Virtual modifier + physical key** combinations.
- **Virtual modifier + virtual key** combinations.
- **Hold-to-repeat** for keys such as Backspace, arrows, and letters.
- Adjustable **overlay size** and **opacity**.
- Remembers your selected keys and overlay settings.

---

## System Requirements

| Requirement | Details |
|---|---|
| Operating system | Windows 10 or Windows 11 |
| Architecture | Windows x64 |
| Build requirement | .NET 8 SDK |
| Python | **Not required** |
| Language / Framework | C# / .NET 8 / Windows Forms / Win32 API |

---

## Installation

### 1. Download the project

Download the project ZIP from the repository and extract it to a normal folder.

> Do not run the project directly from inside the ZIP archive.

### 2. Install .NET 8 SDK

Download the official **.NET 8 SDK for Windows x64**:

https://dotnet.microsoft.com/en-us/download/dotnet/8.0

Choose **SDK**, not only the Runtime.

To confirm that the SDK is installed, open Command Prompt and run:

```bat
dotnet --version
```

If the result starts with `8.`, the SDK is installed correctly.

### 3. Build the application

Inside the extracted project folder, double-click:

```text
build.bat
```

Wait until the build process finishes and displays:

```text
Done.
```

### 4. Run the application

Open:

```text
bin\Release\net8.0-windows\win-x64\publish\
```

Then run:

```text
BrokenKeysOverlay.exe
```

You can also use:

```text
run-dev.bat
```

for development/testing.

---

## Quick Start

1. Open `BrokenKeysOverlay.exe`.
2. Select the physical keys that are broken or missing.
3. Adjust the overlay size if needed.
4. Click **Save overlay**.
5. Click the text field or application where you want to type.
6. Click the replacement key on the floating overlay.

### Example

```text
Physical W key is broken
        ↓
Select W in Broken Keys Overlay
        ↓
Place the cursor in Chrome / Word / Notepad
        ↓
Click W on the overlay
        ↓
W is sent to the currently active typing target
```

---

## Recommended Overlay Size

| Size | Recommended use |
|---|---|
| `30–36 px` | Laptop, mouse, or trackpad |
| `40–44 px` | Touch-friendly use |
| `45–48 px` | Larger touchscreen targets |

The default layout is intentionally compact so it behaves more like a **small floating replacement-key strip** than a full on-screen keyboard.

---

## Keyboard Shortcut Examples

| Input | Result |
|---|---|
| Physical `Ctrl` + Virtual `A` | `Ctrl+A` |
| Virtual `Ctrl` + Physical `A` | `Ctrl+A` |
| Virtual `Ctrl` + Virtual `A` | `Ctrl+A` |
| Virtual `Ctrl` + Virtual `Shift` + Physical `S` | `Ctrl+Shift+S` |

Virtual modifier keys use a **one-shot mechanism**, so you do not need to hold them with the mouse while pressing the next key.

---

## Reopen Settings

You can reopen the settings menu using the overlay grip or the system tray icon.

- Double-click the `⋮⋮` grip.
- Right-click the grip.
- Open Broken Keys Overlay from the Windows system tray.

---

## Important Limitations

### Windows only

Broken Keys Overlay currently supports **Windows only**.

It does **not** support:

- iOS / iPhone
- iPadOS / iPad
- macOS
- Android
- Linux
- ChromeOS

The included build script currently targets **Windows x64**.

Windows ARM64 is not officially supported by the current build configuration.

### Fn key

On most laptops, the `Fn` key is handled by firmware, an embedded controller, or manufacturer-specific hardware.

Windows often does not expose `Fn` as a normal virtual key.

Because of this, **Broken Keys Overlay cannot guarantee replacement of the Fn key**.

Manufacturer-specific functions such as:

- Brightness controls
- Keyboard backlight
- Airplane mode
- Performance mode
- Other hardware-specific combinations

may have similar limitations.

### Administrator permissions

Windows restricts simulated input between applications running at different integrity levels.

If the target application is running as **Administrator** while Broken Keys Overlay is running normally, Windows may block the injected input.

For normal applications, avoid running Broken Keys Overlay as Administrator unless necessary.

### Games and special software

Some applications may ignore simulated input, including:

- Games
- Anti-cheat systems
- Remote Desktop sessions
- Virtual machines
- Secure desktop environments
- Raw Input applications
- Software with custom keyboard handling

Compatibility with these cases is not guaranteed.

### Windows security screens

Broken Keys Overlay is not designed to control:

- Windows sign-in screen
- UAC secure desktop
- `Ctrl+Alt+Delete` screen

### Not a hardware repair tool

Broken Keys Overlay is a software workaround.

It does not repair or diagnose physical keyboard hardware.

If your keyboard is damaged by liquid, oxidation, a loose cable, failed switches, or a motherboard issue, hardware repair may still be required.

---

## Troubleshooting

### `.NET 8 SDK was not found`

Install the official .NET 8 SDK for Windows x64:

https://dotnet.microsoft.com/en-us/download/dotnet/8.0

Then reopen the project folder and run:

```text
build.bat
```

again.

### The overlay opens, but key presses do not type anything

Check the following:

1. Make sure a normal text field is focused before clicking the overlay.
2. Test with **Notepad** first.
3. Check whether the target application is running as Administrator.
4. Test a simple letter key before trying shortcuts or special keys.
5. Restart the overlay if a modifier was left active during a previous test.

### The overlay is too large

Open Settings and reduce **Overlay size** to around:

```text
30–36 px
```

for normal laptop use.

### A special laptop key does not work

The key may be handled by firmware or a manufacturer-specific HID interface instead of a normal Windows virtual key.

Such keys are not guaranteed to work with Broken Keys Overlay.

---

## Manual Build

Normally, you only need to run:

```text
build.bat
```

The build script uses a publish process equivalent to:

```bat
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

The final executable is created at:

```text
bin\Release\net8.0-windows\win-x64\publish\BrokenKeysOverlay.exe
```

---

## Developer

**Nguyen Khang Pham**

- GitHub: https://github.com/khangkhangkhan-g
- Email: nguyenkhangpham1306@gmail.com

---

## Project Status

Broken Keys Overlay is an independent utility project.

It is **not affiliated with, sponsored by, or developed by Microsoft**.

Windows, .NET, and Microsoft are trademarks of Microsoft Corporation.

---

<div align="center">

Built for keyboards that still work — except for *that one key*.

</div>
'''

out = Path("/mnt/data/README_GITHUB_POLISHED.md")
out.write_text(readme, encoding="utf-8")
print(f"Created: {out}")

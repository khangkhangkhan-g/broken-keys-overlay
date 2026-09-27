from pathlib import Path

readme = r'''<div align="center">

# ⌨️ Broken Keys Overlay

### Replace only the keyboard keys that are broken - without opening a full on-screen keyboard.

A lightweight Windows utility that creates a compact floating key palette for broken, missing, or unreliable keyboard keys.

<br>

![Windows](https://img.shields.io/badge/Windows-10%20%7C%2011-0078D6?logo=windows&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![C%23](https://img.shields.io/badge/C%23-Windows%20Forms-239120?logo=csharp&logoColor=white)
![Python](https://img.shields.io/badge/Python-Not%20Required-3776AB?logo=python&logoColor=white)

<br>

**[English](#english) · [Tiếng Việt](#tiếng-việt)**

</div>

---

<a id="english"></a>

# English

## What does Broken Keys Overlay help with?

A keyboard does not always fail completely. Sometimes only one or two keys stop working, double-register, become unreliable, or are physically missing.

In that situation, replacing the entire keyboard or opening a full Windows on-screen keyboard can be unnecessary and inconvenient.

**Broken Keys Overlay** solves that specific problem by letting you create a small floating replacement-key strip containing only the keys you actually need.

For example:

```text
Physical W key is broken
        ↓
Select W in Broken Keys Overlay
        ↓
Keep typing normally in Chrome, Word, Notepad, etc.
        ↓
Click W on the floating overlay whenever needed
```

The goal is simple:

> Keep using the keyboard you already have, while replacing only the keys that no longer work.

---

## Overview

**Broken Keys Overlay** is a lightweight Windows utility designed for laptops and desktop keyboards with one or more broken, missing, or unreliable keys.

Instead of displaying the entire Windows on-screen keyboard, the app lets you select **only the keys you need** and places them in a compact floating overlay.

The overlay stays above other applications while preserving the active typing target, so you can continue working in Chrome, Word, Notepad, search bars, messaging apps, and other standard Windows input fields.

> Example: if your physical **W** key is broken, select **W** in Broken Keys Overlay and use the floating replacement key whenever you need it.

---

## Features

- **Only show the keys you need** - no full keyboard taking over your screen.
- **Compact floating overlay** that can be moved anywhere.
- **Always on top** while remaining unobtrusive.
- **Focus-preserving input** for normal Windows text fields.
- Supports:
  - Letters and numbers
  - Punctuation
  - Arrow and navigation keys
  - Function keys
  - `Ctrl`, `Shift`, `Alt`, and `Win`
  - Selected media and browser keys
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

for development and testing.

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
| `30-36 px` | Laptop, mouse, or trackpad |
| `40-44 px` | Touch-friendly use |
| `45-48 px` | Larger touchscreen targets |

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
30-36 px
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

## Trust & Safety

Broken Keys Overlay is designed as a local Windows utility.

- It does not require a cloud account.
- It does not require Python.
- It does not collect passwords.
- It does not intentionally transmit typed content to an external server.
- It uses Windows input APIs locally to simulate selected replacement keys.
- It is not intended to bypass Windows security screens, UAC, anti-cheat systems, or protected environments.

As with any open-source utility that injects keyboard input, review the source code and build it yourself if you want maximum transparency.

---

## Developer

**Nguyen Khang Pham**

- GitHub: [https://github.com/khangkhangkhan-g](https://github.com/khangkhangkhan-g)
- Email: nguyenkhangpham1306@gmail.com

---

## Project Status

Broken Keys Overlay is an independent utility project.

It is **not affiliated with, sponsored by, or developed by Microsoft**.

Windows, .NET, and Microsoft are trademarks of Microsoft Corporation.

---

<div align="center">

Built for keyboards that still work - except for *that one key*.

</div>

---

<a id="tiếng-việt"></a>

# Tiếng Việt

## Broken Keys Overlay giúp gì?

Bàn phím không phải lúc nào cũng hỏng hoàn toàn. Rất nhiều trường hợp chỉ có một hoặc vài phím bị liệt, chập chờn, mất keycap, hoặc bấm không còn ổn định.

Trong tình huống đó, thay cả bàn phím hoặc bật bàn phím ảo đầy đủ của Windows thường khá bất tiện.

**Broken Keys Overlay** giải quyết đúng vấn đề này bằng cách cho phép bạn tạo một thanh phím thay thế nhỏ, chỉ chứa những phím thật sự bị hỏng.

Ví dụ:

```text
Phím W vật lý bị hỏng
        ↓
Chọn W trong Broken Keys Overlay
        ↓
Tiếp tục gõ bình thường trong Chrome, Word, Notepad...
        ↓
Khi cần W, bấm W trên overlay nổi
```

Mục tiêu rất đơn giản:

> Tiếp tục sử dụng bàn phím hiện tại, nhưng thay thế bằng phần mềm đúng những phím không còn hoạt động.

---

## Tổng quan

**Broken Keys Overlay** là một tiện ích Windows gọn nhẹ dành cho laptop hoặc bàn phím desktop có một hoặc nhiều phím bị hỏng, mất hoặc hoạt động không ổn định.

Thay vì hiển thị toàn bộ bàn phím ảo của Windows, ứng dụng chỉ cho phép bạn chọn **những phím thật sự cần thay thế** và đưa chúng lên một overlay nhỏ nổi trên màn hình.

Overlay luôn nằm trên các cửa sổ khác nhưng vẫn cố gắng giữ nguyên vùng nhập liệu hiện tại, nhờ đó bạn có thể tiếp tục gõ trong Chrome, Word, Notepad, thanh tìm kiếm, ứng dụng nhắn tin và các ô nhập liệu Windows thông thường.

> Ví dụ: nếu phím **W** vật lý bị hỏng, chỉ cần chọn **W** trong Broken Keys Overlay và dùng phím W nổi khi cần.

---

## Tính năng

- **Chỉ hiển thị phím bị hỏng** - không cần mở cả bàn phím ảo chiếm màn hình.
- **Overlay nhỏ gọn**, có thể kéo đến bất kỳ vị trí nào.
- **Luôn nổi trên cùng** nhưng không quá cồng kềnh.
- **Giữ focus vùng nhập liệu** trong các ứng dụng Windows thông thường.
- Hỗ trợ:
  - Chữ cái và số
  - Dấu câu
  - Phím mũi tên và điều hướng
  - Phím chức năng
  - `Ctrl`, `Shift`, `Alt`, `Win`
  - Một số phím media và trình duyệt
- Hỗ trợ tổ hợp **modifier vật lý + phím ảo**.
- Hỗ trợ tổ hợp **modifier ảo + phím vật lý**.
- Hỗ trợ tổ hợp **modifier ảo + phím ảo**.
- **Giữ phím để lặp** với Backspace, phím mũi tên, chữ cái...
- Chỉnh được **kích thước overlay** và **độ trong suốt**.
- Ghi nhớ danh sách phím và thiết lập overlay.

---

## Yêu cầu hệ thống

| Yêu cầu | Chi tiết |
|---|---|
| Hệ điều hành | Windows 10 hoặc Windows 11 |
| Kiến trúc | Windows x64 |
| Yêu cầu để build | .NET 8 SDK |
| Python | **Không cần** |
| Ngôn ngữ / Framework | C# / .NET 8 / Windows Forms / Win32 API |

---

## Cài đặt

### 1. Tải project

Tải file ZIP của project từ repository và giải nén ra một thư mục bình thường.

> Không chạy project trực tiếp từ bên trong file ZIP.

### 2. Cài .NET 8 SDK

Tải **.NET 8 SDK cho Windows x64** từ trang chính thức:

https://dotnet.microsoft.com/en-us/download/dotnet/8.0

Chọn bản **SDK**, không chỉ cài Runtime.

Để kiểm tra SDK đã cài thành công chưa, mở Command Prompt và chạy:

```bat
dotnet --version
```

Nếu kết quả bắt đầu bằng `8.`, nghĩa là SDK đã được cài đúng.

### 3. Build ứng dụng

Trong folder project đã giải nén, double-click:

```text
build.bat
```

Chờ quá trình build hoàn tất và xuất hiện:

```text
Done.
```

### 4. Chạy ứng dụng

Mở:

```text
bin\Release\net8.0-windows\win-x64\publish\
```

Sau đó chạy:

```text
BrokenKeysOverlay.exe
```

Trong quá trình phát triển hoặc test, bạn cũng có thể dùng:

```text
run-dev.bat
```

---

## Bắt đầu nhanh

1. Mở `BrokenKeysOverlay.exe`.
2. Chọn những phím vật lý đang bị hỏng hoặc bị mất.
3. Chỉnh kích thước overlay nếu cần.
4. Bấm **Save overlay**.
5. Click vào ô nhập liệu hoặc ứng dụng nơi bạn muốn gõ.
6. Bấm phím thay thế trên overlay nổi.

### Ví dụ

```text
Phím W vật lý bị hỏng
        ↓
Chọn W trong Broken Keys Overlay
        ↓
Đặt con trỏ trong Chrome / Word / Notepad
        ↓
Bấm W trên overlay
        ↓
W được gửi tới vùng nhập liệu đang hoạt động
```

---

## Kích thước overlay đề xuất

| Kích thước | Phù hợp |
|---|---|
| `30-36 px` | Laptop, chuột hoặc trackpad |
| `40-44 px` | Dễ bấm hơn trên màn hình cảm ứng |
| `45-48 px` | Mục tiêu cảm ứng lớn |

Thiết kế mặc định cố tình được giữ nhỏ để overlay hoạt động giống **một dải phím thay thế nổi**, thay vì biến thành một bàn phím ảo đầy đủ.

---

## Ví dụ tổ hợp phím

| Cách nhập | Kết quả |
|---|---|
| `Ctrl` vật lý + `A` ảo | `Ctrl+A` |
| `Ctrl` ảo + `A` vật lý | `Ctrl+A` |
| `Ctrl` ảo + `A` ảo | `Ctrl+A` |
| `Ctrl` ảo + `Shift` ảo + `S` vật lý | `Ctrl+Shift+S` |

Các phím modifier ảo sử dụng cơ chế **one-shot**, vì vậy bạn không cần giữ chuột trên `Ctrl` hoặc `Shift` trong lúc bấm phím tiếp theo.

---

## Mở lại phần cài đặt

Bạn có thể mở lại menu cài đặt bằng grip trên overlay hoặc icon trong system tray.

- Double-click vào grip `⋮⋮`.
- Right-click vào grip.
- Mở Broken Keys Overlay từ Windows system tray.

---

## Giới hạn quan trọng

### Chỉ hỗ trợ Windows

Broken Keys Overlay hiện chỉ hỗ trợ **Windows**.

Không hỗ trợ:

- iOS / iPhone
- iPadOS / iPad
- macOS
- Android
- Linux
- ChromeOS

Build script hiện tại target **Windows x64**.

Windows ARM64 chưa được hỗ trợ chính thức trong cấu hình build hiện tại.

### Phím Fn

Trên phần lớn laptop, phím `Fn` được xử lý bởi firmware, embedded controller hoặc phần cứng riêng của nhà sản xuất.

Windows thường không expose `Fn` như một virtual key bình thường.

Vì vậy, **Broken Keys Overlay không thể đảm bảo thay thế được phím Fn**.

Các chức năng riêng của nhà sản xuất như:

- Tăng giảm độ sáng
- Đèn nền bàn phím
- Airplane mode
- Performance mode
- Các tổ hợp phần cứng đặc biệt khác

cũng có thể gặp giới hạn tương tự.

### Quyền Administrator

Windows giới hạn việc mô phỏng input giữa các ứng dụng có mức quyền khác nhau.

Nếu ứng dụng đích đang chạy bằng **Administrator** nhưng Broken Keys Overlay chạy ở quyền bình thường, Windows có thể chặn input được inject.

Với ứng dụng thông thường, không nên chạy Broken Keys Overlay bằng Administrator nếu không thật sự cần.

### Game và phần mềm đặc biệt

Một số ứng dụng có thể bỏ qua input mô phỏng, gồm:

- Game
- Hệ thống anti-cheat
- Remote Desktop
- Máy ảo
- Secure desktop
- Ứng dụng dùng Raw Input
- Phần mềm có cơ chế keyboard handling riêng

Không đảm bảo tương thích với các trường hợp này.

### Màn hình bảo mật Windows

Broken Keys Overlay không được thiết kế để điều khiển:

- Màn hình đăng nhập Windows
- UAC secure desktop
- Màn hình `Ctrl+Alt+Delete`

### Không phải công cụ sửa phần cứng

Broken Keys Overlay chỉ là một giải pháp phần mềm thay thế tạm thời.

Ứng dụng không sửa hoặc chẩn đoán hư hỏng vật lý của bàn phím.

Nếu bàn phím bị hỏng do nước, oxy hóa, cáp lỏng, switch hỏng hoặc lỗi bo mạch, bạn vẫn có thể cần sửa phần cứng.

---

## Xử lý sự cố

### Báo `.NET 8 SDK was not found`

Cài .NET 8 SDK cho Windows x64 từ:

https://dotnet.microsoft.com/en-us/download/dotnet/8.0

Sau đó quay lại folder project và chạy lại:

```text
build.bat
```

### Overlay mở nhưng bấm phím không nhập được

Kiểm tra lần lượt:

1. Đảm bảo một ô nhập liệu thông thường đang được focus trước khi bấm overlay.
2. Test bằng **Notepad** trước.
3. Kiểm tra ứng dụng đích có đang chạy bằng Administrator hay không.
4. Thử một phím chữ đơn giản trước khi test shortcut hoặc phím đặc biệt.
5. Restart overlay nếu một modifier bị giữ ở trạng thái active trong lần test trước.

### Overlay quá lớn

Mở Settings và giảm **Overlay size** xuống khoảng:

```text
30-36 px
```

cho nhu cầu laptop thông thường.

### Phím đặc biệt trên laptop không hoạt động

Phím đó có thể do firmware hoặc HID interface riêng của nhà sản xuất xử lý thay vì Windows virtual key bình thường.

Những phím này không được đảm bảo hoạt động với Broken Keys Overlay.

---

## Build thủ công

Thông thường bạn chỉ cần chạy:

```text
build.bat
```

Build script sử dụng publish process tương đương:

```bat
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

File `.exe` cuối cùng được tạo tại:

```text
bin\Release\net8.0-windows\win-x64\publish\BrokenKeysOverlay.exe
```

---

## Quyền riêng tư và an toàn

Broken Keys Overlay được thiết kế như một tiện ích Windows chạy cục bộ.

- Không yêu cầu tài khoản cloud.
- Không yêu cầu Python.
- Không yêu cầu hoặc thu thập mật khẩu.
- Không chủ đích gửi nội dung bạn gõ tới server bên ngoài.
- Chỉ dùng Windows input APIs cục bộ để mô phỏng những phím thay thế mà bạn chọn.
- Không được thiết kế để vượt qua Windows security screen, UAC, anti-cheat hoặc môi trường được bảo vệ.

Nếu muốn mức minh bạch cao nhất, bạn có thể tự review source code và tự build project.

---

## Nhà phát triển

**Nguyen Khang Pham**

- GitHub: [https://github.com/khangkhangkhan-g](https://github.com/khangkhangkhan-g)
- Email: nguyenkhangpham1306@gmail.com

---

## Trạng thái dự án

Broken Keys Overlay là một dự án tiện ích độc lập.

Dự án **không liên kết, không được tài trợ và không được phát triển bởi Microsoft**.

Windows, .NET và Microsoft là các nhãn hiệu thuộc Microsoft Corporation.

---

<div align="center">

Dành cho những chiếc bàn phím vẫn hoạt động tốt - ngoại trừ *đúng cái phím đó*.

</div>
'''

out = Path("/mnt/data/README_Broken_Keys_Bilingual.md")
out.write_text(readme, encoding="utf-8")
print(f"Created: {out}")

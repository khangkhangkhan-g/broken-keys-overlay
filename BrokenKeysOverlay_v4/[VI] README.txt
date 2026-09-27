Broken Keys Overlay

Một tiện ích nhỏ gọn dành cho Windows, giúp thay thế tạm thời một hoặc nhiều phím bàn phím bị hỏng.

Thay vì mở toàn bộ bàn phím ảo và che phần lớn màn hình, Broken Keys Overlay cho phép bạn chỉ chọn đúng những phím cần dùng. Các phím này sẽ xuất hiện dưới dạng một thanh nổi nhỏ, luôn nằm trên các ứng dụng khác nhưng vẫn giữ ô nhập liệu hoặc ứng dụng hiện tại ở trạng thái focus.

Ứng dụng phù hợp cho laptop và bàn phím desktop Windows khi một số phím vật lý bị hỏng, mất phím hoặc hoạt động không ổn định.

Developer
Nguyen Khang Pham
GitHub: https://github.com/khangkhangkhan-g
Email: nguyenkhangpham1306@gmail.com



	Tính năng
• Chỉ chọn những phím bị hỏng hoặc bị mất mà bạn muốn thay thế.
• Thanh phím nổi nhỏ gọn thay vì toàn bộ bàn phím ảo.
• Luôn hiển thị trên các cửa sổ khác.
• Không chủ động chiếm focus khỏi vị trí đang nhập liệu.
• Hoạt động với các ứng dụng Windows phổ biến như trình duyệt, Notepad, Microsoft Word, ô tìm kiếm và các trường nhập liệu thông thường.
• Hỗ trợ chữ cái, số, dấu câu, phím điều hướng, phím chức năng, phím modifier và một số phím media/trình duyệt.
• Hỗ trợ tổ hợp với Ctrl, Shift, Alt và Win ảo.
• Hỗ trợ tổ hợp phím modifier vật lý + phím ảo.
• Hỗ trợ phím modifier ảo + phím vật lý.
• Hỗ trợ giữ phím để lặp lại với các phím như Backspace, phím mũi tên và chữ cái.
• Có thể kéo thanh phím nổi đến vị trí mong muốn.
• Có thể chỉnh kích thước phím và độ trong suốt.
• Ghi nhớ danh sách phím đã chọn và cài đặt overlay.

	Yêu cầu hệ thống
• Windows 10 hoặc Windows 11
• Windows 64-bit (x64) đối với build script hiện tại
• .NET 8 SDK nếu build từ source code

Không cần cài Python.
Broken Keys Overlay được viết bằng C# / .NET 8, sử dụng Windows Forms và Win32 APIs.


Hướng dẫn cài đặt từng bước

	1. Tải project

Tải file ZIP của project từ trang project/repository, sau đó giải nén
Không chạy project trực tiếp khi file vẫn còn nằm bên trong ZIP.

	2. Cài .NET 8 SDK

Tải .NET 8 SDK cho Windows x64 chính thức từ Microsoft:
https://dotnet.microsoft.com/en-us/download/dotnet/8.0

Hãy chọn SDK, không chỉ cài Runtime.
Sau khi cài xong, nếu cần hãy đóng và mở lại File Explorer hoặc Command Prompt.
Để kiểm tra .NET đã được cài đúng chưa, mở Command Prompt và chạy:

dotnet --version

Nếu hiện phiên bản bắt đầu bằng 8. thì .NET 8 SDK đã được cài thành công.

	3. Build ứng dụng

Mở thư mục Broken Keys Overlay vừa giải nén và double-click:

build.bat

Một cửa sổ Command Prompt sẽ mở và bắt đầu build ứng dụng Windows.

Chờ đến khi hiện:

Done.

Build script sẽ tạo ra một file .exe Windows dạng self-contained.

	4. Tìm file chạy ứng dụng

Sau khi build xong, mở thư mục:

bin\Release\net8.0-windows\win-x64\publish\

Chạy file:

BrokenKeysOverlay.exe(Hoặc Run-dev)

Windows có thể đang ẩn đuôi .exe, vì vậy file có thể chỉ hiện tên BrokenKeysOverlay với loại file là Application.

	5. Thiết lập lần đầu
1. Mở BrokenKeysOverlay.exe.(Hoặc Run-dev)
2. Chọn đúng những phím trên bàn phím vật lý đang bị hỏng hoặc bị mất.
3. Chỉnh kích thước overlay nếu cần.
4. Nhấn Save overlay.
5. Click vào ứng dụng hoặc ô nhập liệu mà bạn muốn gõ.
6. Click phím thay thế trên thanh overlay.

	Ví dụ:

Phím W vật lý bị hỏng
→ Chọn W trong Broken Keys Overlay
→ Đặt con trỏ vào Chrome / Word / Notepad
→ Click W trên overlay
→ Phím W được gửi vào vị trí nhập liệu đang active trên Windows

Kích thước overlay khuyến nghị

30–36 px   Dùng gọn trên laptop / chuột / trackpad
40–44 px   Dễ dùng hơn nếu thao tác bằng cảm ứng
45–48 px   Phím lớn hơn cho màn hình cảm ứng

Kích thước mặc định được thiết kế để hoạt động giống một thanh bàn phím nổi nhỏ thay vì một bàn phím ảo toàn màn hình.

	Ví dụ tổ hợp phím

Ctrl vật lý + A ảo
→ Ctrl+A

Ctrl ảo + A vật lý
→ Ctrl+A

Ctrl ảo + A ảo
→ Ctrl+A

Ctrl ảo + Shift ảo + S vật lý
→ Ctrl+Shift+S

Các modifier ảo hoạt động theo cơ chế one-shot, nghĩa là bạn không cần giữ chúng bằng chuột trong khi chờ bấm phím tiếp theo.

	Mở lại phần cài đặt

Bạn có thể mở lại phần Settings bằng tay cầm của overlay hoặc icon trong system tray.

Tùy phiên bản hiện tại, bạn có thể:
• Double-click vào tay cầm ⋮⋮.
• Right-click vào tay cầm.
• Mở icon Broken Keys Overlay trong Windows system tray.

	Giới hạn quan trọng và disclaimer

Broken Keys Overlay là một tiện ích chỉ dành cho Windows.

Ứng dụng không hỗ trợ:
• iOS / iPhone
• iPadOS / iPad
• macOS
• Android
• Linux
• ChromeOS

Cấu hình build hiện tại dành cho Windows x64. Windows ARM64 chưa được hỗ trợ chính thức bởi build script đi kèm.

	Phím Fn

Trên phần lớn laptop, phím Fn được xử lý trực tiếp bởi firmware, embedded controller hoặc lớp phần cứng riêng của nhà sản xuất. Windows thường không nhận Fn như một phím ảo thông thường.

Vì vậy, Broken Keys Overlay không thể đảm bảo thay thế được phím Fn.

Các phím đặc biệt của hãng như chỉnh độ sáng, đèn bàn phím, airplane mode, performance mode hoặc các tổ hợp phần cứng khác cũng có thể gặp giới hạn tương tự.

Ứng dụng chạy với quyền Administrator

Windows hạn chế việc gửi input giả lập giữa các ứng dụng chạy ở mức quyền khác nhau.

Nếu ứng dụng đích đang chạy Run as Administrator nhưng Broken Keys Overlay chạy ở quyền thông thường, Windows có thể chặn input được gửi vào.

Với các ứng dụng thông thường, không nên chạy app với quyền Administrator nếu không cần thiết.

	Game và phần mềm đặc biệt

Một số game, hệ thống anti-cheat, Remote Desktop, máy ảo, secure desktop, ứng dụng dùng Raw Input hoặc cơ chế nhận bàn phím riêng có thể bỏ qua input được giả lập.

Không đảm bảo tương thích với các trường hợp này.

Màn hình bảo mật của Windows

Ứng dụng không được thiết kế để điều khiển các màn hình bảo mật của Windows như:
• Màn hình đăng nhập Windows
• UAC secure desktop
• Màn hình Ctrl+Alt+Delete

	Không phải công cụ sửa phần cứng

Broken Keys Overlay chỉ là giải pháp phần mềm tạm thời. Ứng dụng không sửa bàn phím vật lý và cũng không chẩn đoán lỗi phần cứng.

Nếu bàn phím bị lỗi do vô nước, oxy hóa, lỏng cáp bàn phím, hỏng switch hoặc lỗi mainboard, bạn vẫn có thể cần sửa chữa phần cứng.

	Xử lý lỗi

Hiện lỗi '.NET 8 SDK was not found'

Cài .NET 8 SDK chính thức cho Windows x64:

https://dotnet.microsoft.com/en-us/download/dotnet/8.0

Sau đó đóng và mở lại thư mục project rồi chạy build.bat lần nữa.

Overlay mở được nhưng bấm phím không nhập được

Kiểm tra lần lượt:
1. Đảm bảo một ô nhập liệu bình thường đang được focus trước khi click overlay.
2. Test trước bằng Notepad.
3. Kiểm tra xem ứng dụng đích có đang chạy Administrator trong khi overlay chạy bình thường hay không.
4. Test một phím chữ đơn giản trước khi thử shortcut hoặc phím đặc biệt.
5. Khởi động lại overlay nếu lần test trước để modifier ở trạng thái đang active.

Overlay quá lớn

Mở Settings và giảm Overlay size xuống khoảng 30–36 px nếu dùng trên laptop thông thường.
Một phím đặc biệt trên laptop không hoạt động
Phím đó có thể được xử lý qua firmware hoặc giao diện HID riêng của nhà sản xuất thay vì thông qua một Windows virtual key thông thường. Những phím này không được đảm bảo hoạt động với Broken Keys Overlay.


Build thủ công

	Thông thường bạn chỉ cần chạy:

build.bat

Build script sử dụng quy trình tương đương với lệnh:
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true

File .exe cuối cùng được tạo tại:
bin\Release\net8.0-windows\win-x64\publish\BrokenKeysOverlay.exe

	Trạng thái project

Broken Keys Overlay là một project tiện ích độc lập, không trực thuộc, không được Microsoft bảo trợ và không phải sản phẩm do Microsoft phát triển.

Windows, .NET và Microsoft là các thương hiệu thuộc Microsoft Corporation.

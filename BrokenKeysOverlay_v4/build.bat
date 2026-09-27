@echo off
setlocal
cd /d "%~dp0"
where dotnet >nul 2>nul
if errorlevel 1 (
  echo .NET 8 SDK was not found.
  echo Install the .NET 8 SDK from Microsoft, then run this file again.
  pause
  exit /b 1
)

echo Building a single-file Windows executable...
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
if errorlevel 1 (
  echo Build failed.
  pause
  exit /b 1
)

echo.
echo Done.
echo EXE: bin\Release\net8.0-windows\win-x64\publish\BrokenKeysOverlay.exe
pause

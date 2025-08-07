# MTGA Fullscreen Helper

<img align="right" width="25%" src="screenshot.png">

Tired of casting the <kbd>ALT</kbd> + <kbd>ENTER</kbd> fullscreen spell at every scene change in MTG Arena on your ultrawide display? This tray script finally breaks the [years-long ultrawide ~~bug~~ curse Wizards forgot to unsummon](https://feedback.wizards.com/forums/918667-mtg-arena-bugs-product-suggestions/suggestions/47712281-can-t-use-ultrawide-fullscreen).

This was crafted for GitHub’s [For the Love of Code hackathon](https://gh.io/ftloc) using [GitHub Copilot](https://github.com/features/copilot), [Claude Sonnet 4](https://www.anthropic.com/claude/sonnet), and a `+1`/`+1` token of stubbornness. 

I usually conjure spells on macOS and Linux, but this Windows build is my first summoning in that domain. If you see bugs, send word to the council ([open an issue](https://github.com/leereilly/MTGAFullscreenHelper/issues/new)).

**Quick install:** Navigate to [Releases](https://github.com/leereilly/MTGAFullscreenHelper/releases/) and download the latest `MTGAFullscreenHelper-Setup.exe`. Double-click. Install. Magic.
<br clear="all" />

## Features

- **System Tray Application**: Runs in background with no visible window
- **Automatic Detection**: Monitors MTGA window state every second (configurable)
- **Automatic Restoration**: Sends ALT+ENTER when windowed mode is detected
- **Configurable Settings**: Customize via `config.json`
- **Simple Controls**: Right-click tray icon to toggle active/inactive or quit

## Requirements

- Windows 10/11
- .NET 6.0 Runtime or later
- Magic: The Gathering Arena

## Installation

### Option 1: Windows Installer
1. Download the latest installer (`MTGAFullscreenHelper-Setup.exe`) from the [Releases page](../../releases)
2. Run the installer and follow the setup wizard
3. (Optional) Choose to create a desktop shortcut or enable auto-start
4. Launch MTGA Fullscreen Helper from the Start Menu or desktop icon

### Option 2: Download Release
1. Download the latest release from the [Releases page](../../releases)
2. Extract to a folder of your choice
3. Run `MTGAFullscreenHelper.exe` (right-click → "Run as administrator" recommended)

### Option 3: Build from Source
1. Install [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
2. Clone or download this repository
3. Open terminal in the project folder
4. Run:
   ```bash
   dotnet build --configuration Release
   ```
5. Find the built executable in `bin\Release\net6.0-windows\`

## Usage

1. **Start the application**: Double-click `MTGAFullscreenHelper.exe` or run from command line
2. **Tray icon appears**: Look for the application icon in your system tray (notification area)
3. **Automatic monitoring**: The app will check MTGA's window state every second
4. **Right-click menu**: 
   - **Toggle Active**: Pause/resume monitoring
   - **Reset Counter**: Reset the fullscreen restoration count to zero
   - **Quit**: Exit the application

## Configuration

Edit `config.json` to customize settings:

```json
{
  "WindowTitle": "Magic The Gathering",
  "Executable": "MTGA.exe", 
  "CheckIntervalMs": 1000
}
```

- **WindowTitle**: The exact window title to look for (MTGA's default)
- **Executable**: Process name (not currently used, reserved for future features)
- **CheckIntervalMs**: How often to check window state in milliseconds

## Troubleshooting

### App doesn't detect MTGA
- Make sure MTGA is running and the window title matches the config
- Try running as administrator for better process access

### Alt+Enter doesn't work
- Ensure the app has permission to send keystrokes to other applications
- Run as administrator
- Check that MTGA accepts Alt+Enter for fullscreen toggle

### Tray icon missing
- Check the system tray overflow area (click the up arrow in tray)
- Restart the application

### Counter shows high numbers
- The counter may increment rapidly if the window title doesn't match exactly
- Use "Reset Counter" from the right-click menu to start fresh
- Try pausing the app, starting MTGA, waiting for it to go fullscreen, then reactivating

## Technical Details

- Built with .NET 6 Windows Forms
- Uses Win32 APIs for window detection
- Timer-based monitoring with configurable intervals
- Cross-process keystroke simulation via SendKeys

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Issues and pull requests are welcome!

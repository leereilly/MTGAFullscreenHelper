[Setup]
AppName=MTGA Fullscreen Helper
AppVersion=0.0.2
AppPublisher=Lee Reilly
DefaultDirName={autopf}\MTGA Fullscreen Helper
DefaultGroupName=MTGA Fullscreen Helper
OutputBaseFilename=MTGAFullscreenHelper-Setup
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "startupicon"; Description: "Start with Windows"; GroupDescription: "Auto-start options"

[Files]
Source: "bin\Release\net6.0-windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\MTGA Fullscreen Helper"; Filename: "{app}\MTGAFullscreenHelper.exe"
Name: "{group}\{cm:UninstallProgram,MTGA Fullscreen Helper}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\MTGA Fullscreen Helper"; Filename: "{app}\MTGAFullscreenHelper.exe"; Tasks: desktopicon
Name: "{autostartup}\MTGA Fullscreen Helper"; Filename: "{app}\MTGAFullscreenHelper.exe"; Tasks: startupicon

[Run]
Filename: "{app}\MTGAFullscreenHelper.exe"; Description: "{cm:LaunchProgram,MTGA Fullscreen Helper}"; Flags: nowait postinstall skipifsilent runascurrentuser
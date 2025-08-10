[Setup]
AppName=MTGA Fullscreen Helper
AppVersion=0.0.4
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
Source: "MTGAFullScreenHelper.ico"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\MTGA Fullscreen Helper"; Filename: "{app}\MTGAFullscreenHelper.exe"
Name: "{group}\{cm:UninstallProgram,MTGA Fullscreen Helper}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\MTGA Fullscreen Helper"; Filename: "{app}\MTGAFullscreenHelper.exe"; Tasks: desktopicon
Name: "{autostartup}\MTGA Fullscreen Helper"; Filename: "{app}\MTGAFullscreenHelper.exe"; Tasks: startupicon

[Run]
Filename: "{app}\MTGAFullscreenHelper.exe"; Description: "{cm:LaunchProgram,MTGA Fullscreen Helper}"; Flags: nowait postinstall skipifsilent runascurrentuser

[Code]
const
  BM_CLICK = $00F5;

var
  FakePage: TWizardPage;
  FakeProgress: TNewProgressBar;
  FakeLabel: TNewStaticText;
  FakeDone: Boolean;

procedure InitializeWizard;
begin
  // Create a custom page that will act as a fun fake install page
  FakePage := CreateCustomPage(wpReady, 'Installing MTGA Fullscreen Helper', 'A little MTG magic while we prepare your setup...');

  FakeLabel := TNewStaticText.Create(FakePage);
  FakeLabel.Parent := FakePage.Surface;
  FakeLabel.Left := 0;
  FakeLabel.Top := 20;
  FakeLabel.Width := FakePage.SurfaceWidth;
  FakeLabel.AutoSize := True;
  FakeLabel.Caption := '';

  FakeProgress := TNewProgressBar.Create(FakePage);
  FakeProgress.Parent := FakePage.Surface;
  FakeProgress.Left := 0;
  FakeProgress.Top := FakeLabel.Top + 28;
  FakeProgress.Width := FakePage.SurfaceWidth;
  FakeProgress.Min := 0;
  FakeProgress.Max := 100;
  FakeProgress.Position := 0;

  FakeDone := False;
end;

procedure DoFakeInstall;
var
  i, j, startPos: Integer;
  msgs: array[0..5] of string;
  oldNext, oldBack, oldCancel: Boolean;
begin
  // Six themed messages, ~1.67 seconds each => 10 seconds total
  msgs[0] := 'Playing Sol Ring for extra speed';
  msgs[1] := 'Sacrificing temporary files';
  msgs[2] := 'Exiling old versions';
  msgs[3] := 'Scrying for optimal settings';
  msgs[4] := 'Searching library for dependencies';
  msgs[5] := 'Untapping resources';

  // Disable navigation during fake install
  oldNext := WizardForm.NextButton.Enabled;
  oldBack := WizardForm.BackButton.Enabled;
  oldCancel := WizardForm.CancelButton.Enabled;
  WizardForm.NextButton.Enabled := False;
  WizardForm.BackButton.Enabled := False;
  WizardForm.CancelButton.Enabled := False;

  FakeProgress.Position := 0;
  startPos := 0;

  for i := 0 to 5 do
  begin
    FakeLabel.Caption := msgs[i] + '...';
    FakeLabel.Update;
    for j := 1 to 17 do // ~16.67% per phase (100/6 phases)
    begin
      if startPos + j <= 100 then
        FakeProgress.Position := startPos + j;
      FakeProgress.Update;
      WizardForm.Update; // keep UI responsive
      Sleep(100); // 0.1s * 17 = 1.7s per phase
    end;
    startPos := startPos + 17;
    if startPos > 100 then startPos := 100;
  end;

  // Ensure we reach 100%
  FakeProgress.Position := 100;
  FakeProgress.Update;

  // Re-enable navigation
  WizardForm.NextButton.Enabled := oldNext;
  WizardForm.BackButton.Enabled := oldBack;
  WizardForm.CancelButton.Enabled := oldCancel;
end;

// Let the wizard navigate naturally to the custom page (it's after wpReady).
// When it becomes current, run the fake stage once, then proceed.
procedure CurPageChanged(CurPageID: Integer);
begin
  if (CurPageID = FakePage.ID) and (not FakeDone) then
  begin
    DoFakeInstall();
    FakeDone := True;
    PostMessage(WizardForm.NextButton.Handle, BM_CLICK, 0, 0); // proceed to real install page
  end;
end;

// Keep default navigation behavior elsewhere
function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := True;
end;
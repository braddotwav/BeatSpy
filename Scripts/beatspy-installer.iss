#define AppName "BeatSpy"
#define AppExeName "BeatSpy.exe"
#define AppVersion "1.0"
#define AppPublisher "RealJamako"
#define AppURL "https://github.com/RealJamako/BeatSpy"

[Setup]
AppId=DD8DF703-E563-41BA-A4F4-31B7CCFD2D6F
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
VersionInfoVersion={#AppVersion}
DefaultDirName={commonpf64}\{#AppName}
DisableProgramGroupPage=auto
SolidCompression=yes
WizardStyle=modern
WizardSmallImageFile=../BeatSpy/Assets/Bmp/icon.bmp
AllowUNCPath=no
AllowNetworkDrive=no
LicenseFile=../LICENSE.txt
OutputDir=/bin/
OutputBaseFilename=BeatSpy-Installer
UninstallDisplayIcon={app}\{#AppExeName}
SetupIconFile=../BeatSpy/Assets/Ico/icon.ico
UninstallDisplayName={#AppName}

[Files]
Source: "../BeatSpy/bin/Publish/*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "startmenu"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Icons]
Name: "{autoprograms}\{#AppName}"; Filename: "{app}\{#AppExeName}"; Tasks: startmenu
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; Tasks: desktopicon

[UninstallDelete]
Type: filesandordirs; Name: "{userappdata}/BeatSpy"







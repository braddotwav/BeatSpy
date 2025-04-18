#define AppName "BeatSpy"
#define AppExeName "BeatSpy.exe"
#define AppPublisher "braddotwav"
#define AppURL "https://github.com/braddotwav/BeatSpy"
#ifndef AppVersion
  #define AppVersion = '0.0.0.0';
#endif

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
PrivilegesRequired=none
WizardStyle=modern
WizardSmallImageFile=../BeatSpy/Resources/Icon/Icon.bmp
AllowUNCPath=no
AllowNetworkDrive=no
LicenseFile=../LICENSE.txt
OutputDir=/bin/
OutputBaseFilename=beatspy_installer_win64
UninstallDisplayIcon={app}\{#AppExeName}
SetupIconFile=../BeatSpy/Resources/Icon/Icon.ico
UninstallDisplayName={#AppName}

[Files]
Source: "../BeatSpy\bin\framework-dependant\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";
Name: "startmenu"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}";

[Icons]
Name: "{autoprograms}\{#AppName}"; Filename: "{app}\{#AppExeName}"; Tasks: startmenu
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; Tasks: desktopicon

[UninstallDelete]
Type: filesandordirs; Name: "{userappdata}/BeatSpy"







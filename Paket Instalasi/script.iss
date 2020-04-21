[Setup]
AppName = Timer Application
AppVerName =  Timer Application v1.0
AppPublisher = Adi Zaenul
AllowCancelDuringInstall = yes
DefaultDirName={pf}\Timer Application
DefaultGroupName = Timer Application
Compression = lzma
SolidCompression = yes
OutputBaseFilename = TimerSetup
AllowNoIcons = yes
AlwaysRestart = no
AlwaysShowComponentsList = no
DisableProgramGroupPage = yes
AppendDefaultDirName = yes
CreateUninstallRegKey = yes
DisableStartupPrompt = yes
LanguageDetectionMethod=none
ShowLanguageDialog=no
Uninstallable = yes
UninstallFilesDir={app}\Uninstall
UninstallDisplayName = Timer Application
WindowVisible = no
AppCopyright = Copyright © 2020. Adi Zaenul
FlatComponentsList = yes
VersionInfoVersion = 1.1
SetupIconFile = setup.ico
DisableWelcomePage = no
WizardimageFile=win.bmp

[Languages]
Name: en; MessagesFile: compiler:Default.isl

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: checkedonce

[Files]
;My Application Runtime
Source: Files\*; DestDir: {app}; Flags: ignoreversion
Source: Data\*; DestDir: {app}\Database; Flags: ignoreversion
Source: Media\*; DestDir: {app}\Media; Flags: ignoreversion

[Icons]
Name: {group}\Timer Application; Filename: {app}\TimerApp.exe; WorkingDir: {app}
Name: {userdesktop}\Timer Application; Filename: {app}\TimerApp.exe; WorkingDir: {app}; Tasks: desktopicon

[Run]
Filename: "{app}\TimerApp.exe"; Description: "Launch Timer App"; Flags: postinstall runascurrentuser

[Registry]
;mencatat lokasi instalasi program, ini dibutuhkan jika kita ingin membuat paket instalasi update
Root: HKCU; Subkey: "Software\\Timer Application"; ValueName: "installDir"; ValueType: String; ValueData: {app}; Flags: uninsdeletevalue
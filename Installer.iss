; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "G910 Logitech Utilities"
#define MyAppVersion "1.0.1"
#define MyAppPublisher "Bura Chuhadar"
#define MyAppExeName "G910 Logitech Utilities.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{55197C1F-A8D3-4708-B2D2-4CF1AF3BD2D9}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputBaseFilename=G910 Logitech Utilities
Compression=lzma
SolidCompression=yes
WizardStyle=modern


[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked



[Files]
Source: "bin\Release\net7.0-windows\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net7.0-windows\App.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net7.0-windows\G910 Logitech Utilities.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net7.0-windows\G910 Logitech Utilities.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net7.0-windows\G910 Logitech Utilities.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net7.0-windows\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net7.0-windows\System.Data.SQLite.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net7.0-windows\log4net.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net7.0-windows\log4net.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net7.0-windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent





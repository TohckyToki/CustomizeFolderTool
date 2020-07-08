; �ű��� Inno Setup �ű��� ���ɣ�
; �йش��� Inno Setup �ű��ļ�����ϸ��������İ����ĵ���

#define MyAppName "CustomizeFolderTool"
#define MyAppVersion "0.1"
#define MyAppPublisher "YuengFu"

[Setup]
; ע: AppId��ֵΪ������ʶ��Ӧ�ó���
; ��ҪΪ������װ����ʹ����ͬ��AppIdֵ��
; (��Ҫ�����µ� GUID�����ڲ˵��е�� "����|���� GUID"��)
AppId={{B9F334C3-3AE8-4A3D-98AF-9F2E6BA93A61}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
; �Ƴ������У����ڹ���װģʽ�����У�Ϊ�����û���װ����
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputDir=C:\Users\yueng\source\repos\TohckyToki\CustomizeFolderTool\Setup
OutputBaseFilename=Installer
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "C:\Users\yueng\source\repos\TohckyToki\CustomizeFolderTool\Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; ע��: ��Ҫ���κι���ϵͳ�ļ���ʹ�á�Flags: ignoreversion��

[Icons]
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"

[Run]
Filename: "{app}\CustomizeFolderTool.exe"; Parameters: "-register --admin --add"; WorkingDir: {app}; Flags: skipifdoesntexist; StatusMsg: "���ڽ���ע��..."; Check: IsAdminInstallMode();
Filename: "{app}\CustomizeFolderTool.exe"; Parameters: "-register --user --add"; WorkingDir: {app}; Flags: skipifdoesntexist; StatusMsg: "���ڽ���ע��..."; Check: IsNotAdminInstallMode();

[Run]
Filename: "{app}\CustomizeFolderTool.exe"; Parameters: "-register --admin --delete"; WorkingDir: {app}; Flags: skipifdoesntexist; StatusMsg: "���ڽ��ע��..."; Check: IsAdminInstallMode();
Filename: "{app}\CustomizeFolderTool.exe"; Parameters: "-register --user --delete"; WorkingDir: {app}; Flags: skipifdoesntexist; StatusMsg: "���ڽ��ע��..."; Check: IsNotAdminInstallMode();

[Code]
function IsNotAdminInstallMode(): Boolean;
begin
  Result := not IsAdminInstallMode();
end;
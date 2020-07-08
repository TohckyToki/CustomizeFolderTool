; 脚本由 Inno Setup 脚本向导 生成！
; 有关创建 Inno Setup 脚本文件的详细资料请查阅帮助文档！

#define MyAppName "CustomizeFolderTool"
#define MyAppVersion "0.1"
#define MyAppPublisher "YuengFu"

[Setup]
; 注: AppId的值为单独标识该应用程序。
; 不要为其他安装程序使用相同的AppId值。
; (若要生成新的 GUID，可在菜单中点击 "工具|生成 GUID"。)
AppId={{B9F334C3-3AE8-4A3D-98AF-9F2E6BA93A61}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
; 移除以下行，以在管理安装模式下运行（为所有用户安装）。
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputDir=C:\Users\yueng\source\repos\TohckyToki\CustomizeFolderTool\Setup
OutputBaseFilename=setup
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "C:\Users\yueng\source\repos\TohckyToki\CustomizeFolderTool\Release\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; 注意: 不要在任何共享系统文件上使用“Flags: ignoreversion”

[Icons]
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"


using Microsoft.Win32;
using ToolRegister.Languages;

namespace ToolRegister;

public class Register
{
    private RegistryKey regDirectory { get; }

    public Register(bool isAdmin)
    {
        this.regDirectory = isAdmin
            ? Registry.ClassesRoot.CreateSubKey("Directory")
            : Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("Classes").CreateSubKey("Directory");
    }

    public void Add(ILanguage language)
    {
        this.Delete(false);

        var installedPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
        var exePath = Path.Combine(installedPath, $"{nameof(CustomizeFolderToolPlus)}.exe");
        var regMainMenu = default(RegistryKey);
        var regCmd = default(RegistryKey);
        try
        {
            regMainMenu = this.regDirectory.CreateSubKey("shell").CreateSubKey("CustomizeFolderTool");
            regMainMenu.SetValue("", "CustomizeFolderTool");
            regMainMenu.SetValue("Icon", Path.Combine(installedPath, "Assets", "CustomizeFolderTool.ico"), RegistryValueKind.String);
            regMainMenu.SetValue("ExtendedSubCommandsKey", @"Directory\ContextMenus\CustomizeFolderTool", RegistryValueKind.String);
            regMainMenu.Close();

            regMainMenu = this.regDirectory.CreateSubKey("ContextMenus").CreateSubKey("CustomizeFolderTool").CreateSubKey("shell");

            regCmd = regMainMenu.CreateSubKey("_01_AddAlias");
            regCmd.SetValue("", language.AddAlias);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} ""%1"" -a alias -lang {language.CodePage}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_02_DeleteAlias");
            regCmd.SetValue("", language.DeleteAlias);
            regCmd.SetValue("CommandFlags", 0x40, RegistryValueKind.DWord);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} ""%1"" -d alias -lang {language.CodePage}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_03_ModifyIcon");
            regCmd.SetValue("", language.ModifyIcon);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} ""%1"" -a icons -lang {language.CodePage}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_04_ClearIcon");
            regCmd.SetValue("", language.ClearIcon);
            regCmd.SetValue("CommandFlags", 0x40, RegistryValueKind.DWord);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} ""%1"" -d icons -lang {language.CodePage}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_05_AddRemark");
            regCmd.SetValue("", language.AddRemark);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} ""%1"" -a remarks -lang {language.CodePage}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_06_RemoveRemark");
            regCmd.SetValue("", language.RemoveRemark);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} ""%1"" -d remarks -lang {language.CodePage}");
            regCmd.Close();

        }
        catch (Exception)
        {
        }
        finally
        {
            regCmd?.Close();
            regMainMenu?.Close();
            this.regDirectory.Close();
        }
    }

    public void Delete(bool needClose = true)
    {
        try
        {
            if (this.regDirectory.OpenSubKey("shell")?.OpenSubKey("CustomizeFolderTool") != null)
            {
                this.regDirectory.CreateSubKey("shell").DeleteSubKeyTree("CustomizeFolderTool");
            }
            if (this.regDirectory.OpenSubKey("ContextMenus")?.OpenSubKey("CustomizeFolderTool") != null)
            {
                this.regDirectory.CreateSubKey("ContextMenus").DeleteSubKeyTree("CustomizeFolderTool");
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            if (needClose)
            {
                this.regDirectory.Close();
            }
        }
    }

    public static RegisterState CheckRegisterState()
    {
        var result = 0;

        var user = Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("Classes").CreateSubKey("Directory");
        result = user.OpenSubKey("shell")?.OpenSubKey("CustomizeFolderTool") != null
            ? 1 : user.OpenSubKey("ContextMenus")?.OpenSubKey("CustomizeFolderTool") != null
            ? 1 : result;

        var admin = Registry.ClassesRoot.CreateSubKey("Directory");
        result = admin.OpenSubKey("shell")?.OpenSubKey("CustomizeFolderTool") != null
            ? 2 : admin.OpenSubKey("ContextMenus")?.OpenSubKey("CustomizeFolderTool") != null
            ? 2 : result;

        return (RegisterState)result;
    }

    public enum RegisterState
    {
        None,
        User,
        Admin
    }
}

using Microsoft.Win32;

using ToolRegister.Languages;

namespace ToolRegister;

public class Register
{
    private RegistryKey regDirectory { get; }

    public Register(bool isAdmin)
    {
        regDirectory = isAdmin
            ? Registry.ClassesRoot.CreateSubKey("Directory")
            : Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("Classes").CreateSubKey("Directory");
    }

    public void Add(ILanguage language)
    {
        Delete(false);

        var installedPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
        var exePath = Path.Combine(installedPath, "CustomizeFolderTool.exe");
        var regMainMenu = default(RegistryKey);
        var regCmd = default(RegistryKey);
        try
        {
            regMainMenu = regDirectory.CreateSubKey("shell").CreateSubKey("CustomizeFolderTool");
            regMainMenu.SetValue("", "CustomizeFolderTool");
            regMainMenu.SetValue("Icon", Path.Combine(installedPath, "assets", "CustomizeFolderTool.ico"), RegistryValueKind.String);
            regMainMenu.SetValue("ExtendedSubCommandsKey", @"Directory\ContextMenus\CustomizeFolderTool", RegistryValueKind.String);
            regMainMenu.Close();

            regMainMenu = regDirectory.CreateSubKey("ContextMenus").CreateSubKey("CustomizeFolderTool").CreateSubKey("shell");

            regCmd = regMainMenu.CreateSubKey("_01_AddAlias");
            regCmd.SetValue("", language.AddAlias);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} -alias --add ""%1""");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_02_DeleteAlias");
            regCmd.SetValue("", language.DeleteAlias);
            regCmd.SetValue("CommandFlags", 0x40, RegistryValueKind.DWord);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} -alias --delete ""%1""");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_03_ModifyIcon");
            regCmd.SetValue("", language.ModifyIcon);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} -icons --add ""%1""");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_04_ClearIcon");
            regCmd.SetValue("", language.ClearIcon);
            regCmd.SetValue("CommandFlags", 0x40, RegistryValueKind.DWord);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} -icons --delete ""%1""");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_05_AddRemark");
            regCmd.SetValue("", language.AddRemark);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} -remark --add ""%1""");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_06_RemoveRemark");
            regCmd.SetValue("", language.RemoveRemark);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} -remark --delete ""%1""");
            regCmd.Close();

        }
        catch (Exception)
        {
        }
        finally
        {
            regCmd?.Close();
            regMainMenu?.Close();
            regDirectory.Close();
        }
    }

    public void Delete(bool needClose = true)
    {
        try
        {
            if (regDirectory.OpenSubKey("shell")?.OpenSubKey("CustomizeFolderTool") != null)
            {
                regDirectory.CreateSubKey("shell").DeleteSubKeyTree("CustomizeFolderTool");
            }
            if (regDirectory.OpenSubKey("ContextMenus")?.OpenSubKey("CustomizeFolderTool") != null)
            {
                regDirectory.CreateSubKey("ContextMenus").DeleteSubKeyTree("CustomizeFolderTool");
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            if (needClose)
            {
                regDirectory.Close();
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

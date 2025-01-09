using Microsoft.Win32;
using ToolLib.Languages.Register;
using static ToolLib.Constants;

namespace ToolLib;

public class RegistryManager
{
    private RegistryKey regDirectory { get; }
    private EnvironmentVariableTarget target { get; }

    public RegistryManager(bool isAdmin)
    {
        this.regDirectory = isAdmin
            ? Registry.ClassesRoot.CreateSubKey("Directory")
            : Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("Classes").CreateSubKey("Directory");

        this.target = isAdmin ? EnvironmentVariableTarget.Machine : EnvironmentVariableTarget.User;
    }

    public void Add(ILanguage language)
    {
        this.Delete(false);

        var installedPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
        var exePath = Path.Combine(installedPath, ToolExeFileName);
        var resourcesPath = Path.Combine(installedPath, ResourcesFolder);
        var regMainMenu = default(RegistryKey);
        var regCmd = default(RegistryKey);
        try
        {
            regMainMenu = this.regDirectory.CreateSubKey("shell").CreateSubKey(ToolName);
            regMainMenu.SetValue(string.Empty, ToolName);
            regMainMenu.SetValue("Icon", Path.Combine(installedPath, ToolIconFileName), RegistryValueKind.String);
            regMainMenu.SetValue("ExtendedSubCommandsKey", @"Directory\ContextMenus\CustomizeFolderTool", RegistryValueKind.String);
            regMainMenu.Close();

            regMainMenu = this.regDirectory.CreateSubKey("ContextMenus").CreateSubKey(ToolName).CreateSubKey("shell");

            regCmd = regMainMenu.CreateSubKey("_01_AddAlias");
            regCmd.SetValue("", language.AddAlias);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} ""%1"" -a alias -lang {language.CodePage}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_02_DeleteAlias");
            regCmd.SetValue("", language.DeleteAlias);
            regCmd.SetValue("CommandFlags", 0x40, RegistryValueKind.DWord);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} ""%1"" -d alias -lang {language.CodePage}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_03_ChangeIcon");
            regCmd.SetValue("", language.ChangeIcon);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} ""%1"" -a icon -lang {language.CodePage}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_04_RestoreIcon");
            regCmd.SetValue("", language.RestoreIcon);
            regCmd.SetValue("CommandFlags", 0x40, RegistryValueKind.DWord);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} ""%1"" -d icon -lang {language.CodePage}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_05_AddComment");
            regCmd.SetValue("", language.AddComment);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} ""%1"" -a comment -lang {language.CodePage}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_06_RemoveComment");
            regCmd.SetValue("", language.RemoveComment);
            regCmd.SetValue("CommandFlags", 0x40, RegistryValueKind.DWord);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} ""%1"" -d comment -lang {language.CodePage}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_07_RefreshFolder");
            regCmd.SetValue("", language.RefreshFolder);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} ""%1"" -ra");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_08_ResetFolder");
            regCmd.SetValue("", language.ResetFolder);
            regCmd.SetValue("CommandFlags", 0x40, RegistryValueKind.DWord);
            regCmd.CreateSubKey("command").SetValue("", $@"{exePath} ""%1"" -rs");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey("_09_OpenToolPath");
            regCmd.SetValue("", language.OpenToolPath);
            regCmd.CreateSubKey("command").SetValue("", $@"explorer {installedPath}");
            regCmd.Close();

            Environment.SetEnvironmentVariable(EnvName, resourcesPath, target);
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

            Environment.SetEnvironmentVariable(EnvName, null, target);
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

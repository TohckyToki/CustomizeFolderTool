#nullable disable

using CustomizeFolderToolPlus.Common.Languages.Register;
using Microsoft.Win32;
using static CustomizeFolderToolPlus.Common.Constants;

namespace CustomizeFolderToolPlus.Common;

public class RegistryManager
{
    private RegistryKey RegDirectory { get; }
    private EnvironmentVariableTarget Target { get; }

    public RegistryManager()
    {
        this.RegDirectory = Registry.CurrentUser.CreateSubKey(RegistryValue.Software).CreateSubKey(RegistryValue.Classes).CreateSubKey(RegistryValue.Directory);
        this.Target = EnvironmentVariableTarget.User;
    }

    public async Task Add(ILanguage language)
    {
        await this.Delete(false);

        var installedPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)!;
        var baseCommand = $"{Path.Combine(installedPath, ToolExeFileName)} {RegistryValue.CurrentLocation}";
        var languageSetting = $"{ToolCommand.Language} {language.CodePage}";
        var regMainMenu = default(RegistryKey);
        var regCmd = default(RegistryKey);
        try
        {
            regMainMenu = this.RegDirectory.CreateSubKey(RegistryValue.Shell).CreateSubKey(ToolName);
            regMainMenu.SetValue(string.Empty, ToolName);
            regMainMenu.SetValue(RegistryValue.Icon, Path.Combine(installedPath, ToolIconFileName), RegistryValueKind.String);
            regMainMenu.SetValue(RegistryValue.ExtendedSubCommandsKey, @$"{RegistryValue.Directory}\{RegistryValue.ContextMenus}\{ToolName}", RegistryValueKind.String);
            regMainMenu.Close();

            regMainMenu = this.RegDirectory.CreateSubKey(RegistryValue.ContextMenus).CreateSubKey(ToolName).CreateSubKey(RegistryValue.Shell);

            regCmd = regMainMenu.CreateSubKey(RegistryValue._01_AddAlias);
            regCmd.SetValue(string.Empty, language.AddAlias);
            regCmd.CreateSubKey(RegistryValue.Command).SetValue(string.Empty, $"{baseCommand} {ToolCommand.Add} {ToolCommand.Alias} {languageSetting}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey(RegistryValue._02_DeleteAlias);
            regCmd.SetValue(string.Empty, language.DeleteAlias);
            regCmd.SetValue(RegistryValue.CommandFlags, 0x40, RegistryValueKind.DWord);
            regCmd.CreateSubKey(RegistryValue.Command).SetValue(string.Empty, $"{baseCommand} {ToolCommand.Delete} {ToolCommand.Alias} {languageSetting}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey(RegistryValue._03_ChangeIcon);
            regCmd.SetValue(string.Empty, language.ChangeIcon);
            regCmd.CreateSubKey(RegistryValue.Command).SetValue(string.Empty, $"{baseCommand} {ToolCommand.Add} {ToolCommand.Icon} {languageSetting}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey(RegistryValue._04_RestoreIcon);
            regCmd.SetValue(string.Empty, language.RestoreIcon);
            regCmd.SetValue(RegistryValue.CommandFlags, 0x40, RegistryValueKind.DWord);
            regCmd.CreateSubKey(RegistryValue.Command).SetValue(string.Empty, $"{baseCommand} {ToolCommand.Delete} {ToolCommand.Icon} {languageSetting}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey(RegistryValue._05_AddComment);
            regCmd.SetValue(string.Empty, language.AddComment);
            regCmd.CreateSubKey(RegistryValue.Command).SetValue(string.Empty, $"{baseCommand} {ToolCommand.Add} {ToolCommand.Comment} {languageSetting}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey(RegistryValue._06_RemoveComment);
            regCmd.SetValue(string.Empty, language.RemoveComment);
            regCmd.SetValue(RegistryValue.CommandFlags, 0x40, RegistryValueKind.DWord);
            regCmd.CreateSubKey(RegistryValue.Command).SetValue(string.Empty, $"{baseCommand} {ToolCommand.Delete} {ToolCommand.Comment} {languageSetting}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey(RegistryValue._07_RefreshFolder);
            regCmd.SetValue(string.Empty, language.RefreshFolder);
            regCmd.CreateSubKey(RegistryValue.Command).SetValue(string.Empty, $"{baseCommand} {ToolCommand.Reapply}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey(RegistryValue._08_ResetFolder);
            regCmd.SetValue(string.Empty, language.ResetFolder);
            regCmd.SetValue(RegistryValue.CommandFlags, 0x40, RegistryValueKind.DWord);
            regCmd.CreateSubKey(RegistryValue.Command).SetValue(string.Empty, $"{baseCommand} {ToolCommand.Reset}");
            regCmd.Close();

            regCmd = regMainMenu.CreateSubKey(RegistryValue._09_OpenToolPath);
            regCmd.SetValue(string.Empty, language.OpenToolPath);
            regCmd.CreateSubKey(RegistryValue.Command).SetValue(string.Empty, $"{RegistryValue.Explorer} {installedPath}");
            regCmd.Close();

            regMainMenu.Close();

            await Task.Run(() => Environment.SetEnvironmentVariable(EnvName, installedPath, Target));
        }
        catch (Exception)
        {
        }
        finally
        {
            regCmd?.Close();
            regMainMenu?.Close();
            this.RegDirectory.Close();
        }
    }

    public async Task Delete(bool needClose = true)
    {
        try
        {
            if (this.RegDirectory.OpenSubKey(RegistryValue.Shell)?.OpenSubKey(ToolName) != null)
            {
                this.RegDirectory.CreateSubKey(RegistryValue.Shell).DeleteSubKeyTree(ToolName);
            }
            if (this.RegDirectory.OpenSubKey(RegistryValue.ContextMenus)?.OpenSubKey(ToolName) != null)
            {
                this.RegDirectory.CreateSubKey(RegistryValue.ContextMenus).DeleteSubKeyTree(ToolName);
            }

            if (needClose)
            {
                await Task.Run(() => Environment.SetEnvironmentVariable(EnvName, null, Target));
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            if (needClose)
            {
                this.RegDirectory.Close();
            }
        }
    }

    public static RegisterState CheckRegisterState()
    {
        var result = 0;

        var user = Registry.CurrentUser.CreateSubKey(RegistryValue.Software).CreateSubKey(RegistryValue.Classes).CreateSubKey(RegistryValue.Directory);
        result = user.OpenSubKey(RegistryValue.Shell)?.OpenSubKey(ToolName) != null
            ? 1 : user.OpenSubKey(RegistryValue.ContextMenus)?.OpenSubKey(ToolName) != null
            ? 1 : result;

        return (RegisterState)result;
    }

    public enum RegisterState
    {
        None,
        User,
    }
}

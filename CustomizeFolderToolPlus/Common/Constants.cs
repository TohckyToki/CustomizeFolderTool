namespace CustomizeFolderToolPlus.Common;

public abstract class Constants
{
    public const string ToolName = "CustomizeFolderTool";
    public const string ToolIconFileName = "CustomizeFolderTool.ico";
    public const string ToolExeFileName = "CustomizeFolderToolPlus.exe";
    public const string ToolResourceFileName = "ToolResources.dll";
    public const string ToolResourceFolder = "Resources";

    public const string ToolResourceTemplateFileName = "ToolResources.{0}.dll";

    public const string ToolConfigFileName = "";

    public const string EnvName = "CFT_BASE";
    public const string BaseFolder = $"%{EnvName}%";

    public const string Shell32 = "Shell32.dll";
    public const string Kernel32 = "kernel32";

    public const string DesktopFile = "desktop.ini";

    public abstract class ToolCommand
    {
        public const string Add = "-a";
        public const string Delete = "-d";
        public const string Reset = "-rs";
        public const string Reapply = "-ra";

        public const string Language = "-lang";

        public const string Alias = "alias";
        public const string Icon = "icon";
        public const string Comment = "comment";
    }

    public abstract class RegistryValue
    {
        public const string Software = "SOFTWARE";
        public const string Classes = "Classes";
        public const string Directory = "Directory";

        public const string CurrentLocation = @"""%1""";
        public const string Explorer = "explorer";

        public const string Shell = "shell";
        public const string Icon = "Icon";
        public const string ExtendedSubCommandsKey = "ExtendedSubCommandsKey";

        public const string ContextMenus = "ContextMenus";
        public const string Command = "command";
        public const string CommandFlags = "CommandFlags";

        public const string _01_AddAlias = "_01_AddAlias";
        public const string _02_DeleteAlias = "_02_DeleteAlias";
        public const string _03_ChangeIcon = "_03_ChangeIcon";
        public const string _04_RestoreIcon = "_04_RestoreIcon";
        public const string _05_AddComment = "_05_AddComment";
        public const string _06_RemoveComment = "_06_RemoveComment";
        public const string _07_RefreshFolder = "_07_RefreshFolder";
        public const string _08_ResetFolder = "_08_ResetFolder";
        public const string _09_OpenToolPath = "_09_OpenToolPath";
    }

    public abstract class Extensions
    {
        public const string Ico = ".ico";
        public const string Png = ".png";
    }

    public abstract class Wildcard
    {
        public const string Ico = "*.ico";
        public const string Png = "*.png";
    }
}

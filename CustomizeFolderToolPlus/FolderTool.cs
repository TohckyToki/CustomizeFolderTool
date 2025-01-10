using CustomizeFolderToolPlus.Common;
using System.Reflection;
using System.Text.RegularExpressions;
using static CustomizeFolderToolPlus.Common.Constants;

namespace CustomizeFolderToolPlus;

public class FolderTool
{
    private const int AliasIndex = 101;
    private const int IconIndex = 1;

    private string FolderPath { get; }
    private string ToolFolder { get; }
    private string ResourceFolder { get; }

    private uint ResourceId { get; set; }
    private string? ResourceFileFullName { get; set; }
    private string? ResourceFileRelativeName { get; set; }

    public static FolderTool Create(string folderPath)
    {
        var tool = new FolderTool(folderPath);
        tool.GenerateResourceFile();
        return tool;
    }

    public static void Reset(string folderPath)
    {
        var tool = new FolderTool(folderPath);
        tool.Reset();
    }

    public static void Reapply(string folderPath)
    {
        var tool = new FolderTool(folderPath);
        tool.Reapply();
    }

    private FolderTool(string folderPath)
    {
        this.FolderPath = folderPath;
        this.ToolFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        this.ResourceFolder = Path.Combine(this.ToolFolder, ToolResourceFolder);
    }

    private uint GetLastId()
    {
        if (!Directory.Exists(this.ResourceFolder))
        {
            Directory.CreateDirectory(this.ResourceFolder);
        }

        var files = Directory.GetFiles(this.ResourceFolder);
        var resourceFiles = files.Where(x => Regex.IsMatch(x, string.Format(ToolResourceTemplateFileName.Replace(".", @"\."), @"\d+")));
        var indexes = resourceFiles.Select(x => x.Replace(".dll", "").Split(".", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Last());
        var maxIndex = indexes.Max();
        return string.IsNullOrWhiteSpace(maxIndex) ? 1U : (uint.Parse(maxIndex) + 1);
    }

    private void GenerateResourceFile()
    {
        var id = FolderManager.GetFlags(this.FolderPath);
        if (id <= 0)
        {
            id = this.GetLastId();
        }
        var resourceFileFullName = Path.Combine(this.ResourceFolder, string.Format(ToolResourceTemplateFileName, id));
        if (!File.Exists(resourceFileFullName))
        {
            File.Copy(Path.Combine(this.ToolFolder, ToolResourceFileName), resourceFileFullName);
        }
        this.ResourceId = id;
        this.ResourceFileFullName = resourceFileFullName;
        this.ResourceFileRelativeName = Path.Combine(BaseFolder, ToolResourceFolder, Path.GetFileName(resourceFileFullName));
    }

    public void CreateAlias(string alias)
    {
        if (this.ResourceFileFullName != null && this.ResourceFileRelativeName != null)
        {
            ResourcesManager.CreateStringResources(this.ResourceFileFullName, AliasIndex, alias);
            FolderManager.SetLocalizedName(this.FolderPath, this.ResourceFileRelativeName, AliasIndex);
            FolderManager.ResetFlags(this.FolderPath);
            FolderManager.SetFlags(this.FolderPath, this.ResourceId);
        }
    }

    public void DeleteAlias()
    {
        FolderManager.RemoveLocalizedName(this.FolderPath);
    }

    public void CreateIcon(byte[] iconData)
    {
        if (this.ResourceFileFullName != null && this.ResourceFileRelativeName != null)
        {
            ResourcesManager.CreateIconResources(this.ResourceFileFullName, IconIndex, iconData);
            FolderManager.SetIcon(this.FolderPath, this.ResourceFileRelativeName, -1);
            FolderManager.ResetFlags(this.FolderPath);
            FolderManager.SetFlags(this.FolderPath, this.ResourceId);
        }
    }

    public void DeleteIcon()
    {
        FolderManager.RemoveIcon(this.FolderPath);
    }

    public void CreateComment(string tipinfo)
    {
        FolderManager.SetInfoTip(this.FolderPath, tipinfo);
        FolderManager.ResetFlags(this.FolderPath);
        FolderManager.SetFlags(this.FolderPath, this.ResourceId);
    }

    public void DeleteComment()
    {
        FolderManager.RemoveInfoTip(this.FolderPath);
    }

    private void Reset()
    {
        var id = FolderManager.GetFlags(this.FolderPath);
        var resourceFileFullName = Path.Combine(this.ResourceFolder, string.Format(ToolResourceTemplateFileName, id));
        if (File.Exists(resourceFileFullName))
        {
            File.Delete(resourceFileFullName);
        }
        var desktopFile = Path.Combine(this.FolderPath, DesktopFile);
        if (File.Exists(desktopFile))
        {
            File.Delete(desktopFile);
        }
    }

    private void Reapply()
    {
        var desktopFile = Path.Combine(this.FolderPath, DesktopFile);
        if (File.Exists(desktopFile))
        {
            FolderManager.RefreshFolder(this.FolderPath);
        }
    }
}

using IniParser;
using IniParser.Model;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace CustomizeFolderToolPlus;

public class FolderTool
{
    private const string DesktopFile = "desktop.ini";
    private const string ResourceFile = "desktop.ini.res.dll";

    private const string ShellClassInfo = ".ShellClassInfo";
    private const string LocalizedResourceName = "LocalizedResourceName";
    private const string IconResource = "IconResource";
    private const string InfoTip = "InfoTip";

    private readonly string _filePath;
    private readonly string _resourcesPath;
    private readonly IniData data;

    private static void CheckAuthority(string folderPath)
    {
        //Todo: Check if user has permission to modify this folder.
    }

    private static void CheckDesktopFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
    }

    private static void CheckResourceFile(string filePath)
    {
        var resourceFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Resources\\ToolResources.dll");
        if (!File.Exists(filePath))
        {
            File.Copy(resourceFile, filePath);

            new FileInfo(filePath)
            {
                Attributes = FileAttributes.Archive | FileAttributes.Hidden | FileAttributes.System
            }.Refresh();
        }
    }

    public static FolderTool CreateDesktopFile(string folderPath)
    {
        CheckAuthority(folderPath);
        var filePath = Path.Combine(folderPath, DesktopFile);
        var resourcesPath = Path.Combine(folderPath, ResourceFile);
        CheckDesktopFile(filePath);
        CheckResourceFile(resourcesPath);
        return new FolderTool(filePath, resourcesPath);
    }

    public static void DeleteDesktopFile(string folderPath)
    {
        CheckAuthority(folderPath);
        var deleteFlag = false;
        var resourcesPath = Path.Combine(folderPath, ResourceFile);
        if (File.Exists(resourcesPath))
        {
            File.Delete(resourcesPath);
            deleteFlag = true;
        }
        var filePath = Path.Combine(folderPath, DesktopFile);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            deleteFlag |= true;
        }
        if (deleteFlag)
        {
            ExplorerManager.RefreshFolder(folderPath);
        }
    }

    public static void ApplyDesktopFile(string folderPath)
    {
        CheckAuthority(folderPath);
        var filePath = Path.Combine(folderPath, DesktopFile);
        if (File.Exists(filePath))
        {
            ExplorerManager.RefreshFolder(folderPath);
        }
    }

    private FolderTool(string filePath, string resourcesPath)
    {
        this._filePath = filePath;
        this._resourcesPath = resourcesPath;
        var parser = new FileIniDataParser();
        this.data = parser.ReadFile(filePath);
    }

    private KeyDataCollection CreateSectionData(string sectionName)
    {
        this.data.Sections.AddSection(sectionName);
        return this.data.Sections[sectionName];
    }

    public FolderTool CreateAliasWithResource(string alias)
    {
        ResourcesManager.CreateStringResources(this._resourcesPath, 101, alias);

        var section = this.CreateSectionData(ShellClassInfo);
        section.RemoveKey(LocalizedResourceName);
        section.AddKey(LocalizedResourceName, $"@{this._resourcesPath},-101");
        return this;
    }

    public FolderTool CreateAlias(string alias)
    {
        var section = this.CreateSectionData(ShellClassInfo);
        section.RemoveKey(LocalizedResourceName);
        section.AddKey(LocalizedResourceName, alias);
        return this;
    }

    public FolderTool DeleteAlias()
    {
        var section = this.CreateSectionData(ShellClassInfo);
        section.RemoveKey(LocalizedResourceName);
        return this;
    }

    public FolderTool CreateIcon(string iconPath)
    {
        var section = this.CreateSectionData(ShellClassInfo);
        this.DeleteIconFile(section);
        section.AddKey(IconResource, Path.GetFileName(iconPath) + ",0");
        return this;
    }

    public FolderTool CreateIconWithResource(byte[] iconData)
    {
        ResourcesManager.CreateIconResources(this._resourcesPath, 1, iconData);

        var section = this.CreateSectionData(ShellClassInfo);
        section.RemoveKey(IconResource);
        section.AddKey(IconResource, $"@{this._resourcesPath},-1");
        return this;
    }

    public FolderTool DeleteIcon()
    {
        var section = this.CreateSectionData(ShellClassInfo);
        this.DeleteIconFile(section);
        return this;
    }

    private void DeleteIconFile(KeyDataCollection section)
    {
        if (section.ContainsKey(IconResource))
        {
            var oldIcon = section[IconResource];
            var imgPath = oldIcon.Split(',')[0];
            if (Path.GetExtension(imgPath) == ".ico")
            {
                var folderPath = Path.GetDirectoryName(this._filePath)!;
                File.Delete(Path.Combine(folderPath, imgPath));
            }
            section.RemoveKey(IconResource);
        }
    }

    public FolderTool CreateComment(string tipinfo)
    {
        var section = this.CreateSectionData(ShellClassInfo);
        section.RemoveKey(InfoTip);
        section.AddKey(InfoTip, tipinfo);
        return this;
    }

    public FolderTool DeleteComment()
    {
        var section = this.CreateSectionData(ShellClassInfo);
        section.RemoveKey(InfoTip);
        return this;
    }

    /// <summary>
    /// Save data to ini file
    /// </summary>
    public void Save()
    {
        var fileInfo = new FileInfo(this._filePath);
        fileInfo.Attributes = FileAttributes.Normal;
        File.WriteAllText(this._filePath, this.data.ToString(), Encoding.Unicode);
        var folder = Path.GetDirectoryName(this._filePath)!;
        ExplorerManager.RefreshFolder(folder);
    }


}

using IniParser;
using IniParser.Model;

using System.Runtime.InteropServices;
using System.Text;

namespace CustomizeFolderToolPlus;

public class FolderTool
{
    private const string DesktopFile = "desktop.ini";

    private const string ShellClassInfo = ".ShellClassInfo";
    private const string LocalizedResourceName = "LocalizedResourceName";
    private const string IconResource = "IconResource";
    private const string InfoTip = "InfoTip";

    private readonly string _filePath;
    private readonly IniData data;

    private static void CheckAuthority(string folderPath)
    {
        //Todo: Check if user has permission to modify this folder.
    }

    public static FolderTool CreateDesktopFile(string folderPath)
    {
        CheckAuthority(folderPath);
        var filePath = Path.Combine(folderPath, DesktopFile);
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
        return new FolderTool(filePath);
    }

    public static void DeleteDesktopFile(string folderPath)
    {
        CheckAuthority(folderPath);
        var filePath = Path.Combine(folderPath, DesktopFile);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    private FolderTool(string filePath)
    {
        var parser = new FileIniDataParser();
        this._filePath = filePath;
        this.data = parser.ReadFile(filePath);
    }

    private KeyDataCollection CreateSectionData(string sectionName)
    {
        this.data.Sections.AddSection(sectionName);
        return this.data.Sections[sectionName];
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
        this.RefreshSystemFile();
    }

    #region External
    private void RefreshSystemFile()
    {
        LPSHFOLDERCUSTOMSETTINGS FolderSettings = new LPSHFOLDERCUSTOMSETTINGS();

        FolderSettings.dwMask = 0x40;
        uint FCS_FORCEWRITE = 0x00000002;
        var pszPath = Path.GetDirectoryName(this._filePath)!;
        _ = SHGetSetFolderCustomSettings(ref FolderSettings, pszPath, FCS_FORCEWRITE);
    }

    [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
    private static extern uint SHGetSetFolderCustomSettings(ref LPSHFOLDERCUSTOMSETTINGS pfcs, string pszPath, uint dwReadWrite);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct LPSHFOLDERCUSTOMSETTINGS
    {
        public uint dwSize;
        public uint dwMask;
        public nint pvid;
        public string pszWebViewTemplate;
        public uint cchWebViewTemplate;
        public string pszWebViewTemplateVersion;
        public string pszInfoTip;
        public uint cchInfoTip;
        public nint pclsid;
        public uint dwFlags;
        public string pszIconFile;
        public uint cchIconFile;
        public int iIconIndex;
        public string pszLogo;
        public uint cchLogo;
    }
    #endregion
}

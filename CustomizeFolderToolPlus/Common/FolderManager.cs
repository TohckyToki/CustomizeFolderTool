#nullable disable

using System.Runtime.InteropServices;
using static ToolLib.Constants;

namespace ToolLib;

public static class FolderManager
{
    public static uint GetFlags(string folderPath)
    {
        var FolderSettings = new LPSHFOLDERCUSTOMSETTINGS
        {
            dwMask = (uint)FCSM.FCSM_FLAGS,
        };
        _ = SHGetSetFolderCustomSettings(ref FolderSettings, folderPath, (uint)FCS.FCS_READ);
        return FolderSettings.dwFlags;
    }

    public static void SetFlags(string folderPath, uint flag)
    {
        var FolderSettings = new LPSHFOLDERCUSTOMSETTINGS
        {
            dwMask = (uint)FCSM.FCSM_FLAGS,
            dwFlags = flag
        };
        _ = SHGetSetFolderCustomSettings(ref FolderSettings, folderPath, (uint)FCS.FCS_FORCEWRITE);
    }

    public static void RefreshFolder(string folderPath)
    {
        var flag = GetFlags(folderPath);
        SetFlags(folderPath, flag);
    }

    public static void SetInfoTip(string folderPath, string infoTip)
    {
        var FolderSettings = new LPSHFOLDERCUSTOMSETTINGS
        {
            dwMask = (uint)FCSM.FCSM_INFOTIP,
            pszInfoTip = infoTip
        };
        _ = SHGetSetFolderCustomSettings(ref FolderSettings, folderPath, (uint)FCS.FCS_FORCEWRITE);
    }

    public static void RemoveInfoTip(string folderPath)
    {
        SetInfoTip(folderPath, null);
    }

    public static void SetIcon(string folderPath, string resFile, int iconIndex)
    {
        var FolderSettings = new LPSHFOLDERCUSTOMSETTINGS
        {
            dwMask = (uint)FCSM.FCSM_ICONFILE,
            pszIconFile = resFile,
            iIconIndex = iconIndex
        };
        _ = SHGetSetFolderCustomSettings(ref FolderSettings, folderPath, (uint)FCS.FCS_FORCEWRITE);
    }

    public static void RemoveIcon(string folderPath)
    {
        SetIcon(folderPath, null, 0);
    }

    public static void SetLocalizedName(string folderPath, string resFile, int stringIndex)
    {
        _ = SHSetLocalizedName(folderPath, resFile, stringIndex);
    }

    public static void RemoveLocalizedName(string folderPath)
    {
        _ = SHSetLocalizedName(folderPath, null, 0);
    }

    #region External
    private enum FCSM
    {
        // Deprecated. pvid contains the folder's GUID.
        FCSM_VIEWID = 0b00000001,
        // Deprecated. pszWebViewTemplate contains a pointer to a buffer containing the path to the folder's WebView template.
        FCSM_WEBVIEWTEMPLATE = 0b00000010,
        // pszInfoTip contains a pointer to a buffer containing the folder's info tip.
        FCSM_INFOTIP = 0b00000100,
        // pclsid contains the folder's CLSID.
        FCSM_CLSID = 0b00001000,
        // pszIconFile contains the path to the file containing the folder's icon.
        FCSM_ICONFILE = 0b00010000,
        // pszLogo contains the path to the file containing the folder's thumbnail icon.
        FCSM_LOGO = 0b00100000,
        // Not used.
        FCSM_FLAGS = 0b01000000,
    }

    private enum FCS
    {
        FCS_READ = 0x00000001,
        FCS_FORCEWRITE = 0x00000002,
        FCS_WRITE = FCS_READ | FCS_FORCEWRITE,
    }

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

    [DllImport(Shell32, CharSet = CharSet.Auto)]
    private static extern uint SHGetSetFolderCustomSettings(ref LPSHFOLDERCUSTOMSETTINGS pfcs, string pszPath, uint dwReadWrite);

    [DllImport(Shell32, CharSet = CharSet.Auto)]
    private static extern uint SHSetLocalizedName(string path, string resourcePath, int resourceID);
    #endregion
}

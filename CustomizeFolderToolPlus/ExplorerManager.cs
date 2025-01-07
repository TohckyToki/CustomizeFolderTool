using System.Runtime.InteropServices;

namespace CustomizeFolderToolPlus
{
    public static class ExplorerManager
    {
        public static void RefreshFolder(string folderPath)
        {
            LPSHFOLDERCUSTOMSETTINGS FolderSettings = new LPSHFOLDERCUSTOMSETTINGS();

            FolderSettings.dwMask = 0x40;
            uint FCS_FORCEWRITE = 0x00000002;
            _ = SHGetSetFolderCustomSettings(ref FolderSettings, folderPath, FCS_FORCEWRITE);
        }

        #region External
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
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using System.IO;
using IniParser.Model;
using System.Runtime.InteropServices;

namespace CustomizeFolderTool.Util {
    public class Desktop {
        private string filePath;
        private IniData data;

        public static Desktop CreateDesktopFile(string folderPath) {
            CheckHasAuthority(folderPath);
            var filePath = Path.Combine(folderPath, "desktop.ini");
            if (!File.Exists(filePath)) {
                File.Create(filePath).Close();
            }
            return new Desktop(filePath);
        }

        public static void DeleteDesktopFile(string folderPath) {
            CheckHasAuthority(folderPath);
            var filePath = Path.Combine(folderPath, "desktop.ini");
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
        }

        private static void CheckHasAuthority(string folderPath) {
            //Todo: Check if it hasn't the authority of this directory, throw an exception of no rights to modify this directory.
        }

        private Desktop() {
        }

        private Desktop(string filePath) : this() {
            var parser = new FileIniDataParser();
            this.filePath = filePath;
            this.data = parser.ReadFile(filePath);
        }

        public void Save() {
            //Todo: Change the file properties to make sure it can be written.
            var fInfo = new FileInfo(filePath);
            fInfo.Attributes = FileAttributes.Normal;
            File.WriteAllText(filePath, data.ToString());
            RefreshSystemFile();
        }

        private KeyDataCollection GetOrCreateSectionData(string sectionName) {
            data.Sections.AddSection(sectionName);
            return data.Sections[sectionName];
        }

        public Desktop DeleteAlias() {
            var section = GetOrCreateSectionData(".ShellClassInfo");
            section.RemoveKey("LocalizedResourceName");
            return this;
        }

        public Desktop CreateAlias(string alias) {
            var section = GetOrCreateSectionData(".ShellClassInfo");
            section.RemoveKey("LocalizedResourceName");
            section.AddKey("LocalizedResourceName", alias);
            return this;
        }

        #region External
        private void RefreshSystemFile() {
            LPSHFOLDERCUSTOMSETTINGS FolderSettings = new LPSHFOLDERCUSTOMSETTINGS();
            FolderSettings.dwMask = 0x10;
            uint FCS_FORCEWRITE = 0x00000002;
            var pszPath = Path.GetDirectoryName(filePath);
            _ = SHGetSetFolderCustomSettings(ref FolderSettings, pszPath, FCS_FORCEWRITE);
        }

        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        private static extern uint SHGetSetFolderCustomSettings(ref LPSHFOLDERCUSTOMSETTINGS pfcs, string pszPath, uint dwReadWrite);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct LPSHFOLDERCUSTOMSETTINGS {
            public uint dwSize;
            public uint dwMask;
            public IntPtr pvid;
            public string pszWebViewTemplate;
            public uint cchWebViewTemplate;
            public string pszWebViewTemplateVersion;
            public string pszInfoTip;
            public uint cchInfoTip;
            public IntPtr pclsid;
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

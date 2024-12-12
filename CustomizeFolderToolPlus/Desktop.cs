using IniParser;
using IniParser.Model;

using System.Runtime.InteropServices;
using System.Text;

namespace CustomizeFolderToolPlus
{
    public class Desktop
    {
        private string filePath;
        private IniData data;

        public static Desktop CreateDesktopFile(string folderPath)
        {
            CheckHasAuthority(folderPath);
            var filePath = Path.Combine(folderPath, "desktop.ini");
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            return new Desktop(filePath);
        }

        public static void DeleteDesktopFile(string folderPath)
        {
            CheckHasAuthority(folderPath);
            var filePath = Path.Combine(folderPath, "desktop.ini");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private static void CheckHasAuthority(string folderPath)
        {
            //Todo: Check if it hasn't the authority of this directory, throw an exception of no rights to modify this directory.
        }

        private Desktop(string filePath)
        {
            var parser = new FileIniDataParser();
            this.filePath = filePath;
            data = parser.ReadFile(filePath);
        }

        /// <summary>
        /// save ini data to file
        /// </summary>
        public void Save()
        {
            var fInfo = new FileInfo(filePath);
            fInfo.Attributes = FileAttributes.Normal;
            File.WriteAllText(filePath, data.ToString(), Encoding.Unicode);
            RefreshSystemFile();
        }

        private KeyDataCollection CreateSectionData(string sectionName)
        {
            data.Sections.AddSection(sectionName);
            return data.Sections[sectionName];
        }

        public Desktop DeleteAlias()
        {
            var section = CreateSectionData(".ShellClassInfo");
            section.RemoveKey("LocalizedResourceName");
            return this;
        }

        public Desktop CreateAlias(string alias)
        {
            var section = CreateSectionData(".ShellClassInfo");
            section.RemoveKey("LocalizedResourceName");
            section.AddKey("LocalizedResourceName", alias);
            return this;
        }

        private void DeleteIconFile(KeyDataCollection section)
        {
            if (section.ContainsKey("IconResource"))
            {
                var oldIcon = section["IconResource"];
                var imgPath = oldIcon.Split(',')[0];
                if (Path.GetExtension(imgPath) == ".ico")
                {
                    var folderPath = Path.GetDirectoryName(filePath)!;
                    File.Delete(Path.Combine(folderPath, imgPath));
                }
                section.RemoveKey("IconResource");
            }
        }

        public Desktop DeleteIcon()
        {
            var section = CreateSectionData(".ShellClassInfo");
            DeleteIconFile(section);
            return this;
        }

        public Desktop CreateIcon(string iconPath)
        {
            var section = CreateSectionData(".ShellClassInfo");
            DeleteIconFile(section);
            section.AddKey("IconResource", Path.GetFileName(iconPath) + ",0");
            return this;
        }

        public Desktop DeleteRemark()
        {
            var section = CreateSectionData(".ShellClassInfo");
            section.RemoveKey("InfoTip");
            return this;
        }

        public Desktop CreateTipinfo(string tipinfo)
        {
            var section = CreateSectionData(".ShellClassInfo");
            section.RemoveKey("InfoTip");
            section.AddKey("InfoTip", tipinfo);
            return this;
        }

        #region External
        private void RefreshSystemFile()
        {
            LPSHFOLDERCUSTOMSETTINGS FolderSettings = new LPSHFOLDERCUSTOMSETTINGS();

            FolderSettings.dwMask = 0x40;
            uint FCS_FORCEWRITE = 0x00000002;
            var pszPath = Path.GetDirectoryName(filePath)!;
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
}

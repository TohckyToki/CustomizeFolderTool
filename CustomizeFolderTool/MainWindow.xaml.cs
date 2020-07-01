using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CustomizeFolderTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string desktop;

        public MainWindow()
        {
            if (string.IsNullOrWhiteSpace(App.Path))
            {
                this.Close();
                return;
            }
            InitializeComponent();
            this.Title = App.Path;
            desktop = Path.Combine(App.Path, "desktop.ini");
        }

        private void BtnModify_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAlias.Text))
            {
                MessageBox.Show("别名为空");
                return;
            }
            var sb = new StringBuilder();
            sb.AppendLine(Desktop.ShellClassInfo.Name);
            sb.Append($"{Desktop.ShellClassInfo.LocalizedResourceName} = {txtAlias.Text}");
            sb.AppendLine();
            if (File.Exists(desktop))
            {
                File.Delete(desktop);
            }
            File.WriteAllText(desktop, sb.ToString(), Encoding.Unicode);
            RefreshDirectory();


            LPSHFOLDERCUSTOMSETTINGS FolderSettings = new LPSHFOLDERCUSTOMSETTINGS();
            FolderSettings.dwMask = 0x10;
            uint FCS_FORCEWRITE = 0x00000002;
            var pszPath = App.Path;
            uint HRESULT = SHGetSetFolderCustomSettings(ref FolderSettings, pszPath, FCS_FORCEWRITE);
            this.Close();
        }

        private void RefreshDirectory()
        {
            var file = new FileInfo(desktop);
            var directory = new DirectoryInfo(App.Path);
            file.Attributes = FileAttributes.Hidden | FileAttributes.System | FileAttributes.Archive;
            file.Refresh();
            directory.Attributes = FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Directory;
            directory.Refresh();
        }

        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        static extern UInt32 SHGetSetFolderCustomSettings(ref LPSHFOLDERCUSTOMSETTINGS pfcs, string pszPath, UInt32 dwReadWrite);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct LPSHFOLDERCUSTOMSETTINGS
        {
            public UInt32 dwSize;
            public UInt32 dwMask;
            public IntPtr pvid;
            public string pszWebViewTemplate;
            public UInt32 cchWebViewTemplate;
            public string pszWebViewTemplateVersion;
            public string pszInfoTip;
            public UInt32 cchInfoTip;
            public IntPtr pclsid;
            public UInt32 dwFlags;
            public string pszIconFile;
            public UInt32 cchIconFile;
            public int iIconIndex;
            public string pszLogo;
            public UInt32 cchLogo;
        }
    }
}

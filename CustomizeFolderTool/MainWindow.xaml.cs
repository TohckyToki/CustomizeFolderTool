using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace CustomizeFolderTool {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private string desktop;

        public MainWindow() {
            if (string.IsNullOrWhiteSpace(App.Path)) {
                this.Close();
                return;
            }
            InitializeComponent();
            this.Title = App.Path;
            desktop = Path.Combine(App.Path, "desktop.ini");
        }

        private void BtnModify_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrWhiteSpace(txtAlias.Text)) {
                MessageBox.Show("别名为空");
                return;
            }
            var sb = new StringBuilder();
            sb.AppendLine(Desktop.ShellClassInfo.Name);
            sb.Append($"{Desktop.ShellClassInfo.LocalizedResourceName} = {txtAlias.Text}");
            sb.AppendLine();
            if (File.Exists(desktop)) {
                var file = new FileInfo(desktop);
                file.Attributes = FileAttributes.Normal;
                file.Refresh();
            }
            File.WriteAllText(desktop, sb.ToString(), Encoding.Default);
            RefreshDirectory();
            this.Close();
        }

        private void RefreshDirectory() {
            var file = new FileInfo(desktop);
            var directory = new DirectoryInfo(App.Path);
            directory.Attributes = FileAttributes.ReadOnly | FileAttributes.Directory;
            directory.Refresh();
            file.Attributes = FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.Archive;
            file.Refresh();
        }
    }
}

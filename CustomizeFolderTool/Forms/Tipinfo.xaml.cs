using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CustomizeFolderTool.Forms {
    /// <summary>
    /// Interaction logic for Tipinfo.xaml
    /// </summary>
    public partial class Tipinfo : Window {
        public static string folderPath;

        public Tipinfo() {
            InitializeComponent();
        }

        private void TextBoxKeyDown(object sender, KeyEventArgs eventArgs) {
            if (eventArgs.Key == Key.Enter) {
                if (CheckIsValided()) {
                    ApplyAlias();
                }
            }
        }

        private void Button1Click(object sender, RoutedEventArgs eventArgs) {
            if (CheckIsValided()) {
                ApplyAlias();
            }
        }

        private void Button2Click(object sender, RoutedEventArgs eventArgs) {
            Close();
        }

        private bool CheckIsValided() {
            if (string.IsNullOrEmpty(txtTipinfo.Text)) {
                txtTipinfo.Focus();
                return false;
            }
            return true;
        }

        private void ApplyAlias() {
            Util.Desktop.CreateDesktopFile(folderPath).CreateTipinfo(txtTipinfo.Text).Save();
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Process.GetCurrentProcess().Kill();
        }
    }
}

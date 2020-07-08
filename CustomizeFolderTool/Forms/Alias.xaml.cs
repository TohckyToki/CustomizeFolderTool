using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for Alias.xaml
    /// </summary>
    public partial class Alias : Window {
        public static string folderPath;

        public Alias() {
            InitializeComponent();

        }

        private void TextBoxKeyDown(object sender, KeyEventArgs eventArgs) {
            if (eventArgs.Key == Key.Enter) {
                if (CheckIsValided()) {
                    ApplyAlias();
                }
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs eventArgs) {
            if (CheckIsValided()) {
                ApplyAlias();
            }
        }

        private bool CheckIsValided() {
            if (string.IsNullOrEmpty(txtAlias.Text)) {
                txtAlias.Focus();
                return false;
            }
            return true;
        }

        private void ApplyAlias() {
            Util.Desktop.CreateDesktopFile(folderPath).CreateAlias(txtAlias.Text).Save();
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Process.GetCurrentProcess().Kill();
        }
    }
}

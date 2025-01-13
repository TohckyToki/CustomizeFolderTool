using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace CustomizeFolderTool.Forms
{
    /// <summary>
    /// Interaction logic for Alias.xaml
    /// </summary>
    public partial class Alias : Window
    {
        public static string folderPath;

        public Alias()
        {
            InitializeComponent();

        }

        private void TextBoxKeyDown(object sender, KeyEventArgs eventArgs)
        {
            if (eventArgs.Key == Key.Enter)
            {
                if (CheckIsValided())
                {
                    ApplyAlias();
                }
            }
        }

        private void Button1Click(object sender, RoutedEventArgs eventArgs)
        {
            if (CheckIsValided())
            {
                ApplyAlias();
            }
        }

        private void Button2Click(object sender, RoutedEventArgs eventArgs)
        {
            Close();
        }

        private bool CheckIsValided()
        {
            if (string.IsNullOrEmpty(txtAlias.Text))
            {
                txtAlias.Focus();
                return false;
            }
            return true;
        }

        private void ApplyAlias()
        {
            Util.Desktop.CreateDesktopFile(folderPath).CreateAlias(txtAlias.Text).Save();
            Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}

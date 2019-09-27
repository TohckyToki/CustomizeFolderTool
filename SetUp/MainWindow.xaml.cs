using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace SetUp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e) {
            var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var exe = Path.Combine(directory, "CustomizeFolderTool.exe");
            if (!File.Exists(exe)) {
                MessageBox.Show("CustomizeFolderTool.exe不存在");
                return;
            }
            Register.Add(exe + @" ""%1""");
            MessageBox.Show("添加成功");
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e) {
            Register.Delete();
            MessageBox.Show("删除成功");
        }
    }
}

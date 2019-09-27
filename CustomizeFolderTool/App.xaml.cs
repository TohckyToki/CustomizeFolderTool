using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CustomizeFolderTool {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public static string Path;
        private void Application_Startup(object sender, StartupEventArgs e) {
            if (e.Args.Length == 1 && System.IO.Directory.Exists(e.Args[0])) {
                Path = e.Args[0];
            }
            Path = @"D:\ZDHJ";
        }
    }
}

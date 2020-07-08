using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CustomizeFolderTool.Forms;
using CustomizeFolderTool.Util;

namespace CustomizeFolderTool {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private string[] Args;

        private void Application_Startup(object sender, StartupEventArgs e) {
            var isShutdown = true;
            Args = e.Args;
            if (Args?.Length > 0) {
                switch (Args[0]?.ToLower()) {
                    case "-register":
                        if (Args.Length > 1) {
                            Register register = default;
                            if (Args[1].ToLower() == "--admin") {
                                register = new Register(true);
                            } else if (Args[1].ToLower() == "--user") {
                                register = new Register(false);
                            }
                            if (Args.Length > 2) {
                                if (Args[2].ToLower() == "--add") {
                                    register.Add();
                                } else if (Args[2].ToLower() == "--delete") {
                                    register.Delete();
                                }
                            }
                        }
                        break;
                    case "-alias":
                        if (Args.Length > 2) {
                            if (Directory.Exists(Args[2])) {
                                if (Args[1].ToLower() == "--add") {
                                    this.StartupUri = new Uri("./Forms/Alias.xaml", UriKind.Relative);
                                    Alias.folderPath = Args[2];
                                    isShutdown = false;
                                } else if (Args[1].ToLower() == "--delete") {
                                    Desktop.CreateDesktopFile(Args[2]).DeleteAlias().Save();
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            if (isShutdown) {
                this.Shutdown();
            }
        }
    }
}

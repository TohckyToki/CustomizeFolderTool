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
        private void Application_Startup(object sender, StartupEventArgs e) {
            var isShutdown = true;
            if (e.Args?.Length > 0) {
                switch (e.Args[0]?.ToLower()) {
                    case "-register":
                        if (e.Args.Length > 1) {
                            Register register = default;
                            if (e.Args[1].ToLower() == "--admin") {
                                register = new Register(true);
                            } else if (e.Args[1].ToLower() == "--user") {
                                register = new Register(false);
                            }
                            if (e.Args.Length > 2) {
                                if (e.Args[2].ToLower() == "--add") {
                                    register.Add();
                                } else if (e.Args[2].ToLower() == "--delete") {
                                    register.Delete();
                                }
                            }
                        }
                        break;
                    case "-alias":
                        if (e.Args.Length > 2) {
                            if (Directory.Exists(e.Args[2])) {
                                if (e.Args[1].ToLower() == "--add") {
                                    this.StartupUri = new Uri("./Forms/Alias.xaml", UriKind.Relative);
                                    Alias.folderPath = e.Args[2];
                                    isShutdown = false;
                                } else if (e.Args[1].ToLower() == "--delete") {
                                    Desktop.CreateDesktopFile(e.Args[2]).DeleteAlias().Save();
                                }
                            }
                        }
                        break;
                    case "-color":
                        if (e.Args.Length > 2) {
                            if (Directory.Exists(e.Args[2])) {
                                if (e.Args[1].ToLower() == "--add") {
                                    this.StartupUri = new Uri("./Forms/Color.xaml", UriKind.Relative);
                                    Color.folderPath = e.Args[2];
                                    isShutdown = false;
                                } else if (e.Args[1].ToLower() == "--delete") {
                                    Desktop.CreateDesktopFile(e.Args[2]).DeleteAlias().Save();
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

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CustomizeFolderTool.Util {
    class Register {
        private RegistryKey regDirectory { get; }

        public Register(bool isAdmin) {
            if (isAdmin) {
                regDirectory = Registry.ClassesRoot.CreateSubKey("Directory");
            } else {
                regDirectory = Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("Classes").CreateSubKey("Directory");
            }
        }

        public void Add() {
            var installedPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var exePath = Path.Combine(installedPath, "CustomizeFolderTool.exe");
            RegistryKey regMainMenu = null;
            RegistryKey regCmd = null;
            try {
                regMainMenu = regDirectory.CreateSubKey("shell").CreateSubKey("CustomizeFolderTool");
                regMainMenu.SetValue("", "CustomizeFolderTool");
                regMainMenu.SetValue("Icon", Path.Combine(installedPath, "assets", "CustomizeFolderTool.ico"), RegistryValueKind.String);
                regMainMenu.SetValue("ExtendedSubCommandsKey", @"Directory\ContextMenus\CustomizeFolderTool", RegistryValueKind.String);
                regMainMenu.Close();

                regMainMenu = regDirectory.CreateSubKey("ContextMenus").CreateSubKey("CustomizeFolderTool").CreateSubKey("shell");

                regCmd = regMainMenu.CreateSubKey("AddAlias");
                regCmd.SetValue("", "添加别名");
                regCmd.CreateSubKey("command").SetValue("", $@"{exePath} -alias --add ""%1""");
                regCmd.Close();

                regCmd = regMainMenu.CreateSubKey("DeleteAlias");
                regCmd.SetValue("", "移除别名");
                regCmd.CreateSubKey("command").SetValue("", $@"{exePath} -alias --delete ""%1""");
                regCmd.Close();

            } catch (Exception) {
            } finally {
                regCmd?.Close();
                regMainMenu?.Close();
                regDirectory.Close();
            }
        }

        public void Delete() {
            try {
                regDirectory.CreateSubKey("shell").DeleteSubKeyTree("CustomizeFolderTool");
                regDirectory.CreateSubKey("ContextMenus").DeleteSubKeyTree("CustomizeFolderTool");
            } catch (Exception) {
            } finally {
                regDirectory.Close();
            }
        }
    }
}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SetUp {
    class Register {
        private const string MenuName = "Directory\\shell\\CustomizeFolderTool";
        private const string Command = "Directory\\shell\\CustomizeFolderTool\\command";

        public static void Add (string command) {
            RegistryKey regmenu = null;
            RegistryKey regcmd = null;
            try {
                regmenu = Registry.ClassesRoot.CreateSubKey(MenuName);
                if (regmenu != null) {
                    regmenu.SetValue("", "自定义文件夹");
                }
                regcmd = Registry.ClassesRoot.CreateSubKey(Command);
                if (regcmd != null)
                    regcmd.SetValue("", command);
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            } finally {
                if (regmenu != null)
                    regmenu.Close();
                if (regcmd != null)
                    regcmd.Close();
            }
        }

        public static void Delete() {
            try {
                RegistryKey reg = Registry.ClassesRoot.OpenSubKey(Command);
                if (reg != null) {
                    reg.Close();
                    Registry.ClassesRoot.DeleteSubKey(Command);
                }
                reg = Registry.ClassesRoot.OpenSubKey(MenuName);
                if (reg != null) {
                    reg.Close();
                    Registry.ClassesRoot.DeleteSubKey(MenuName);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

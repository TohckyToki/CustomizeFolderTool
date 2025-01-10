using CustomizeFolderToolPlus.Common;
using System.Drawing;
using System.Reflection;

namespace Test
{
    internal class Program
    {
        static void Main()
        {
            var file = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "ToolResources.dll");
            var icon = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Icon09.ico");

            ResourcesManager.CreateStringResources(file, 101, "FFF");
            ResourcesManager.CreateIconResources(file, 17, File.ReadAllBytes(icon));
            var rFile = Path.Combine("%CFT_TEST%", "ToolResources.dll");

            var iconFile = new Icon(icon);
            iconFile.ToBitmap().GetHicon();
            

            var folder = @"D:\test\aaa";
            FolderManager.RemoveLocalizedName(folder);
            FolderManager.SetLocalizedName(folder, rFile, 101);
            FolderManager.SetIcon(folder, rFile, 1);
        }
    }
}
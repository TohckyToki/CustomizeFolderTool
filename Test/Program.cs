using System.Reflection;
using ToolLib;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var file = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "ToolResources.dll");
            var icon = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Icon02.ico");

            ResourcesManager.CreateStringResources(file, 1, "QWE");
            ResourcesManager.CreateIconResources(file, 0, File.ReadAllBytes(icon));

            var folder = @"D:\test\zzz";
            FolderManager.SetInfoTip(folder, null);
            FolderManager.SetIcon(folder, file, 0);
            FolderManager.SetLocalizedName(folder, file, 1);
            var flags = FolderManager.GetFlags(folder);

            Console.WriteLine(flags);

        }
    }
}
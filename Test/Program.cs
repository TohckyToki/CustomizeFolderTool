using System.Reflection;
using System.Text;
using Vestris.ResourceLib;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var file = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                "ToolResources.dll");
            var icon = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                "IconDefault.ico");

            ResourcesManager.CreateIconResources(file, 1, File.ReadAllBytes(icon));
            ResourcesManager.CreateStringResources(file, 1, "ASD");

            // IconFile iconFile = new IconFile(icon);
            // IconDirectoryResource iconDirectoryResource = new IconDirectoryResource(iconFile);
            // iconDirectoryResource.SaveTo(file);
        }
    }
}
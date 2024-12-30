using CustomizeFolderToolPlus.Forms;
using CustomizeFolderToolPlus.Languages;

namespace CustomizeFolderToolPlus
{
    internal static class Program
    {
        private static string[] behaviors =
        {
            "-a", "-d",
        };

        private static string[] targets =
        {
            "alias", "icons", "remarks",
        };

        private static ILanguage[] languages =
        {
            new English(),
            new Chinese(),
        };

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[]? args)
        {
            if (args?.Length > 4
                && Directory.Exists(args[0])
                && behaviors.Contains(args[1].ToLower())
                && targets.Contains(args[2].ToLower())
                && args[3].ToLower() == "-lang")
            {
                var folder = args[0];
                var behavior = args[1].ToLower();
                var target = args[2].ToLower();
                var lang = args[4].ToLower();
                var language = languages[0];
                for (var i = 1; i < languages.Length; i++)
                {
                    if (lang == languages[i].CodePage.ToLower())
                    {
                        language = languages[i];
                    }
                }

                if (behavior == "-a")
                {
                    ApplicationConfiguration.Initialize();
                    IBaseForm? form = target switch
                    {
                        "alias" => new Alias(),
                        "icons" => new Icons(),
                        "remarks" => new Remarks(),
                        _ => default,
                    };
                    if (form != null)
                    {
                        form.FolderPath = folder;
                        form.Language = language;

                        Application.Run((Form)form);
                    }
                }
                else if (behavior == "-d")
                {
                    var desktop = target switch
                    {
                        "alias" => FolderTool.CreateDesktopFile(folder).DeleteAlias(),
                        "icons" => FolderTool.CreateDesktopFile(folder).DeleteIcons(),
                        "remarks" => FolderTool.CreateDesktopFile(folder).DeleteRemarks(),
                        _ => default,
                    };
                    desktop?.Save();
                    Application.Exit();
                }
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
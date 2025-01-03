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
            "alias", "icon", "comment",
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
                language = languages.FirstOrDefault(x => x.CodePage.ToLower() == lang) ?? language;

                if (behavior == "-a")
                {
                    ApplicationConfiguration.Initialize();
                    IBaseForm? form = target switch
                    {
                        "alias" => new Alias(),
                        "icon" => new Icons(),
                        "comment" => new Comment(),
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
                        "icon" => FolderTool.CreateDesktopFile(folder).DeleteIcon(),
                        "comment" => FolderTool.CreateDesktopFile(folder).DeleteComment(),
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
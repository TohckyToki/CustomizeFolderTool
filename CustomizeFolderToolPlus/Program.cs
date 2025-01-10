using CustomizeFolderToolPlus.Forms;
using CustomizeFolderToolPlus.Interfaces;
using ToolLib.Languages.Tool;
using static ToolLib.Constants;

namespace CustomizeFolderToolPlus;

internal static class Program
{
    private static string[][] behaviors =
    [
        [ToolCommand.Add, ToolCommand.Delete,],
        [ ToolCommand.Reset, ToolCommand.Reapply, ]
    ];

    private static string[] targets =
    [
        ToolCommand.Alias, ToolCommand.Icon, ToolCommand.Comment,
    ];

    private static ILanguage[] languages =
    [
        new English(),
        new Chinese(),
    ];

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[]? args)
    {
        if (args?.Length > 4
            && Directory.Exists(args[0])
            && behaviors[0].Contains(args[1].ToLower())
            && targets.Contains(args[2].ToLower())
            && args[3].ToLower() == ToolCommand.Language)
        {
            var folder = args[0];
            var behavior = args[1].ToLower();
            var target = args[2].ToLower();
            var lang = args[4].ToLower();
            var language = languages[0];
            language = languages.FirstOrDefault(x => x.CodePage.ToLower() == lang) ?? language;

            if (behavior == ToolCommand.Add)
            {
                ApplicationConfiguration.Initialize();
                IFormBase? form = target switch
                {
                    ToolCommand.Alias => new Alias(),
                    ToolCommand.Icon => new Icons(),
                    ToolCommand.Comment => new Comment(),
                    _ => default,
                };
                if (form != null)
                {
                    form.FolderPath = folder;
                    form.Language = language;

                    Application.Run((Form)form);
                }
            }
            else if (behavior == ToolCommand.Delete)
            {
                var desktop = target switch
                {
                    ToolCommand.Alias => FolderTool.CreateDesktopFile(folder).DeleteAlias(),
                    ToolCommand.Icon => FolderTool.CreateDesktopFile(folder).DeleteIcon(),
                    ToolCommand.Comment => FolderTool.CreateDesktopFile(folder).DeleteComment(),
                    _ => default,
                };
                desktop?.Save();
                Application.Exit();
            }
        }
        else if (args?.Length > 1
            && Directory.Exists(args[0])
            && behaviors[1].Contains(args[1].ToLower()))
        {
            var folder = args[0];
            var behavior = args[1].ToLower();
            if (behavior == ToolCommand.Reset)
            {
                FolderTool.DeleteDesktopFile(folder);
            }
            else if (behavior == ToolCommand.Reapply)
            {
                FolderTool.ApplyDesktopFile(folder);
            }
            Application.Exit();
        }
        else
        {
            Application.Exit();
        }
    }
}
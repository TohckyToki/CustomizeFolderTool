using CustomizeFolderToolPlus.Common.Languages.Tool;
using CustomizeFolderToolPlus.Forms;
using CustomizeFolderToolPlus.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static CustomizeFolderToolPlus.Common.Constants;

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
                IFormBase form = target switch
                {
                    ToolCommand.Alias => new Alias(),
                    ToolCommand.Icon => new Icons(),
                    ToolCommand.Comment => new Comment(),
                    _ => default!,
                };
                form.FolderPath = folder;
                form.Language = language;

                Application.Run((Form)form);
            }
            else if (behavior == ToolCommand.Delete)
            {
                var tool = FolderTool.Create(folder);
                switch (target)
                {
                    case ToolCommand.Alias:
                        tool.DeleteAlias();
                        break;
                    case ToolCommand.Icon:
                        tool.DeleteIcon();
                        break;
                    case ToolCommand.Comment:
                        tool.DeleteComment();
                        break;
                    default:
                        break;
                }
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
                FolderTool.Reset(folder);
            }
            else if (behavior == ToolCommand.Reapply)
            {
                FolderTool.Reapply(folder);
            }
            Application.Exit();
        }
        else
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Register());
        }
    }
}
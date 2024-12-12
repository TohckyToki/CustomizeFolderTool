using CustomizeFolderToolPlus.Forms;

namespace CustomizeFolderToolPlus
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[]? args)
        {
            if (args?.Length <= 2)
            {
                Application.Exit();
            }

            var folder = args![2];

            if (!Directory.Exists(folder))
            {
                Application.Exit();
            }

            var behavior = args![1].ToLower();
            var formName = args![0].ToLower();

            if (behavior == "--add")
            {
                var form = formName switch
                {
                    "-alias" => new Alias(),
                    "-icons" => new Icons(),
                    "-remark" => new Remark(),
                    _ => default(Form),
                };
                if (form != null)
                {
                    ((IBaseForm)form).FolderPath = folder;

                    ApplicationConfiguration.Initialize();
                    Application.Run(form);
                }
                else
                {
                    Application.Exit();
                }
            }
            else
            {
                if (behavior == "--delete")
                {
                    var desktop = formName switch
                    {
                        "-alias" => Desktop.CreateDesktopFile(folder).DeleteAlias(),
                        "-icons" => Desktop.CreateDesktopFile(folder).DeleteIcon(),
                        "-remark" => Desktop.CreateDesktopFile(folder).DeleteRemark(),
                        _ => default(Desktop),
                    };
                    desktop?.Save();
                }
                Application.Exit();
            }
        }
    }
}
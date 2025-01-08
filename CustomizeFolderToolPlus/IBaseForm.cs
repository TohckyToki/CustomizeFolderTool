using ToolLib.Languages.Tool;

namespace CustomizeFolderToolPlus
{
    internal interface IBaseForm
    {
        public string? FolderPath { get; set; }
        public ILanguage? Language { get; set; }
    }
}

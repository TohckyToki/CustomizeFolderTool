using CustomizeFolderToolPlus.Languages;

namespace CustomizeFolderToolPlus
{
    internal interface IBaseForm
    {
        public string? FolderPath { get; set; }
        public ILanguage? Language { get; set; }
    }
}

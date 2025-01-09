using ToolLib.Languages.Tool;

namespace CustomizeFolderToolPlus.Interfaces;

internal interface IFormBase
{
    public string? FolderPath { get; set; }
    public ILanguage? Language { get; set; }
}

namespace CustomizeFolderToolPlus.Languages;

public interface ILanguage : ILanguageBase
{
    public string ConfirmText { get; }
    public string CancelText { get; }

    public string AliasTitle { get; }
    public string AliasMessage { get; }

    public string IconsTitle { get; }

    public string RemarksTitle { get; }
    public string RemarksMessage { get; }
}

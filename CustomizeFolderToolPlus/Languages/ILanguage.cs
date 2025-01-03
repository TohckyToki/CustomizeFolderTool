namespace CustomizeFolderToolPlus.Languages;

public interface ILanguage : ILanguageBase
{
    public string ConfirmText { get; }
    public string CancelText { get; }

    public string AliasTitle { get; }
    public string AliasMessage { get; }

    public string IconTitle { get; }

    public string CommentTitle { get; }
    public string CommentMessage { get; }

    public string IconDefault { get; }
    public string Icon01 { get; }
    public string Icon02 { get; }
    public string Icon03 { get; }
    public string Icon04 { get; }
    public string Icon05 { get; }
    public string Icon06 { get; }
    public string Icon07 { get; }
    public string Icon08 { get; }
    public string Icon09 { get; }
    public string Icon10 { get; }
    public string Icon11 { get; }
    public string Icon12 { get; }
    public string Icon13 { get; }
    public string IconAdd { get; }
    public string IconAddTitle { get; }
    public string IconAddIcoFilter { get; }
    public string IconAddPngFilter { get; }
    public string IconAddWarningTitle { get; }
    public string IconAddWarningMessage { get; }
}

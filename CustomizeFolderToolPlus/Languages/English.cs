namespace CustomizeFolderToolPlus.Languages;

public class English : ILanguage
{
    public string CodePage => "en-US";

    public string ConfirmText => "OK";

    public string CancelText => "Cancel";

    public string AliasTitle => "Alias";

    public string AliasMessage => "Input your folder alias please";

    public string IconsTitle => "Icons";

    public string RemarksTitle => "Remarks";

    public string RemarksMessage => "Input remarks for your folder please";
}

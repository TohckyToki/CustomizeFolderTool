namespace ToolLib.Languages.Register;

public interface ILanguage : ILanguageBase
{
    public string LanguageTitle { get; }
    public string Register { get; }
    public string Unregister { get; }

    public string AddAlias { get; }
    public string DeleteAlias { get; }
    public string ChangeIcon { get; }
    public string RestoreIcon { get; }
    public string AddComment { get; }
    public string RemoveComment { get; }
    public string RefreshFolder { get; }
    public string ResetFolder { get; }
    public string OpenToolPath { get; }
}

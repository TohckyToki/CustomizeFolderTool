using CustomizeFolderToolPlus.Languages;

namespace ToolRegister.Languages
{
    public interface ILanguage : ILanguageBase
    {
        public string LanguageTitle { get; }
        public string Register { get; }
        public string Unregister { get; }
        public string Admin { get; }

        public string AddAlias { get; }
        public string DeleteAlias { get; }
        public string ModifyIcon { get; }
        public string ClearIcon { get; }
        public string AddRemark { get; }
        public string RemoveRemark { get; }
    }
}

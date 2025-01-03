using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolRegister.Languages
{
    public class English : ILanguage
    {
        public string CodePage => "en-US";

        public string LanguageTitle => "Language";

        public string Register => "Register";

        public string Unregister => "Unregister";

        public string Admin => "As Admin";

        public string AddAlias => "Add Alias";

        public string DeleteAlias => "Delete Alias";

        public string ChangeIcon => "Change Icon";

        public string RestoreIcon => "Restore Icon";

        public string AddComment => "Add Comment";

        public string RemoveComment => "Remove Comment";

        public string OpenToolPath => "Open register folder";
    }
}

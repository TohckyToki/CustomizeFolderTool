using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomizeFolderToolPlus.Languages
{
    public class Chinese : ILanguage
    {
        public string CodePage => "zh-CN";

        public string ConfirmText => "";

        public string CancelText => "";

        public string AliasTitle => "";

        public string AliasMessage => "";

        public string IconsTitle => "";

        public string RemarksTitle => "";

        public string RemarksMessage => "";
    }
}

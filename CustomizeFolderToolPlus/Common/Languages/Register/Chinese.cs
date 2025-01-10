namespace ToolLib.Languages.Register;

public class Chinese : ILanguage
{
    public string CodePage => "zh-CN";

    public string LanguageTitle => "语言选择";

    public string Register => "注册";

    public string Unregister => "解除";

    public string AddAlias => "添加别名";

    public string DeleteAlias => "移除别名";

    public string ChangeIcon => "更换图标";

    public string RestoreIcon => "还原图标";

    public string AddComment => "添加备注";

    public string RemoveComment => "删除备注";

    public string RefreshFolder => "刷新文件夹";

    public string ResetFolder => "重置文件夹";

    public string OpenToolPath => "打开注册文件夹";
}

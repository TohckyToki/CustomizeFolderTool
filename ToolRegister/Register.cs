using ToolLib;
using ToolLib.Languages.Register;

namespace ToolRegister;

public partial class Register : Form
{
    private ILanguage Language = new English();

    public Register()
    {
        InitializeComponent();
    }

    private void Register_Load(object sender, EventArgs e)
    {
        comboBox1.SelectedIndex = 0;
        CheckRegisterState();
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBox1.SelectedIndex == 0)
        {
            Language = new English();
            SetLanguage();
        }
        else if (comboBox1.SelectedIndex == 1)
        {
            Language = new Chinese();
            SetLanguage();
        }
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void button1_Click(object sender, EventArgs e)
    {
        var register = checkBox1.Checked ? new RegistryManager(true) : new RegistryManager(false);
        register.Add(Language);
        CheckRegisterState();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        var register = checkBox1.Checked ? new RegistryManager(true) : new RegistryManager(false);
        register.Delete();
        CheckRegisterState();
    }

    private void CheckRegisterState()
    {
        switch (RegistryManager.CheckRegisterState())
        {
            case RegistryManager.RegisterState.User:
            case RegistryManager.RegisterState.Admin:
                checkBox1.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = true;
                break;
            case RegistryManager.RegisterState.None:
            default:
                checkBox1.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = false;
                break;
        }
    }

    private void SetLanguage()
    {
        label1.Text = Language.LanguageTitle;
        button1.Text = Language.Register;
        button2.Text = Language.Unregister;
        checkBox1.Text = Language.Admin;
    }
}

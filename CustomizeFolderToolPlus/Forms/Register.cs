using CustomizeFolderToolPlus.Common;
using CustomizeFolderToolPlus.Common.Languages.Register;
using System;
using System.Windows.Forms;

namespace CustomizeFolderToolPlus.Forms;

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

    private async void button1_Click(object sender, EventArgs e)
    {
        this.panel2.Visible = true;
        var register = new RegistryManager();
        await register.Add(Language);
        CheckRegisterState();
        this.panel2.Visible = false;
    }

    private async void button2_Click(object sender, EventArgs e)
    {
        this.panel2.Visible = true;
        var register = new RegistryManager();
        await register.Delete();
        CheckRegisterState();
        this.panel2.Visible = false;
    }

    private void CheckRegisterState()
    {
        switch (RegistryManager.CheckRegisterState())
        {
            case RegistryManager.RegisterState.User:
                button1.Enabled = false;
                button2.Enabled = true;
                break;
            case RegistryManager.RegisterState.None:
            default:
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
    }
}

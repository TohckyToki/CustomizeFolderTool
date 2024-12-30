using ToolRegister.Languages;

namespace ToolRegister;

public partial class Form1 : Form
{
    private ILanguage Language = new English();

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
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
        var register = checkBox1.Checked ? new Register(true) : new Register(false);
        register.Add(Language);
        CheckRegisterState();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        var register = checkBox1.Checked ? new Register(true) : new Register(false);
        register.Delete();
        CheckRegisterState();
    }

    private void CheckRegisterState()
    {
        switch (Register.CheckRegisterState())
        {
            case Register.RegisterState.User:
            case Register.RegisterState.Admin:
                checkBox1.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = true;
                break;
            case Register.RegisterState.None:
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

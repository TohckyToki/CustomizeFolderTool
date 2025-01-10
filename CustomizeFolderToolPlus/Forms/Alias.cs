using CustomizeFolderToolPlus.Common.Languages.Tool;
using CustomizeFolderToolPlus.Interfaces;

namespace CustomizeFolderToolPlus.Forms;

public partial class Alias : Form, IFormBase
{
    public string FolderPath { get; set; } = default!;
    public ILanguage Language { get; set; } = default!;
    public Alias()
    {
        InitializeComponent();
    }

    private void Alias_Load(object sender, EventArgs e)
    {
        this.Text = Language.AliasTitle;
        this.label1.Text = Language.AliasMessage;
        this.button1.Text = Language.ConfirmText;
        this.button2.Text = Language.CancelText;
        textBox1.Focus();
    }

    private void TextBox1_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            SaveAlias(textBox1.Text);
        }
    }

    private void Button1_Click(object sender, EventArgs e)
    {
        SaveAlias(textBox1.Text);
    }

    private void Button2_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void SaveAlias(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            textBox1.Focus();
            return;
        }
        FolderTool.Create(this.FolderPath).CreateAlias(alias);
        this.Close();
    }
}

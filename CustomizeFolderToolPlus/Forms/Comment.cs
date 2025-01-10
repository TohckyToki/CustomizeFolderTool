using CustomizeFolderToolPlus.Interfaces;
using ToolLib.Languages.Tool;

namespace CustomizeFolderToolPlus.Forms;

public partial class Comment : Form, IFormBase
{
    public string FolderPath { get; set; } = null!;
    public ILanguage Language { get; set; } = null!;

    public Comment()
    {
        InitializeComponent();
    }

    private void Comment_Load(object sender, EventArgs e)
    {
        this.Text = Language.CommentTitle;
        this.label1.Text = Language.CommentMessage;
        this.button1.Text = Language.ConfirmText;
        this.button2.Text = Language.CancelText;
        textBox1.Focus();
    }

    private void TextBox1_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            SaveComment(textBox1.Text);
        }
    }

    private void Button1_Click(object sender, EventArgs e)
    {
        SaveComment(textBox1.Text);
    }

    private void Button2_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void SaveComment(string remarks)
    {
        if (string.IsNullOrWhiteSpace(remarks))
        {
            textBox1.Focus();
            return;
        }
        FolderTool.Create(this.FolderPath).CreateComment(remarks);
        this.Close();
    }
}

using ToolLib.Languages.Tool;

namespace CustomizeFolderToolPlus;

public partial class Comment : Form, IBaseForm
{
    public string? FolderPath { get; set; }
    public ILanguage? Language { get; set; }

    public Comment()
    {
        InitializeComponent();
    }

    private void Comment_Load(object sender, EventArgs e)
    {
        if (Language != null)
        {
            this.Text = Language.CommentTitle;
            this.label1.Text = Language.CommentMessage;
            this.button1.Text = Language.ConfirmText;
            this.button2.Text = Language.CancelText;
        }
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
        if (string.IsNullOrEmpty(remarks))
        {
            textBox1.Focus();
            return;
        }
        FolderTool.CreateDesktopFile(this.FolderPath!).CreateComment(remarks).Save();
        this.Close();
    }
}

using CustomizeFolderToolPlus.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomizeFolderToolPlus.Forms
{
    public partial class Remarks : Form, IBaseForm
    {
        public string? FolderPath { get; set; }
        public ILanguage? Language { get; set; }

        public Remarks()
        {
            InitializeComponent();
        }

        private void Remarks_Load(object sender, EventArgs e)
        {
            if (Language != null)
            {
                this.Text = Language.RemarksTitle;
                this.label1.Text = Language.RemarksMessage;
                this.button1.Text = Language.ConfirmText;
                this.button2.Text = Language.CancelText;
            }
            textBox1.Focus();
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SaveRemarks(textBox1.Text);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SaveRemarks(textBox1.Text);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveRemarks(string remarks)
        {
            if (string.IsNullOrEmpty(remarks))
            {
                textBox1.Focus();
                return;
            }
            FolderTool.CreateDesktopFile(this.FolderPath!).CreateRemarks(remarks).Save();
            this.Close();
        }
    }
}

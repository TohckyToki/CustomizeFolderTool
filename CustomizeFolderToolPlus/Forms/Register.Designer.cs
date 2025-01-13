using System.Drawing;
using System.Windows.Forms;

namespace CustomizeFolderToolPlus.Forms;

partial class Register
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Register));
        this.button1 = new Button();
        this.button2 = new Button();
        this.label1 = new Label();
        this.comboBox1 = new ComboBox();
        this.panel1 = new Panel();
        this.panel2 = new Panel();
        this.progressBar1 = new ProgressBar();
        this.panel1.SuspendLayout();
        this.panel2.SuspendLayout();
        this.SuspendLayout();
        // 
        // button1
        // 
        this.button1.Location = new Point(62, 40);
        this.button1.Name = "button1";
        this.button1.Size = new Size(80, 23);
        this.button1.TabIndex = 0;
        this.button1.Text = "Register";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += this.button1_Click;
        // 
        // button2
        // 
        this.button2.Location = new Point(62, 70);
        this.button2.Name = "button2";
        this.button2.Size = new Size(80, 23);
        this.button2.TabIndex = 1;
        this.button2.Text = "Unregister";
        this.button2.UseVisualStyleBackColor = true;
        this.button2.Click += this.button2_Click;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new Point(12, 9);
        this.label1.Name = "label1";
        this.label1.Size = new Size(59, 15);
        this.label1.TabIndex = 2;
        this.label1.Text = "Language";
        // 
        // comboBox1
        // 
        this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        this.comboBox1.FormattingEnabled = true;
        this.comboBox1.Items.AddRange(new object[] { "English", "中文" });
        this.comboBox1.Location = new Point(77, 6);
        this.comboBox1.Name = "comboBox1";
        this.comboBox1.Size = new Size(115, 23);
        this.comboBox1.TabIndex = 3;
        this.comboBox1.SelectedIndexChanged += this.comboBox1_SelectedIndexChanged;
        // 
        // panel1
        // 
        this.panel1.Controls.Add(this.button2);
        this.panel1.Controls.Add(this.comboBox1);
        this.panel1.Controls.Add(this.button1);
        this.panel1.Controls.Add(this.label1);
        this.panel1.Dock = DockStyle.Fill;
        this.panel1.Location = new Point(0, 0);
        this.panel1.Name = "panel1";
        this.panel1.Size = new Size(204, 101);
        this.panel1.TabIndex = 4;
        // 
        // panel2
        // 
        this.panel2.Controls.Add(this.progressBar1);
        this.panel2.Dock = DockStyle.Fill;
        this.panel2.Location = new Point(0, 0);
        this.panel2.Name = "panel2";
        this.panel2.Size = new Size(204, 101);
        this.panel2.TabIndex = 5;
        this.panel2.Visible = false;
        // 
        // progressBar1
        // 
        this.progressBar1.Location = new Point(12, 35);
        this.progressBar1.Name = "progressBar1";
        this.progressBar1.Size = new Size(180, 20);
        this.progressBar1.Style = ProgressBarStyle.Marquee;
        this.progressBar1.TabIndex = 4;
        this.progressBar1.UseWaitCursor = true;
        // 
        // Register
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(204, 101);
        this.Controls.Add(this.panel2);
        this.Controls.Add(this.panel1);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.Icon = (Icon)resources.GetObject("$this.Icon");
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "Register";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "ToolRegister";
        this.Load += this.Register_Load;
        this.panel1.ResumeLayout(false);
        this.panel1.PerformLayout();
        this.panel2.ResumeLayout(false);
        this.ResumeLayout(false);
    }

    #endregion

    private Button button1;
    private Button button2;
    private Label label1;
    private ComboBox comboBox1;
    private Panel panel1;
    private Panel panel2;
    private ProgressBar progressBar1;
}

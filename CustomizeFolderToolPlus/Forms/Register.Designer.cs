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
        // Register
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(204, 101);
        this.Controls.Add(this.comboBox1);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.button1);
        this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        this.Icon = (Icon)resources.GetObject("$this.Icon");
        this.Name = "Register";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "ToolRegister";
        this.Load += this.Register_Load;
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private Button button1;
    private Button button2;
    private Label label1;
    private ComboBox comboBox1;
}

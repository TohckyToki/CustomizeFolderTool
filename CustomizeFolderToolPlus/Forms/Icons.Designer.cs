namespace CustomizeFolderToolPlus.Forms;

partial class Icons
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.panel1 = new Panel();
        this.toolTip1 = new ToolTip(this.components);
        this.SuspendLayout();
        // 
        // panel1
        // 
        this.panel1.Dock = DockStyle.Fill;
        this.panel1.Location = new Point(0, 0);
        this.panel1.Name = "panel1";
        this.panel1.Size = new Size(380, 232);
        this.panel1.TabIndex = 0;
        // 
        // Icons
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(380, 232);
        this.Controls.Add(this.panel1);
        this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        this.Name = "Icons";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Icons";
        this.Load += this.Icons_Load;
        this.ResumeLayout(false);
    }

    #endregion

    private Panel panel1;
    private ToolTip toolTip1;
}
﻿using System.Drawing;
using System.Windows.Forms;

namespace CustomizeFolderToolPlus.Forms;

partial class Alias
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Alias));
        this.label1 = new Label();
        this.textBox1 = new TextBox();
        this.button2 = new Button();
        this.button1 = new Button();
        this.SuspendLayout();
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new Point(12, 9);
        this.label1.Name = "label1";
        this.label1.Size = new Size(38, 15);
        this.label1.TabIndex = 0;
        this.label1.Text = "label1";
        // 
        // textBox1
        // 
        this.textBox1.Location = new Point(12, 27);
        this.textBox1.Name = "textBox1";
        this.textBox1.Size = new Size(200, 23);
        this.textBox1.TabIndex = 1;
        this.textBox1.KeyDown += this.TextBox1_KeyDown;
        // 
        // button2
        // 
        this.button2.Location = new Point(137, 61);
        this.button2.Name = "button2";
        this.button2.Size = new Size(75, 23);
        this.button2.TabIndex = 3;
        this.button2.Text = "button2";
        this.button2.UseVisualStyleBackColor = true;
        this.button2.Click += this.Button2_Click;
        // 
        // button1
        // 
        this.button1.Location = new Point(12, 61);
        this.button1.Name = "button1";
        this.button1.Size = new Size(75, 23);
        this.button1.TabIndex = 2;
        this.button1.Text = "button1";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += this.Button1_Click;
        // 
        // Alias
        // 
        this.AutoScaleDimensions = new SizeF(7F, 15F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(224, 96);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.textBox1);
        this.Controls.Add(this.label1);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.Icon = (Icon)resources.GetObject("$this.Icon");
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "Alias";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "Form1";
        this.Load += this.Alias_Load;
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private Label label1;
    private TextBox textBox1;
    private Button button2;
    private Button button1;
}

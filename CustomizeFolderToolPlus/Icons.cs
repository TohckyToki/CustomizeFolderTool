using CustomizeFolderToolPlus.Languages;
using System.Drawing.Imaging;

namespace CustomizeFolderToolPlus;

public partial class Icons : Form, IBaseForm
{
    public string? FolderPath { get; set; }
    public ILanguage? Language { get; set; }
    public Icons()
    {
        InitializeComponent();
    }

    private void Icons_Load(object sender, EventArgs e)
    {
        if (Language != null)
        {
            this.Text = Language.IconTitle;
        }

        var panel = this.panel1;

        var addButton = new Func<Icon, string, int, int, Button>((icon, tooltip, left, top) => {
            var button = new Button
            {
                BackgroundImage = new Icon(icon, new Size(256, 256)).ToBitmap(),
                Margin = new Padding(left, top, 0, 0),
            };
            this.toolTip1.SetToolTip(button, tooltip);
            panel.Controls.Add(button);
            return button;
        });

        addButton(Resources.Default, "默认", 10, 10).Click += Default_Click;
        addButton(Properties.Resources.Color00, "颜色1", 84, 10).Click += Colors_Click;
        addButton(Properties.Resources.Color01, "颜色2", 158, 10).Click += Colors_Click;
        addButton(Properties.Resources.Color02, "颜色3", 232, 10).Click += Colors_Click;
        addButton(Properties.Resources.Color03, "颜色4", 306, 10).Click += Colors_Click;
        addButton(Properties.Resources.Color04, "颜色5", 10, 84).Click += Colors_Click;
        addButton(Properties.Resources.Color05, "颜色6", 84, 84).Click += Colors_Click;
        addButton(Properties.Resources.Color06, "颜色7", 158, 84).Click += Colors_Click;
        addButton(Properties.Resources.Color07, "颜色8", 232, 84).Click += Colors_Click;
        addButton(Properties.Resources.Color08, "颜色9", 306, 84).Click += Colors_Click;
        addButton(Properties.Resources.Color09, "颜色10", 10, 158).Click += Colors_Click;
        addButton(Properties.Resources.Color10, "颜色11", 84, 158).Click += Colors_Click;
        addButton(Properties.Resources.Color11, "颜色12", 158, 158).Click += Colors_Click;
        addButton(Properties.Resources.Color12, "颜色13", 232, 158).Click += Colors_Click;
        addButton(Properties.Resources.Plus, "自定义", 306, 158).Click += Customize_Click;
    }

}

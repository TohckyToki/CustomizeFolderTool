using CustomizeFolderToolPlus.Properties;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ToolLib.Languages.Tool;

namespace CustomizeFolderToolPlus;

public partial class Icons : Form, IBaseForm
{
    public string? FolderPath { get; set; }
    public ILanguage? Language { get; set; }

    public Icons()
    {
        this.InitializeComponent();
    }

    private void Icons_Load(object sender, EventArgs e)
    {
        if (this.Language != null)
        {
            this.Text = this.Language.IconTitle;
        }

        this.AddButton(Resources.IconDefault, this.Language!.IconDefault, 10, 10).Click += this.Default_Click;
        this.AddButton(Resources.Icon01, this.Language.Icon01, 84, 10).Click += this.Colors_Click;
        this.AddButton(Resources.Icon02, this.Language.Icon02, 158, 10).Click += this.Colors_Click;
        this.AddButton(Resources.Icon03, this.Language.Icon03, 232, 10).Click += this.Colors_Click;
        this.AddButton(Resources.Icon04, this.Language.Icon04, 306, 10).Click += this.Colors_Click;
        this.AddButton(Resources.Icon05, this.Language.Icon05, 10, 84).Click += this.Colors_Click;
        this.AddButton(Resources.Icon06, this.Language.Icon06, 84, 84).Click += this.Colors_Click;
        this.AddButton(Resources.Icon07, this.Language.Icon07, 158, 84).Click += this.Colors_Click;
        this.AddButton(Resources.Icon08, this.Language.Icon08, 232, 84).Click += this.Colors_Click;
        this.AddButton(Resources.Icon09, this.Language.Icon09, 306, 84).Click += this.Colors_Click;
        this.AddButton(Resources.Icon10, this.Language.Icon10, 10, 158).Click += this.Colors_Click;
        this.AddButton(Resources.Icon11, this.Language.Icon11, 84, 158).Click += this.Colors_Click;
        this.AddButton(Resources.Icon12, this.Language.Icon12, 158, 158).Click += this.Colors_Click;
        this.AddButton(Resources.Icon13, this.Language.Icon13, 232, 158).Click += this.Colors_Click;
        this.AddButton(Resources.IconAdd, this.Language.IconAdd, 306, 158).Click += this.Customize_Click;

        //Modify icon of last button.
        var newicon = new Icon(Resources.IconAdd, new Size(256, 256));
        var bitmap = this.ResizeImage(newicon.ToBitmap(), 40, 40);
        ((Button)this.panel1.Controls[this.panel1.Controls.Count - 1]).BackgroundImage = bitmap;
    }

    private Button AddButton(Icon icon, string tooltip, int left, int top)
    {
        var panel = this.panel1;

        var button = new Button
        {
            BackgroundImage = new Icon(icon, new Size(256, 256)).ToBitmap(),
            Margin = new Padding(),
            Location = new Point(left, top),
            Size = new Size(64, 64),
            Cursor = Cursors.Hand,
            BackgroundImageLayout = ImageLayout.Center,
            Tag = icon,
        };
        this.toolTip1.SetToolTip(button, tooltip);
        panel.Controls.Add(button);
        return button;
    }

    private void Default_Click(object? sender, EventArgs e)
    {
        FolderTool.CreateDesktopFile(this.FolderPath!).DeleteIcon().Save();
        this.Close();
    }

    private void Colors_Click(object? sender, EventArgs e)
    {
        var icon = (Icon)((Button)sender!).Tag!;

        //string iconPath = this.GenerateNewIconFileName();
        //var fs = File.Create(iconPath);
        //new Icon(icon, new Size(256, 256)).Save(fs);
        //fs.Close();
        //new FileInfo(iconPath)
        //{
        //    Attributes = FileAttributes.Archive | FileAttributes.Hidden | FileAttributes.System
        //}.Refresh();
        //FolderTool.CreateDesktopFile(this.FolderPath!).CreateIcon(iconPath).Save();

        var ms = new MemoryStream();
        new Icon(icon, new Size(256, 256)).Save(ms);
        var data = ms.ToArray();
        ms.Close();
        FolderTool.CreateDesktopFile(this.FolderPath!).CreateIconWithResource(data).Save();

        this.Close();
    }

    private void Customize_Click(object? sender, EventArgs e)
    {
        var ofd = new OpenFileDialog
        {
            Title = this.Language!.IconAddTitle,
            Filter = $"{this.Language.IconAddIcoFilter}|*.ico|{this.Language.IconAddPngFilter}|*.png",
            DefaultExt = "*.ico",
            CheckFileExists = true,
            CheckPathExists = true,
            Multiselect = false,
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            FileName = default,
        };
        ofd.FileOk += new System.ComponentModel.CancelEventHandler((obj, args) =>
        {
            if (!string.IsNullOrWhiteSpace(ofd.FileName))
            {
                var icon = Path.GetExtension(ofd.FileName) switch
                {
                    ".ico" => new Icon(ofd.FileName!),
                    ".png" => Icon.FromHandle(((Bitmap)Image.FromFile(ofd.FileName!)).GetHicon()),
                    _ => null,
                };
                if (icon != null)
                {
                    if (icon.Width > 256 || icon.Height > 256 || icon.Width < 32 || icon.Height < 32)
                    {
                        args.Cancel = true;
                        MessageBox.Show(this.Language.IconAddWarningMessage, this.Language.IconAddWarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        });
        if (ofd.ShowDialog(this) == DialogResult.OK)
        {
            var icon = Path.GetExtension(ofd.FileName) switch
            {
                ".ico" => new Icon(ofd.FileName!),
                ".png" => this.IconFromImage(Image.FromFile(ofd.FileName!)),
                _ => null,
            };
            if (icon != null)
            {
                //var iconPath = this.GenerateNewIconFileName();
                //var fs = File.Create(iconPath);
                //icon.Save(fs);
                //fs.Close();
                //new FileInfo(iconPath)
                //{
                //    Attributes = FileAttributes.Archive | FileAttributes.Hidden | FileAttributes.System
                //}.Refresh();
                //FolderTool.CreateDesktopFile(this.FolderPath!).CreateIcon(iconPath).Save();

                var ms = new MemoryStream();
                icon.Save(ms);
                var data = ms.ToArray();
                ms.Close();
                FolderTool.CreateDesktopFile(this.FolderPath!).CreateIconWithResource(data).Save();

                this.Close();
            }
        }

    }

    private string GenerateNewIconFileName()
    {
        string iconPath;
        do
        {
            iconPath = Path.Combine(this.FolderPath!, Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".ico");
        } while (File.Exists(iconPath));
        return iconPath;
    }

    private Bitmap ResizeImage(Image image, int width, int height)
    {
        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using (var graphics = Graphics.FromImage(destImage))
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }

        return destImage;
    }

    private Icon IconFromImage(Image img)
    {
        var ms = new MemoryStream();
        var bw = new BinaryWriter(ms);
        // Header
        bw.Write((short)0);   // 0 : reserved
        bw.Write((short)1);   // 2 : 1=ico, 2=cur
        bw.Write((short)1);   // 4 : number of images
                              // Image directory
        var w = img.Width;
        if (w >= 256) w = 0;
        bw.Write((byte)w);    // 0 : width of image
        var h = img.Height;
        if (h >= 256) h = 0;
        bw.Write((byte)h);    // 1 : height of image
        bw.Write((byte)0);    // 2 : number of colors in palette
        bw.Write((byte)0);    // 3 : reserved
        bw.Write((short)0);   // 4 : number of color planes
        bw.Write((short)0);   // 6 : bits per pixel
        var sizeHere = ms.Position;
        bw.Write(0);     // 8 : image size
        var start = (int)ms.Position + 4;
        bw.Write(start);      // 12: offset of image data
                              // Image data
        img.Save(ms, ImageFormat.Png);
        var imageSize = (int)ms.Position - start;
        ms.Seek(sizeHere, SeekOrigin.Begin);
        bw.Write(imageSize);
        ms.Seek(0, SeekOrigin.Begin);

        // And load it
        return new Icon(ms);
    }

}

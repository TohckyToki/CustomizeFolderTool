using CustomizeFolderTool.Util;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomizeFolderTool.Forms {
    /// <summary>
    /// Interaction logic for Color.xaml
    /// </summary>
    public partial class Color : Window {
        public static string folderPath;

        public Color() {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e) {
            var grid = ((Grid)sender);

            var addButton = new Func<Icon, string, int, int, Button>((icon, tooltip, left, top) => {
                var button = new Button {
                    Background = new ImageBrush(Imaging.CreateBitmapSourceFromHIcon(new Icon(icon, new System.Drawing.Size(256, 256)).ToBitmap().GetHicon(), Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())),
                    Margin = new Thickness(left, top, 0, 0),
                    ToolTip = tooltip,
                };
                grid.Children.Add(button);
                return button;
            });

            addButton(Properties.Resources.Default, "默认", 10, 10).Click += Default_Click;
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

            //Modify icon of last button.
            var newicon = new Icon(Properties.Resources.Plus, new System.Drawing.Size(256, 256));
            var bitmap = ResizeImage(newicon.ToBitmap(), 40, 40);
            var imagebrush = new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())) {
                TileMode = TileMode.None,
                Stretch = Stretch.None,
                Opacity = 0.6
            };
            ((Button)grid.Children[grid.Children.Count - 1]).Background = imagebrush;
        }

        private void Default_Click(object sender, RoutedEventArgs e) {
            Desktop.CreateDesktopFile(folderPath).DeleteIcon().Save();
            Application.Current.Shutdown();
        }

        private void Colors_Click(object sender, RoutedEventArgs e) {
            var num = Convert.ToInt32(((Button)sender).ToolTip.ToString().Replace("颜色", "")) - 1;
            var icon = (Icon)Properties.Resources.ResourceManager.GetObject("Color" + num.ToString().PadLeft(2, '0'));

            string iconPath = GenerateNewIconFileName();

            new Icon(icon, new System.Drawing.Size(256, 256)).Save(File.Create(iconPath));
            var fileInfo = new FileInfo(iconPath) {
                Attributes = FileAttributes.Archive | FileAttributes.Hidden | FileAttributes.System
            };
            fileInfo.Refresh();
            Desktop.CreateDesktopFile(folderPath).CreateIcon(iconPath).Save();
            Application.Current.Shutdown();
        }

        private void Customize_Click(object sender, RoutedEventArgs e) {
            var ofd = new OpenFileDialog {
                Title = "请选择图片",
                Filter = "ICO文件|*.ico|PNG文件|*.png",
                DefaultExt = "*.ico",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = default,
            };
            ofd.FileOk += new System.ComponentModel.CancelEventHandler((obj, args) => {
                Icon icon;
                switch (Path.GetExtension(ofd.FileName)) {
                    case ".ico":
                        icon = new Icon(ofd.FileName);
                        break;
                    case ".png":
                        icon = System.Drawing.Icon.FromHandle(((Bitmap)System.Drawing.Image.FromFile(ofd.FileName)).GetHicon());
                        break;
                    default:
                        icon = null;
                        break;
                }
                if (icon.Width > 256 || icon.Height > 256 || icon.Width < 32 || icon.Height < 32) {
                    args.Cancel = true;
                    MessageBox.Show("请选择长宽像素在32到256之间的图片", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
            if (ofd.ShowDialog(this) ?? false) {
                Icon icon;
                switch (Path.GetExtension(ofd.FileName)) {
                    case ".ico":
                        icon = new Icon(ofd.FileName);
                        break;
                    case ".png":
                        icon = System.Drawing.Icon.FromHandle(((Bitmap)System.Drawing.Image.FromFile(ofd.FileName)).GetHicon());
                        break;
                    default:
                        icon = null;
                        break;
                }
                var iconPath = GenerateNewIconFileName();
                using (FileStream stream = File.OpenWrite(iconPath)) {
                    icon?.Save(stream);
                    new FileInfo(iconPath) {
                        Attributes = FileAttributes.Archive | FileAttributes.Hidden | FileAttributes.System
                    }.Refresh();
                    Desktop.CreateDesktopFile(folderPath).CreateIcon(iconPath).Save();
                }
                Application.Current.Shutdown();
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Process.GetCurrentProcess().Kill();
        }

        private string GenerateNewIconFileName() {
            string iconPath;
            do {
                iconPath = Path.Combine(folderPath, Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".ico");
            } while (File.Exists(iconPath));
            return iconPath;
        }

        private Bitmap ResizeImage(System.Drawing.Image image, int width, int height) {
            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage)) {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes()) {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}

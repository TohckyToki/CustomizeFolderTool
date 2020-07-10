using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CustomizeFolderTool.Forms {
    /// <summary>
    /// Interaction logic for Color.xaml
    /// </summary>
    public partial class Color : Window {
        public Color() {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e) {
            var addButton = new Action<Icon, string, int, int>((icon, tooltip, left, top) => {
                ((Grid)sender).Children.Add(new Button {
                    Background = new ImageBrush(Imaging.CreateBitmapSourceFromHIcon(new Icon(icon, new System.Drawing.Size(256, 256)).ToBitmap().GetHicon(), Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())),
                    Margin = new Thickness(left, top, 0, 0),
                    ToolTip = tooltip
                });
            });

            addButton(Properties.Resources.Default, "默认", 10, 10);
            addButton(Properties.Resources.Color00, "颜色1", 84, 10);
            addButton(Properties.Resources.Color01, "颜色2", 158, 10);
            addButton(Properties.Resources.Color02, "颜色3", 232, 10);
            addButton(Properties.Resources.Color03, "颜色4", 306, 10);
            addButton(Properties.Resources.Color04, "颜色5", 10, 84);
            addButton(Properties.Resources.Color05, "颜色6", 84, 84);
            addButton(Properties.Resources.Color06, "颜色7", 158, 84);
            addButton(Properties.Resources.Color07, "颜色8", 232, 84);
            addButton(Properties.Resources.Color08, "颜色9", 306, 84);
            addButton(Properties.Resources.Color09, "颜色10", 10, 158);
            addButton(Properties.Resources.Color10, "颜色11", 84, 158);
            addButton(Properties.Resources.Color11, "颜色12", 158, 158);
            addButton(Properties.Resources.Color12, "颜色13", 232, 158);
            addButton(Properties.Resources.Plus, "自定义", 306, 158);
        }
    }
}

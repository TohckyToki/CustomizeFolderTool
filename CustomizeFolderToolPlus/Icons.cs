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
    public partial class Icons : Form, IBaseForm
    {
        public string? FolderPath { get; set; }
        public Icons()
        {
            InitializeComponent();
        }

    }
}

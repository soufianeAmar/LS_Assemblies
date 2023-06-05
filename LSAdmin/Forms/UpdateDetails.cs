using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LSAdmin
{
    public partial class UpdateDetails : Form
    {
        public UpdateDetails()
        {
            InitializeComponent();
            string url = LSAdmin.Core.GetApplicationPath() + "\\update.html";
            webBrowser1.Url = new Uri(url);
        }

        public bool ShowUpdateDetails
        {
            get { return cbShowUpdateDetails.Checked; }
        }
    }
}

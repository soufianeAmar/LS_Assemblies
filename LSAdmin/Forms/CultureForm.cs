using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChequeAdmin.Module
{
    public partial class CultureForm : Form
    {
        public string SelectedLanguage
        {
            get { return imageComboBoxEdit1.EditValue.ToString(); }
        }

        public CultureForm()
        {
            InitializeComponent();
        }
    }
}

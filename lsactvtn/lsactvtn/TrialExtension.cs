using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lsactvtn
{
    public partial class TrialExtension : Form
    {
        public string trialExtensionString
        {
            get { return teExtensionString.Text; }
        }

        public TrialExtension()
        {
            InitializeComponent();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            try
            {
                ActivationClass.ta.ExtendTrial(trialExtensionString, ActivationClass.verifiedTrialFlag);
                ActivationClass.Demo = true;
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur d'extension de la Démo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tError extending Demo : {1}", DateTime.Now, ex));
            }
        }
    }
}

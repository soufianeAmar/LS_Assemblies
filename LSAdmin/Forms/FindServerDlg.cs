using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace LSAdmin
{
    public partial class FindServerDlg : Form
    {
        public string ServerName
        {
            get { return teServerName.Text; }
            set { teServerName.Text = value; }
        }
        public string InstanceName
        {
            get
            {
                if (!string.IsNullOrEmpty(teInstanceName.Text))
                    return string.Format(@"\{0}", teInstanceName.Text);
                else
                    return string.Empty;
            }
            set { teInstanceName.Text = value; }
        }

        public FindServerDlg()
        {
            InitializeComponent();
        }
        public FindServerDlg(string serverName, string instanceName)
        {
            InitializeComponent();
            ServerName = serverName;
            InstanceName = instanceName;
        }

        private void teServerName_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            ServerListDlg sld = new ServerListDlg();
            if (sld.ShowDialog() == DialogResult.OK)
            {
                ServerName = ServerListDlg.serverName;
                InstanceName = ServerListDlg.instanceName;
            }
            //FolderBrowserDialog fbd = new FolderBrowserDialog() { ShowNewFolderButton = false, Description = "Recherche du serveur..." };
            //SetRootFolder(fbd, (Environment.SpecialFolder)18);
            //fbd.ShowDialog();
        }
        private void SetRootFolder(FolderBrowserDialog fbd, Environment.SpecialFolder sf)
        {
            Type t = fbd.GetType();
            FieldInfo fi = t.GetField("rootFolder", BindingFlags.NonPublic | BindingFlags.Instance);
            fi.SetValue(fbd, sf);
        }

        private void aConfig_Click(object sender, EventArgs e)
        {
            ConfigServer config = new ConfigServer();
            config.ShowDialog();
        }
    }
}

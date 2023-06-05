using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LSAdmin
{
    public partial class ServerListDlg : Form
    {
        public static string serverName;
        public static string instanceName;

        public ServerListDlg()
        {
            InitializeComponent();
            Thread t = new Thread(getSQLServers);
            t.Start();
        }

        public delegate void ThreadingAction();

        public void getSQLServers()
        {
            Invoke(new ThreadingAction(() =>
            {
                progressPanel1.Visible = true;
                simpleButton1.Enabled = false;
            }));
            DataTable tSQLServerList = LSAdmin.Core.GetAvailableSQLServers();
            Invoke(new ThreadingAction(() =>
            {
                foreach (DataRow dr in tSQLServerList.Rows)
                {
                    ListViewItem lvi = lvComputers.Items.Add(string.Format("{0}\n({1})", dr["ServerName"], dr["InstanceName"]), 0);
                    lvi.SubItems.Add(dr["ServerName"].ToString());
                    lvi.SubItems.Add(dr["InstanceName"].ToString());
                    bOk.Enabled = lvComputers.SelectedItems.Count > 0;
                }
                progressPanel1.Visible = false;
                simpleButton1.Enabled = true;
            }));
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            serverName = lvComputers.SelectedItems[0].SubItems[1].Text;
            instanceName = lvComputers.SelectedItems[0].SubItems[2].Text;
        }

        private void lvComputers_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            bOk.Enabled = lvComputers.SelectedItems.Count > 0;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            lvComputers.Items.Clear();
            Thread t = new Thread(getSQLServers);
            t.Start();
        }
    }
}

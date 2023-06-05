using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using DevExpress.Xpo;

namespace LSAdmin
{
    [NonPersistent]
    public class ExcelReader
    {
        private OleDbConnection CNX;
        public string Excel_filename;
        public String [] sheet_names;
        public String[] column_names;
        public bool header = true;
        public bool mixed_data=false;
        public string connection_string
        {
            get
            {
                string HDR;
                if (header)
                    HDR = "HDR=Yes;";
                else
                    HDR = "HDR=No;";
                string IMEX;
                if (mixed_data)
                    IMEX = "Imex=2;";
                else
                    IMEX = "Imex=1;";
                return String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Excel 8.0;{1}{2}", Excel_filename, HDR, IMEX);
            }
        }

        public ExcelReader()
        {
            CNX = new OleDbConnection();
        }

        public String[] GetSheetNames()
        {
            try
            {
                if (CNX.State == ConnectionState.Closed)
                    throw new Exception("Impossible d'obtenir la liste des feuilles de calcul. connection fermée !");
                //DataTable temporary_table = CNX.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                DataTable temporary_table = CNX.GetSchema("Tables");
                if (temporary_table == null)
                    return null;
                String[] sheet_names = new String[temporary_table.Rows.Count];
                int i = 0;
                foreach (DataRow row in temporary_table.Rows)
                {
                    sheet_names[i] = row["TABLE_NAME"].ToString();
                    i++;
                }
                return sheet_names;
            }
            catch
            {
                return null;
            }
        }

        public String[] GetColumnNames()
        {
            try
            {
                if (CNX.State == ConnectionState.Closed)
                    throw new Exception("Impossible d'obtenir la liste des feuilles de calcul. connection fermée !");
                DataTable temporary_table = CNX.GetSchema("Columns");
                if (temporary_table == null)
                    return null;
                String[] column_names = new String[temporary_table.Rows.Count];
                int i = 0;
                foreach (DataRow row in temporary_table.Rows)
                {
                    column_names[i] = row["COLUMN_NAME"].ToString();
                    i++;
                }
                return column_names;
            }
            catch
            {
                return null;
            }
        }

        public String[] GetColumnNames(string sheet_name)
        {
            try
            {
                if (CNX.State == ConnectionState.Closed)
                    throw new Exception("Impossible d'obtenir la liste des feuilles de calcul. connection fermée !");
                DataTable ExcelTable = new DataTable();
                string select_command;
                select_command = string.Format("select * from [{0}]", sheet_name);
                OleDbDataAdapter oleAdapter = new OleDbDataAdapter(new OleDbCommand(select_command, CNX));
                oleAdapter.FillSchema(ExcelTable, SchemaType.Source);
                oleAdapter.Fill(ExcelTable);
                String[] column_names = new String[ExcelTable.Columns.Count];
                int i = 0;
                foreach (DataColumn column in ExcelTable.Columns)
                {
                    if (column.ColumnName != null)
                    {
                        column_names[i] = column.ColumnName;
                        i++;
                    }
                }
                return column_names;
            }
            catch
            {
                return null;
            }
        }

        public void Connect()
        {
            using (OpenFileDialog dialog = new OpenFileDialog { Filter = "Fichiers Microsoft Excel (*.xls, *.xlsx)|*.xls;*.xlsx|Tous les fichier (*.*)|*.*" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Excel_filename = dialog.FileName;
                    try
                    {
                        if (!System.IO.File.Exists(Excel_filename))
                            throw new Exception("Fichier spécifié introuvable !");
                        //CNX = new OleDbConnection(connection_string);
                        CNX.ConnectionString = connection_string;
                        CNX.Open();
                        sheet_names = GetSheetNames();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Erreur d'ouverture", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Aucun fichier sélectionné !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public bool Connected()
        {
            return (CNX.State == ConnectionState.Open);
        }

        public DataTable GetExcelTable(string sheet_name)
        {
            if (CNX.State == ConnectionState.Open)
            {
                DataTable ExcelTable = new DataTable(sheet_name);
                string select_command;
                select_command = string.Format("select * from [{0}]", sheet_name);
                OleDbDataAdapter oleAdapter = new OleDbDataAdapter(new OleDbCommand(select_command, CNX));
                oleAdapter.FillSchema(ExcelTable, SchemaType.Source);
                oleAdapter.Fill(ExcelTable);
                column_names = GetColumnNames();
                return ExcelTable;
            }
            else
                return null;
        }

        public DataTable GetExcelTable()
        {
            if (CNX.State == ConnectionState.Open)
            {
                DataTable ExcelTable = new DataTable();
                string select_command;
                select_command = string.Format("select * from [{0}]", sheet_names[0].ToString());
                OleDbDataAdapter oleAdapter = new OleDbDataAdapter(new OleDbCommand(select_command, CNX));
                oleAdapter.FillSchema(ExcelTable, SchemaType.Source);
                oleAdapter.Fill(ExcelTable);
                column_names = GetColumnNames();
                return ExcelTable;
            }
            else
                return null;
        }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LSAdmin
{
    public partial class ConfigServer : Form
    {
        public string id
        {
            get { return Helper.cryptage(textEdit2.Text, "EurekaNovoreka"); }
            set { textEdit2.Text = Helper.cryptage(value, "EurekaNovoreka"); }
        }

        public string password
        {
            get { return Helper.cryptage(textEdit1.Text, "EurekaNovoreka"); }
            set { textEdit1.Text = Helper.cryptage(value, "EurekaNovoreka"); }
        }

        public ConfigServer()
        {
            InitializeComponent();
        }

        private void aConfig_Click(object sender, EventArgs e)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["Password"].Value = password;
            config.AppSettings.Settings["Id"].Value = id;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);
            config.Save();
        }
        
        //string decryptage(string encrypt, string key)
        //{
        //    using (System.Security.Cryptography.TripleDESCryptoServiceProvider descrypt = new System.Security.Cryptography.TripleDESCryptoServiceProvider())
        //    {
        //        using (System.Security.Cryptography.MD5CryptoServiceProvider hashMD5Provider = new System.Security.Cryptography.MD5CryptoServiceProvider())
        //        {
        //            byte[] byteHash = hashMD5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
        //            descrypt.Key = byteHash;
        //            descrypt.Mode = System.Security.Cryptography.CipherMode.ECB;
        //            byte[] data = Convert.FromBase64String(encrypt);
        //            return Encoding.UTF8.GetString(descrypt.CreateDecryptor().TransformFinalBlock(data, 0, data.Length));
        //        }
        //    }
        //}


    }
}

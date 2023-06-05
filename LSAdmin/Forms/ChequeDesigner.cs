using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChequeAdmin.Module
{
    public partial class ChequeDesigner : Form
    {
        bool dragging = false;
        bool resizing = false;
        int X, Y;
        Rectangle dropRect;
        public Banque banque;

        float DpiX = 1;

        public ChequeDesigner()
        {
            InitializeComponent();
            dropRect = new Rectangle(0, 0, splitContainer1.Width, splitContainer1.Height);
            Graphics gr = Graphics.FromHwnd(IntPtr.Zero);
            gr.PageUnit = GraphicsUnit.Millimeter;
            DpiX = gr.DpiX;
        }

        public ChequeDesigner(Banque _banque)
        {
            InitializeComponent();
            dropRect = new Rectangle(0, 0, splitContainer1.Width, splitContainer1.Height);
            Graphics gr = Graphics.FromHwnd(IntPtr.Zero);
            gr.PageUnit = GraphicsUnit.Millimeter;
            DpiX = gr.DpiX;
            Font font;
            FontConverter fc = new FontConverter();
            banque = _banque;
            if (banque != null)
            {
                //Montant**************************************************************
                seMontantX.Text = banque.montantX.ToString();
                seMontantY.Text = banque.montantY.ToString();
                font = (Font)fc.ConvertFromInvariantString(banque.fontMontant);
                eFontMontant.Text = font.Name;
                seTailleMontant.Value = (decimal)font.Size;
                cbGrasMontant.Checked = font.Bold;
                cbItaliqueMontant.Checked = font.Italic;
                cbSoulignéMontant.Checked = font.Underline;
                ePréfixeMontant.Text = banque.préfixeMontant;
                eSuffixeMontant.Text = banque.suffixeMontant;
                //Montant en Lettres***************************************************
                seMntLttrX.Text = banque.montant_lettresX.ToString();
                seMntLttrY.Text = banque.montant_lettresY.ToString();
                seMntLttr2X.Text = banque.montant_lettres2X.ToString();
                seMntLttr2Y.Text = banque.montant_lettres2Y.ToString();
                font = (Font)fc.ConvertFromInvariantString(banque.fontMontant_lettres);
                eFontMntLttr.Text = font.Name;
                seTailleMntLttr.Value = (decimal)font.Size;
                cbGrasMntLttr.Checked = font.Bold;
                cbItaliqueMntLttr.Checked = font.Italic;
                cbSoulignéMntLttr.Checked = font.Underline;
                ePréfixeMntLttr.Text = banque.préfixeMntLttr;
                eSuffixeMntLttr.Text = banque.suffixeMntLttr;
                seLargeurMntLttr.Value = banque.largeurMntLttr;
                seLargeurMntLttr2.Value = banque.largeurMntLttr2;
                //Bénéficiaire*********************************************************
                seBénéficiaireX.Text = banque.bénéficiaireX.ToString();
                seBénéficiaireY.Text = banque.bénéficiaireY.ToString();
                font = (Font)fc.ConvertFromInvariantString(banque.fontBénéficiaire);
                eFontBénéficiaire.Text = font.Name;
                seTailleBénéficiaire.Value = (decimal)font.Size;
                cbGrasBénéficiaire.Checked = font.Bold;
                cbItaliqueBénéficiaire.Checked = font.Italic;
                cbSoulignéBénéficiaire.Checked = font.Underline;
                //Lieu*****************************************************************
                seLieuX.Text = banque.lieuX.ToString();
                seLieuY.Text = banque.lieuY.ToString();
                font = (Font)fc.ConvertFromInvariantString(banque.fontLieu);
                eFontLieu.Text = font.Name;
                seTailleLieu.Value = (decimal)font.Size;
                cbGrasLieu.Checked = font.Bold;
                cbItaliqueLieu.Checked = font.Italic;
                cbSoulignéLieu.Checked = font.Underline;
                //Date*****************************************************************
                seDateX.Text = banque.dateX.ToString();
                seDateY.Text = banque.dateY.ToString();
                font = (Font)fc.ConvertFromInvariantString(banque.fontDate);
                eFontDate.Text = font.Name;
                seTailleDate.Value = (decimal)font.Size;
                cbGrasDate.Checked = font.Bold;
                cbItaliqueDate.Checked = font.Italic;
                cbSoulignéDate.Checked = font.Underline;
                //NonEndossable********************************************************
                seNonEndossableX.Text = banque.nonEndossableX.ToString();
                seNonEndossableY.Text = banque.nonEndossableY.ToString();
                //*********************************************************************
                pChèque.BackgroundImage = _banque.modele;
                pChèque.Height = rulerControl2.ScaleValueToPixel(banque.largeur) / 10;
                pChèque.Width = rulerControl2.ScaleValueToPixel(banque.longueur) / 10;
            }
        }

        private void bAccepter_Click(object sender, EventArgs e)
        {
            banque.montantX = Convert.ToInt32(seMontantX.Text);
            banque.montantY = Convert.ToInt32(seMontantY.Text);
            banque.montant_lettresX = Convert.ToInt32(seMntLttrX.Text);
            banque.montant_lettresY = Convert.ToInt32(seMntLttrY.Text);
            banque.montant_lettres2X = Convert.ToInt32(seMntLttr2X.Text);
            banque.montant_lettres2Y = Convert.ToInt32(seMntLttr2Y.Text);
            banque.bénéficiaireX = Convert.ToInt32(seBénéficiaireX.Text);
            banque.bénéficiaireY = Convert.ToInt32(seBénéficiaireY.Text);
            banque.lieuX = Convert.ToInt32(seLieuX.Text);
            banque.lieuY = Convert.ToInt32(seLieuY.Text);
            banque.dateX = Convert.ToInt32(seDateX.Text);
            banque.dateY = Convert.ToInt32(seDateY.Text);
            banque.nonEndossableX = Convert.ToInt32(seNonEndossableX.Text);
            banque.nonEndossableY = Convert.ToInt32(seNonEndossableY.Text);
            FontConverter fc = new FontConverter();
            Font font = GetFont(eFontMontant.Text, (float)seTailleMontant.Value, cbGrasMontant.Checked, cbItaliqueMontant.Checked, cbSoulignéMontant.Checked);
            banque.fontMontant = fc.ConvertToInvariantString(font);
            font = GetFont(eFontMntLttr.Text, (float)seTailleMntLttr.Value, cbGrasMntLttr.Checked, cbItaliqueMntLttr.Checked, cbSoulignéMntLttr.Checked);
            banque.fontMontant_lettres = fc.ConvertToInvariantString(font);
            font = GetFont(eFontBénéficiaire.Text, (float)seTailleBénéficiaire.Value, cbGrasBénéficiaire.Checked, cbItaliqueBénéficiaire.Checked, cbSoulignéBénéficiaire.Checked);
            banque.fontBénéficiaire = fc.ConvertToInvariantString(font);
            font = GetFont(eFontLieu.Text, (float)seTailleLieu.Value, cbGrasLieu.Checked, cbItaliqueLieu.Checked, cbSoulignéLieu.Checked);
            banque.fontLieu = fc.ConvertToInvariantString(font);
            font = GetFont(eFontDate.Text, (float)seTailleDate.Value, cbGrasDate.Checked, cbItaliqueDate.Checked, cbSoulignéDate.Checked);
            banque.fontDate = fc.ConvertToInvariantString(font);
            banque.préfixeMontant = ePréfixeMontant.Text;
            banque.suffixeMontant = eSuffixeMontant.Text;
            banque.préfixeMntLttr = ePréfixeMntLttr.Text;
            banque.suffixeMntLttr = eSuffixeMntLttr.Text;
            banque.largeurMntLttr = (int)seLargeurMntLttr.Value;
            banque.largeurMntLttr2 = (int)seLargeurMntLttr2.Value;
            Close();
        }

        private void control_MouseDown(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            control.BringToFront();
            if (control is Label)
                ((Label)control).BorderStyle = BorderStyle.FixedSingle;
            if (control is PictureBox)
                ((PictureBox)control).BorderStyle = BorderStyle.FixedSingle;
            dragging = control.Width - e.X >= 5;
            resizing = (control.Width - e.X < 5) && (control.Name.StartsWith("cMntLttr"));
            X = e.X;
            Y = e.Y;
            PointToXY(control);
            foreach (Control c in pChèque.Controls)
            {
                if (c != control)
                {
                    if (c is Label) ((Label)c).BorderStyle = BorderStyle.None;
                    if (c is PictureBox) ((PictureBox)c).BorderStyle = BorderStyle.None;
                }
            }
        }

        private void control_MouseMove(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            if (dragging)
            {
                control.Left += e.X - X;
                control.Top += e.Y - Y;
                PointToXY(control);
                //X = e.X;
                //Y = e.Y;
            }
            else if (resizing)
            {
                control.Width = e.X + 5;
                Control[] controls = Controls.Find(string.Format("seLargeur{0}", control.Name.Substring(1)), true);
                if (controls.Length > 0)
                {
                    SpinEdit seLargeur = (SpinEdit)controls[0];
                    seLargeur.Value = (int)((decimal)(control.Width * 25.4) / (decimal)DpiX);
                }
            }
            else if ((control.Width - e.X < 5) && (control.Name.StartsWith("cMntLttr")))
            {
                control.Cursor = Cursors.SizeWE;
            }
            else
                control.Cursor = Cursors.SizeAll;
        }

        private void control_MouseUp(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            //if (control is Label)
            //    ((Label)control).BorderStyle = BorderStyle.None;
            //if (control is PictureBox)
            //    ((PictureBox)control).BorderStyle = BorderStyle.None;
            dragging = false;
            resizing = false;
            //PointToXY(label.Location);
        }

        private void PointToXY(Control control)
        {
            SpinEdit valeurX, valeurY;
            Control[] controls = Controls.Find(string.Format("se{0}X", control.Name.Substring(1)), true);
            if (controls.Length > 0)
            {
                valeurX = (SpinEdit)controls[0];
                controls = Controls.Find(string.Format("se{0}Y", control.Name.Substring(1)), true);
                if (controls.Length > 0)
                {
                    valeurY = (SpinEdit)controls[0];
                    valeurX.Text = Convert.ToString((int)(rulerControl2.PixelToScaleValue(control.Location.X) * 10));
                    valeurY.Text = Convert.ToString((int)(rulerControl2.PixelToScaleValue(control.Location.Y) * 10) + 6);
                    xtraTabControl1.SelectedTabPageIndex = xtraTabControl1.TabPages.IndexOf((XtraTabPage)valeurY.Parent.Parent);
                }
            }
        }
        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g = e.Graphics;
            //g.FillRectangle(Brushes.AntiqueWhite, dropRect);
            splitContainer2.Panel2.Update();
        }

        private void spinEdit_ValueChanged(object sender, EventArgs e)
        {
            if (!dragging)
            {
                SpinEdit spinEdit = sender as SpinEdit;
                Control control;
                Control[] controls = Controls.Find(string.Format("c{0}", spinEdit.Name.Substring(0, spinEdit.Name.Length - 1).Substring(2)), true);
                if (controls.Length > 0)
                {
                    control = (Control)controls[0];
                    if (spinEdit.Name.EndsWith("X"))
                        control.Left = rulerControl2.ScaleValueToPixel((Convert.ToDouble(spinEdit.Text)) / 10);
                    else
                        control.Top = rulerControl2.ScaleValueToPixel((Convert.ToDouble(spinEdit.Text) - 6) / 10);
                }
            }
        }

        private void bTest_Click(object sender, EventArgs e)
        {
            Chèque report = new Chèque()
            {
                montantX = (int)seMontantX.Value,
                montantY = (int)seMontantY.Value,
                montant_lettresX = (int)seMntLttrX.Value,
                montant_lettresY = (int)seMntLttrY.Value,
                largeurMntLttr = (int)seLargeurMntLttr.Value,
                largeurMntLttr2 = (int)seLargeurMntLttr2.Value,
                montant_lettres2X = (int)seMntLttr2X.Value,
                montant_lettres2Y = (int)seMntLttr2Y.Value,
                bénéficiaireX = (int)seBénéficiaireX.Value,
                bénéficiaireY = (int)seBénéficiaireY.Value,
                lieuX = (int)seLieuX.Value,
                lieuY = (int)seLieuY.Value,
                dateX = (int)seDateX.Value,
                dateY = (int)seDateY.Value,
                nonEndossableX = (int)seNonEndossableX.Value,
                nonEndossableY = (int)seNonEndossableY.Value,
                longueur = banque.longueur,
                largeur = banque.largeur,
                margeH = banque.margeH,
                margeV = banque.margeV,
                parametres = Paramètres.GetInstance(banque.Session),
                DataSource = null,
                fontMontant = GetFont(eFontMontant.Text, (float)seTailleMontant.Value, cbGrasMontant.Checked, cbItaliqueMontant.Checked, cbSoulignéMontant.Checked),
                fontMntLttr = GetFont(eFontMntLttr.Text, (float)seTailleMntLttr.Value, cbGrasMntLttr.Checked, cbItaliqueMntLttr.Checked, cbSoulignéMntLttr.Checked),
                fontBénéficiaire = GetFont(eFontBénéficiaire.Text, (float)seTailleBénéficiaire.Value, cbGrasBénéficiaire.Checked, cbItaliqueBénéficiaire.Checked, cbSoulignéBénéficiaire.Checked),
                fontLieu = GetFont(eFontLieu.Text, (float)seTailleLieu.Value, cbGrasLieu.Checked, cbItaliqueLieu.Checked, cbSoulignéLieu.Checked),
                fontDate = GetFont(eFontDate.Text, (float)seTailleDate.Value, cbGrasDate.Checked, cbItaliqueDate.Checked, cbSoulignéDate.Checked),
                préfixeMontant = ePréfixeMontant.Text,
                suffixeMontant = eSuffixeMontant.Text,
                préfixeMntLttr = ePréfixeMntLttr.Text,
                suffixeMntLttr = eSuffixeMntLttr.Text,
            };
            report.PrintDialog();
        }

        Font GetFont(string name, float size, bool bold, bool italic, bool underlined)
        {
            FontStyle fs = FontStyle.Regular;
            if (bold)
                fs = fs | FontStyle.Bold;
            if (italic)
                fs = fs | FontStyle.Italic;
            if (underlined)
                fs = fs | FontStyle.Underline;
            Font font = new Font(name, size, fs);
            return font;
        }

        private void eFont_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            ButtonEdit eFont = (ButtonEdit)sender;
            SpinEdit seTaille = (SpinEdit)eFont.Parent.Controls.Find("seTaille" + eFont.Name.Substring(5), false)[0];
            CheckEdit cbGras = (CheckEdit)eFont.Parent.Controls.Find("cbGras" + eFont.Name.Substring(5), false)[0];
            CheckEdit cbItalique = (CheckEdit)eFont.Parent.Controls.Find("cbItalique" + eFont.Name.Substring(5), false)[0];
            CheckEdit cbSouligné = (CheckEdit)eFont.Parent.Controls.Find("cbSouligné" + eFont.Name.Substring(5), false)[0];
            FontDialog fd = new FontDialog { Font = GetFont(eFont.Text, (float)seTaille.Value, cbGras.Checked, cbItalique.Checked, cbSouligné.Checked) };
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                eFont.Text = fd.Font.Name;
                seTaille.Value = (decimal)fd.Font.Size;
                cbGras.Checked = fd.Font.Bold;
                cbItalique.Checked = fd.Font.Italic;
                cbSouligné.Checked = fd.Font.Underline;
            }
        }

        private void Font_EditValueChanged(object sender, EventArgs e)
        {
            GroupControl gc = (GroupControl)((Control)sender).Parent;
            string rubrique = ((XtraTabPage)gc.Parent).Name.Substring(2);
            Label label = (Label)pChèque.Controls.Find("c" + rubrique, false)[0];
            ButtonEdit eFont = (ButtonEdit)gc.Controls.Find("eFont" + rubrique, false)[0];
            SpinEdit seTaille = (SpinEdit)gc.Controls.Find("seTaille" + rubrique, false)[0];
            CheckEdit cbGras = (CheckEdit)gc.Controls.Find("cbGras" + rubrique, false)[0];
            CheckEdit cbItalique = (CheckEdit)gc.Controls.Find("cbItalique" + rubrique, false)[0];
            CheckEdit cbSouligné = (CheckEdit)gc.Controls.Find("cbSouligné" + rubrique, false)[0];
            if (seTaille.Value > 0)
            {
                label.Font = GetFont(eFont.Text, (float)seTaille.Value, cbGras.Checked, cbItalique.Checked, cbSouligné.Checked);
                if (label == cMntLttr)
                    cMntLttr2.Font = GetFont(eFont.Text, (float)seTaille.Value, cbGras.Checked, cbItalique.Checked, cbSouligné.Checked);
            }
        }

        private void eAffixe_EditValueChanged(object sender, EventArgs e)
        {
            GroupControl gc = (GroupControl)((Control)sender).Parent;
            string rubrique = ((XtraTabPage)gc.Parent).Name.Substring(2);
            Label label = (Label)pChèque.Controls.Find("c" + rubrique, false)[0];
            TextEdit tePréfixe = (TextEdit)gc.Controls.Find("ePréfixe" + rubrique, false)[0];
            TextEdit teSuffixe = (TextEdit)gc.Controls.Find("eSuffixe" + rubrique, false)[0];
            string labelText = rubrique == "Montant" ? ChequeAdmin.Module.Core.resourceManager.GetString("Amount") : 
                ChequeAdmin.Module.Core.resourceManager.GetString("AmountLetters");
            label.Text = tePréfixe.Text + labelText + teSuffixe.Text;
        }

        private void ePréfixeMntLttr_EditValueChanged(object sender, EventArgs e)
        {
            cMntLttr.Text = string.Format("{0}{1}", ePréfixeMntLttr.Text, ChequeAdmin.Module.Core.resourceManager.GetString("AmountLetters1"));
        }

        private void eSuffixeMntLttr_EditValueChanged(object sender, EventArgs e)
        {
            cMntLttr2.Text = string.Format("{0}{1}", ChequeAdmin.Module.Core.resourceManager.GetString("AmountLetters2"), eSuffixeMntLttr.Text);
        }

        private void seLargeurMntLttr_EditValueChanged(object sender, EventArgs e)
        {
            cMntLttr.Width = (int)((seLargeurMntLttr.Value * (decimal)DpiX) / (decimal)25.4);
        }

        private void seLargeurMntLttr2_EditValueChanged(object sender, EventArgs e)
        {
            cMntLttr2.Width = (int)((seLargeurMntLttr2.Value * (decimal)DpiX) / (decimal)25.4);
        }
    }
}

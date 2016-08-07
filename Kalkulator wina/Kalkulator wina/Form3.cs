using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Kalkulator_wina
{
    public partial class Form3 : Form
    {
        private List<Typ_wina> typ;
        private Nastaw wino;
        public void dodaj(double blg, double woda)
        {
            wino.historia.Add(new Dodatek(blg, woda));
            ListViewItem item = new ListViewItem(blg.ToString());
            item.SubItems.Add(woda.ToString());
            listView1.Items.Add(item);
            listView1.Refresh();
        }
        public Form3(Form1 frm,int ind)
        {

            InitializeComponent();
            listView1.View = View.Details;
            listView1.GridLines = true;
            
            listView1.Columns.Add("Cukier", 120);
            listView1.Columns.Add("Woda", 120);
            wino = frm.wina[ind];
            this.Text ="Wino: "+ wino.nazwa;
            //zły balling
           // double baling = wino.historia[0].cukier * 100 / wino.historia[0].woda; // (wino.historia[0].cukier*0.62 + wino.historia[0].woda);//zweryfiokwać
            label2.Text = wino.balling_poczatkowy.ToString("F");
            label6.Text = "";
            label8.Text = "";
            label11.Text = "";
            label4.Text = wino.historia[0].woda.ToString("F");
            ListViewItem item;
            for (int i = 1; i < wino.historia.Count; i++)
            {
                item = new ListViewItem(wino.historia[i].cukier.ToString());
                item.SubItems.Add(wino.historia[i].woda.ToString());

                listView1.Items.Add(item);
            }
            typ = new List<Typ_wina>();
            typ.Add(new Typ_wina("Wytrawne", 0, 10));
            typ.Add(new Typ_wina("Półwytrawne", 10, 30));
            typ.Add(new Typ_wina("Półsłodkie", 30, 60));
            typ.Add(new Typ_wina("Słodkie", 60, 150));
            typ.Add(new Typ_wina("Bardzo słodkie", 150, int.MaxValue));
            textBox1.Text = wino.baling_koncowy.ToString();
            richTextBox1.Text = wino.uwagi;
            listView1.ContextMenuStrip = contextMenuStrip1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 DodajF = new Form4(this);
            DodajF.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            #region Zapisz parametry i zamknij okno
            char[] tekst = textBox1.Text.ToCharArray();
            //Licz parametry wina
            wino.uwagi = richTextBox1.Text;
            for (int i = 0; i < tekst.Length; i++)
            {
                if (tekst[i].Equals('.'))
                {
                    tekst[i] = ',';
                }
            }
            try {
                wino.baling_koncowy = Convert.ToDouble(new string(tekst));
                Close();
            }
            catch
            {
                MessageBox.Show("Nie prawidłowa wartość w polu Blg", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            #region Licz parametry wina; opuszczenie pola do wpisania ballingu koncowego lub przycik oblicz
            char[] tekst = textBox1.Text.ToCharArray();
            //Licz parametry wina
            if (tekst.Length > 0) { 
            for (int i = 0; i < tekst.Length; i++)
            {
                if (tekst[i].Equals('.'))
                {
                    tekst[i] = ',';
                }
            }
            try
            {
                double blg_end = Convert.ToDouble(new string(tekst));
                licz_alk(blg_end);
            }
            catch
            {


                    MessageBox.Show("Nie prawidłowa wartość w polu Blg", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                #endregion
            }
        }
        private void licz_alk(double blg_end)
        {
            #region Wyliczanie parametórw wina
            //licz
            double temp_w =0;
            double temp_c = 0;
            for (int i =0;i <wino.historia.Count;i++)
            {
                temp_w += wino.historia[i].woda;
                temp_c +=wino.historia[i].cukier; }

            double nie_cukry = (wino.historia[0].woda / temp_w)*4;//Poprawka na nie cukry
            double temp_alc = (temp_c * 100 / temp_w - nie_cukry - blg_end) / 1.69;// /100;
            double cukier_resz= (blg_end-nie_cukry+(temp_alc*0.1974+1.3))/100;
            double alc= (temp_c/(temp_w + (temp_c - wino.historia[0].cukier) * 0.62) - cukier_resz)*10/16.9;
        //    MessageBox.Show("nie cukry" + nie_cukry.ToString() + "\n cukier resztkowy" + cukier_resz.ToString() + "\n alkoho zakładany" + temp_alc.ToString() + "\n alkohol" + alc.ToString());
            label6.Text = (temp_w+(temp_c-wino.historia[0].cukier)*0.62).ToString("F")+" l"; //obj gotowego wina 
            label8.Text =  alc.ToString("p"); // wyliczony alkohol
            label11.Text = (cukier_resz*1000).ToString("f")+" g/l"; //cukier resztkowy
            foreach (Typ_wina t in typ)
            {
                if (cukier_resz * 1000> t.dolny && cukier_resz * 1000 < t.gorny) { label12.Text = t.typ; }

            }
            // ToolTip help = new ToolTip();
            //  help.SetToolTip(label11, "g/l");
            #endregion
        }

        private void usuńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Usuń dodatek wody lub cukru
            ListViewItem indeks = listView1.SelectedItems[0];
            wino.historia.Remove(wino.historia[indeks.Index+1]);
            listView1.Items.Remove(indeks);
            #endregion 
        }


    }
    class Typ_wina
    {
        #region Wykożystywane do określania czy winoje słodkie itd.
        public string typ { get; }
        public int dolny { get; }
        public int gorny { get; }
        public Typ_wina(string typ_w, int dolny_z, int gorny_z)
        {
            typ = typ_w;
            dolny = dolny_z;
            gorny = gorny_z;
        }
        #endregion
    }
}
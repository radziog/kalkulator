using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kalkulator_wina
{
    public partial class Form4 : Form
    {
        private Form3 frm3;
        private double cukier=0;
        private double woda=0;

        public Form4(Form3 frm)
        {
            InitializeComponent();
            frm3 = frm;
            
            label8.Text = ((char) 176).ToString() + "Blg";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region Odczytaj wartość i dodaj
            // Dodać okna błędów w catch
            char[] tekst = textBox1.Text.ToCharArray();// cukier
            
            for (int i = 0; i < tekst.Length; i++)
            {
                if (tekst[i].Equals('.'))
                {
                    tekst[i] = ',';
                }
            }
            try
            {
                cukier = Convert.ToDouble(new string(tekst));
            }
            catch { MessageBox.Show("Nie prawidłowa wartość w polu cukier.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            tekst = textBox2.Text.ToCharArray();//woda

                for (int i = 0; i < tekst.Length; i++)
                {
                    if (tekst[i].Equals('.'))
                    {
                        tekst[i] = ',';
                    }
                }
            try
            {
                woda = Convert.ToDouble(new string(tekst));
            }
            catch { MessageBox.Show("Nie prawidłowa wartość w polu woda.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            
            tekst = textBox3.Text.ToCharArray();//obj syropu
            if (tekst.Length > 0)
            {
                for (int i = 0; i < tekst.Length; i++)
                {
                    if (tekst[i].Equals('.'))
                    {
                        tekst[i] = ',';
                    }
                }
                try
                {
                    woda = Convert.ToDouble(new string(tekst));
                }
                catch { MessageBox.Show("Nie prawidłowa wartość w polu objętość", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
          

            tekst = textBox4.Text.ToCharArray(); //baling syropu
            if (tekst.Length > 0)
            {
                for (int i = 0; i < tekst.Length; i++)
                {
                    if (tekst[i].Equals('.'))
                    {
                        tekst[i] = ',';
                    }
                }
                double temp_c;
                try {
                    temp_c = Convert.ToDouble(new string (tekst));
                    temp_c = temp_c / 100;
                    cukier = temp_c / (1 - temp_c + temp_c * 0.62)*woda;
                }
               catch { MessageBox.Show("Nie prawidłowa wartość w polu Balling.","Błąd",MessageBoxButtons.OK,MessageBoxIcon.Error); }
                
            }
            frm3.dodaj(cukier, woda);
            this.Close();
            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #region Podpowiedzi do textbox
        private void textBox1_MouseHover(object sender, EventArgs e)
        {

            ToolTip help = new ToolTip();
            help.SetToolTip(textBox1,"Ilość cukru w kg");
        }

        private void textBox2_MouseHover(object sender, EventArgs e)
        {
            ToolTip help = new ToolTip();
            help.SetToolTip(textBox2, "Ilość wody w L");
        }

        private void textBox3_MouseHover(object sender, EventArgs e)
        {
            ToolTip help = new ToolTip();
            help.SetToolTip(textBox3, "Ilość cieczy w L");
        }

        private void textBox4_MouseHover(object sender, EventArgs e)
        {
            ToolTip help = new ToolTip();
            help.SetToolTip(textBox4, "Gęstość dodawanej mieszaniny w stopniach Balinga");
        }
        #endregion

    }
}

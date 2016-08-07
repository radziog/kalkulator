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
    public partial class Form5 : Form
    {
        private Form1 frm1;
        public Form5(Form1 frm)
        {
            InitializeComponent();
            frm1 = frm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region odzcytanie danych wprowdzonych przez urzytkownika i zapis w pamieci programu
            double cukier_poczatkowy = 0;
            double moszcz = 0;
            double balling = 0;
            double woda = 0;
            char[] tekst;
         
            tekst = textBox3.Text.ToCharArray();
            for (int i = 0; i < tekst.Length; i++)
            {
                if (tekst[i].Equals('.')) //formatowanie liczb zmienno przecinkowych
                {
                    tekst[i] = ',';
                }
            }
            try
            {
                balling = Convert.ToDouble(new string(tekst));

            }
            catch
            {
                MessageBox.Show("Nie prawidłowa wartość w polu balling moszczu", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            tekst = textBox4.Text.ToCharArray();
            try
            {
                for (int i = 0; i < tekst.Length; i++)
                {
                    if (tekst[i].Equals('.'))
                    {
                        tekst[i] = ',';
                    }
                }

                moszcz = Convert.ToDouble(new string(tekst));

            }
            catch
            {
                MessageBox.Show("Nie prawidłowa wartość w polu obj moszczu", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            tekst = textBox5.Text.ToCharArray();
            try
            {
                for (int i = 0; i < tekst.Length; i++)
                {
                    if (tekst[i].Equals('.'))
                    {
                        tekst[i] = ',';
                    }
                }

                woda = Convert.ToDouble(new string(tekst));

            }
            catch
            {
                MessageBox.Show("Nie prawidłowa wartość w polu woda", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            tekst = textBox6.Text.ToCharArray();
            try
            {
                for (int i = 0; i < tekst.Length; i++)
                {
                    if (tekst[i].Equals('.'))
                    {
                        tekst[i] = ',';
                    }
                }

                cukier_poczatkowy = Convert.ToDouble(new string(tekst));

            }
            catch
            {
                MessageBox.Show("Nie prawidłowa wartość w polu cukier początkowy", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            double pop_niecukry = moszcz / (moszcz + woda) * 4; //poprawka na niecukry w nastawie do wyliczenia cukru w moszczu
            double cukier = (balling - pop_niecukry) / 100;//kg cukru w 1kg roztworu(moszczu)
            double temp_c = cukier / (1 - cukier + cukier * 0.62);//kg cukru w 1l roztworu(moszczu)
            cukier = temp_c * moszcz + cukier_poczatkowy;//cukier w moszczu + dodany na początku
            Nastaw wino = new Nastaw(temp_c * moszcz, moszcz, textBox3.Text, textBox4.Text, dateTimePicker1.Text, richTextBox1.Text);
            // wino.nie_cukry = pop_niecukry;
            wino.balling_poczatkowy = balling;
            wino.historia.Add(new Dodatek(cukier_poczatkowy, woda)); // cupkier i woda dodane na początku
            frm1.wina.Add(wino);
            frm1.dodaj_do_wys(wino);
            frm1.refresh();
            Close();
            #endregion
        }
    
}
}

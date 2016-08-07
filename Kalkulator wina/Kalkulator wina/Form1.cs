using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace Kalkulator_wina
{
    public partial class Form1 : Form
    {
        /*dodać:
        -help file
            */
        private static SaveFileDialog sfd;
        private static OpenFileDialog ofd;
        public List<Nastaw> wina = new List<Nastaw>();
        public Form1()
        {
            InitializeComponent();
            listView1.Size = new Size(this.Size.Width - 40, this.Size.Height - 75); //uzależnienie wielkosci listview od wielkosci okna głownego
       
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Nazwa", 120);
            listView1.Columns.Add("Surowiec", 120);
            listView1.Columns.Add("Data nastawu",120);
            listView1.Refresh();
        }
        public void refresh()
        {
            listView1.Refresh();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Definicja okna zapisz 
            sfd = new SaveFileDialog();
            sfd.DefaultExt = "kw";
            sfd.Filter = "Poprzednio zapisane (*.kw)|*.kw|Wszystkie pliki (*.*)|*.*";
            sfd.RestoreDirectory = true;
            sfd.Title = "Zapisz jako";
            sfd.FileOk += new CancelEventHandler(s_wr);
            sfd.ShowDialog();
            #endregion
        }

        private void noweToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form nowe = new Form5(this);
            nowe.Show();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Definicja okna otwórz plik
            ofd = new OpenFileDialog();
            ofd.DefaultExt = "kw";
            ofd.Filter = "Poprzednio zapisane (*.kw)|*.kw|Wszystkie pliki (*.*)|*.*";
            ofd.RestoreDirectory = true;
            ofd.Title = "Wczytaj";
            ofd.FileOk += new CancelEventHandler(s_rd);
            ofd.ShowDialog();
            #endregion
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form about = new AboutBox1();
            about.ShowDialog();
        }

        private void s_wr(object sender, CancelEventArgs e)
        {
            #region Zapisywanie informacji do pliku
            using (Stream fs1 = new FileStream(sfd.FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                IFormatter formattter = new BinaryFormatter();
                foreach (Nastaw i in wina)
                {
                    formattter.Serialize(fs1, i);

                }
            }
            MessageBox.Show("Zapis zakończony sukcesem!", "Syukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
            #endregion
        }
        private void s_rd(object sender, CancelEventArgs e)
        {
            #region Odzczyt informacji z pliku
            try
            {
                using (Stream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (wina.Count != 0)
                    {
                        wina.Clear();
                        listView1.Items.Clear();
                    }
                    IFormatter formatter = new BinaryFormatter();

                    while (fs.Position < fs.Length)
                    {
                        Nastaw wino = new Nastaw();
                        wino = (Nastaw)formatter.Deserialize(fs);
                        dodaj_do_wys(wino);
                        wina.Add(wino);
                    }
                    fs.Close();
                }
               
            }
            catch
            {
                MessageBox.Show("Nie prawidłowy lub uszkodzony plik!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
            #endregion
        }
        public void dodaj_do_wys(Nastaw wino)
        {
            #region Dodanie nowego nastawu do listy wyświetlanych elementów
            ListViewItem element = new ListViewItem(wino.nazwa);
            element.SubItems.Add(wino.surowiec);
            element.SubItems.Add(wino.data);
            listView1.Items.Add(element);
            #endregion
        }

        private void edytujToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            #region Wybór i edycja wybranego wiersza
            ListView.SelectedIndexCollection indeks = listView1.SelectedIndices;
            if (indeks.Count > 0)//sprawdzam czy wybranow wiersz
            {
                    IEnumerator it = indeks.GetEnumerator();
                    it.MoveNext();
                    int i = (int)it.Current;
                    Form3 Edit = new Form3(this, i);
                    Edit.Show(this);
            }
            else {
                MessageBox.Show("Nie wybrano elementu do modyfikacji!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            #endregion
        }

        private void usuńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region Usuwanie niepotrzebnych nastawów nastawów  
            ListView.SelectedIndexCollection indeks = listView1.SelectedIndices;
            if (indeks.Count > 0)//sprawdzam czy wybrano wiersz
            {
                IEnumerator it = indeks.GetEnumerator();
                for (int ind = 0; ind < indeks.Count; ind++)// usuwanie wzsystkich zaznaczonych wierszy
                {
                    it.MoveNext();
                    int i = (int)it.Current;
                    wina.Remove(wina[i]);
                    listView1.Items.Remove(listView1.Items[i]);
               }
            }
            else {
                MessageBox.Show("Nie wybrano elementu do usunięcia", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            #endregion
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            #region Zwieksz rozmiary listview przy zmianie rozmiarów okna
            listView1.Size = new Size(this.Size.Width - 40, this.Size.Height - 75);
            listView1.Refresh();
            #endregion
        }
    }
    [Serializable]

    ///<summary>
   /// Przechowuje wszystkie informacje o projektowanym winie/nastawie
    ///  </sumary>
    public class Nastaw
    {
        #region  Obokty tej klasy wykożystywane są do przechowywania informacji o winie
       
        /*
       nazwa, surowiec i data wykożystyawne są do reprezentacji nastawu na liscie w oknie głównym
        */
       
        public string nazwa { set; get; } //nazwa wina
        public string surowiec { set; get; } //surowiec z któerego został wykonany
        public string uwagi { set; get; } //dodatkowe informacje np. dodatek innego składnika 
        public string data { set; get; } 
        public double balling_poczatkowy { set; get; } //wykorzystywany tylko w oknie edycji do wyświetlania ballingu zmierzonego w obliczenia brane jest pole z listy historia[0]
        public List<Dodatek> historia;
        /*przechowuje informacje o wodzie i cukrze dodanych do moszczu;
        historia[0] moszcz i cukier na podstawie ballingu 
        historia[1] woda i cukier dodane przed fermentacją
    */
        public double baling_koncowy { set; get; } //zakładana lub zmierzona wartość baligu po zakonczeniu fermentacji
        public Nastaw() 
        {
            historia = new List<Dodatek>();
        }
        public Nastaw(double cukier_poczatkowy, double obj)
        {
            historia = new List<Dodatek>();
            historia[0] = new Dodatek(cukier_poczatkowy, obj);
           
        }
        public Nastaw(double cukier_poczatkowy, double obj,string nazwa_nastawu, string surowce,  string data_nastawu, string uwagi_inne)
        {
            historia = new List<Dodatek>();
            historia.Add( new Dodatek(cukier_poczatkowy, obj));
            nazwa = nazwa_nastawu;
            surowiec = surowce;
            data = data_nastawu;
            uwagi = uwagi_inne;
        }
        public void Dodaj(Dodatek dodane) //dodawanie dodatow do historii
        {
            historia.Add(dodane);
        }
        #endregion
    }
    [Serializable]
    public class Dodatek
    {
        #region Obiekty tej klasy wykożystywane są do przechowywania informacji o dodanej wodzie lub cukrze
        public double woda { get; }
        public double cukier {  get; }
        public Dodatek() { }
        public Dodatek(double cukier_a, double woda_a)
        {
            woda = woda_a;
            cukier = cukier_a;
        }
        #endregion
    }
}

using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;//watek
using System.Data.OleDb;//access

namespace Client
{
    public partial class Form2 : Form
    {
        string source = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.txt");
        List <Skladnik> l_Skladnik = new List<Skladnik>();
        List<Skladnik> l_PomSkladnik = new List<Skladnik>();
        int idStartowe;//id od którego  ma zacząć czytać liste
        List<int> listaIlePozycjiNaStrone = new List<int>();
        List<Button> listaButtonowNaStronie = new List<Button>();
        string encryptyingCode;
        string ID;
        int ktoraStronaOgolnie = 0;
        string IP;
        public Form2(string ip,string ec,string id)
        {
            InitializeComponent();
            ID = id;
            IP = ip;
            encryptyingCode = ec;
            StreamReader sr;
            sr = new StreamReader(source);
            string text = sr.ReadLine();
            int ilosc = Convert.ToInt32(text);
            for (int i = 0; i < ilosc; i++)
            {
                text = sr.ReadLine();
                string[] splited = text.Split(' ');
                Skladnik skladnik = new Skladnik();
                skladnik.IdSM = Convert.ToInt32(splited[0]);
                if (splited.Length == 5)
                {
                    skladnik.NazwaSM = splited[1] + " " + splited[2];
                    skladnik.RodzajSM = splited[3];
                    skladnik.IdRodzaj = Convert.ToInt32(splited[3]);
                    skladnik.CenaSM = Convert.ToDouble(splited[4]);
                }
                else
                {
                    skladnik.NazwaSM = splited[1];
                    skladnik.RodzajSM = splited[2];
                    skladnik.IdRodzaj = Convert.ToInt32(splited[2]);
                    skladnik.CenaSM = Convert.ToDouble(splited[3]);
                }
                l_Skladnik.Add(skladnik);
            }
        }
        private void Form2_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
        private void Podziel()
        {
            for (int i = 1; i < 12; i++)
            {
                int ile = 0;
                int ktoraStronaNaLiscie = 0;
                foreach (Skladnik sm in l_Skladnik)
                {
                    int tmp = sm.IdRodzaj;
                    if (tmp == i)
                    {
                        l_PomSkladnik.Add(sm);
                        ile++;

                        if (ile == 36)
                        {
                            listaIlePozycjiNaStrone.Add(ile);
                            ile = 0;
                            ktoraStronaNaLiscie++;
                        }
                    }
                }
                listaIlePozycjiNaStrone.Add(ile);
            }
        }
        void StworzListeStron()
        {
            int ile = 0;
            int ktoraStronaNaLiscie = 0;
            foreach (Skladnik sm in l_Skladnik)
            {
                ile++;

                if (ile == 36)
                {
                    listaIlePozycjiNaStrone.Add(ile);
                    ile = 0;
                    ktoraStronaNaLiscie++;
                }
            }
            listaIlePozycjiNaStrone.Add(ile);
        }
        void CzyZaDuzoPozycji()
        {

            if (l_Skladnik.Count > 36)
            {

                button1.Visible = true;
                button2.Visible = true;
            }
        }
        private void StwórzButtony()//todo
        {
            try
            {
                int ileButtonow = listaIlePozycjiNaStrone[ktoraStronaOgolnie];//w rzędzie mieści się 6 w kolumnie 6
                int ilePokazac = ileButtonow;
                int x, y;
                x = y = 0;
                //int odKtoregoIDZaczac = ktoraStronaOgolnie * 36;
                idStartowe = 0;
                for (int i = 0; i < ktoraStronaOgolnie; i++)
                {
                    idStartowe += listaIlePozycjiNaStrone[i];
                }
                for (int a = idStartowe; a < idStartowe + ilePokazac; a++)
                {
                    if (a % 6 == 0 && x != 0)
                    {
                        y += 125;
                        x = 0;
                    }
                    Button dynamicButton = new Button();
                    dynamicButton.Height = 120;
                    dynamicButton.Width = 120;
                    //dynamicButton.BackColor = Color.Red;
                    //dynamicButton.ForeColor = Color.Blue;
                    dynamicButton.Location = new Point(320 + x, 80 + y);
                    dynamicButton.Text = l_PomSkladnik[a].NazwaSM;
                    x += 125;

                    dynamicButton.Click += new EventHandler(DynamicButton_Click);
                    listaButtonowNaStronie.Add(dynamicButton);
                    Controls.Add(dynamicButton);
                }
            }

            catch
            {
                if (listaIlePozycjiNaStrone[ktoraStronaOgolnie] == 0)
                    MessageBox.Show("Brak pozycji do wyświetlenia");
            }
        }
        private void DynamicButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            listBox1.Items.Add(clickedButton.Text);
            Cena(clickedButton.Text);
            //MessageBox.Show("Dynamic button is clicked");

        }
        private void Cena(string nazwa)
        {
            string cena = label2.Text;
            double a, b;
            a = double.Parse(l_Skladnik.Find(x => x.NazwaSM.Equals(nazwa)).CenaSM.ToString());
            b = double.Parse(cena);
            label2.Text = (a + b).ToString();
        }
        private void Koniec_Click(object sender, EventArgs e)
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            TcpClient tcpclnt = new TcpClient();
            tcpclnt.Connect(IP, 8001);
            Stream stm = tcpclnt.GetStream();
            string str;
            byte[] ba;
            byte[] bb;
            string tekst;
            int k;
            do
            {
                str = "O";//Przesłanie komunikatu o checi przeslania zamowienia
                str= Szyfrowanie.Encrypt(str, encryptyingCode);
                ba = asen.GetBytes(str);
                stm.Write(ba, 0, ba.Length);
                bb = new byte[256];
                k = stm.Read(bb, 0, 256);
                tekst = "";
                for (int i = 0; i < k; i++) tekst += (Convert.ToChar(bb[i]));
                tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
            } while (tekst != "OK");//potwierdzenie od serwera o przyjeciu checi przesłania danych
            do
            {
                str = Convert.ToString(listBox1.Items.Count);//wysłanie komunikatu o ilosci elementów w zamówieniu
                str = Szyfrowanie.Encrypt(str, encryptyingCode);
                ba = asen.GetBytes(str);
                stm.Write(ba, 0, ba.Length);
                bb = new byte[256];
                k = stm.Read(bb, 0, 256);
                tekst = "";
                for (int i = 0; i < k; i++) tekst += (Convert.ToChar(bb[i]));
                tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
            } while (tekst != "OK");//potwierdzenie od serwera o przyjeciu ilosci elementów
            do
            {
                str = Szyfrowanie.Encrypt(ID, encryptyingCode);//wysłanie komunikatu o ID klienta
                ba = asen.GetBytes(str);
                stm.Write(ba, 0, ba.Length);
                bb = new byte[256];
                k = stm.Read(bb, 0, 256);
                tekst = "";
                for (int i = 0; i < k; i++) tekst += (Convert.ToChar(bb[i]));
                tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
            } while (tekst != "OK");//potwierdzenie od serwera o przyjeciu ilosci elementów
            string text = "";

            for (int i = 0; i < (listBox1.Items.Count); i++)// przesył zamówień
            {
                text = listBox1.Items[i].ToString();
                //string[] split = text.Split('\t');
                int id = -1;
                foreach (var skladnik in l_Skladnik)//ustalanie id produktu
                {
                    if (skladnik.NazwaSM == text && id < 0)
                    {
                        id = skladnik.IdSM;
                        break;
                    }
                }
                do//przesył id produktu
                {
                    str = Convert.ToString(id);
                    str = Szyfrowanie.Encrypt(str, encryptyingCode);
                    ba = asen.GetBytes(str);
                    stm.Write(ba, 0, ba.Length);
                    bb = new byte[256];
                    k = stm.Read(bb, 0, 256);
                    tekst = "";
                    for (int j = 0; j < k; j++) tekst += (Convert.ToChar(bb[j]));
                    tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
                } while (tekst != "OK");
                
            }
            label2.Text = "0";
            listBox1.Items.Clear();
            tcpclnt.Close();
        }
        private void Form2_Load_1(object sender, EventArgs e)
        {
            //ZbudujListePozycji();
            CzyZaDuzoPozycji();
            Podziel();
            //StworzListeStron();
            StwórzButtony();
        }
        void UsunButtony()
        {
            foreach (Button btn in listaButtonowNaStronie)
            {
                this.Controls.Remove(btn);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //lewo
            if (ktoraStronaOgolnie > 0)
            {
                ktoraStronaOgolnie--;
                UsunButtony();
                StwórzButtony();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //prawo
            if (ktoraStronaOgolnie < listaIlePozycjiNaStrone.Count - 1)
            {
                ktoraStronaOgolnie++;
                UsunButtony();
                StwórzButtony();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

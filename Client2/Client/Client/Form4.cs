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
    public partial class Form4 : Form
    {
        string source = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.txt");
        string source2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasyZ.txt");
        List<Skladnik> l_Skladnik = new List<Skladnik>();
        bool workWorkMoneyMade = true;
        int screenCount = 0;
        Thread thr;
        List<Button> listaBtnów = new List<Button>();
        bool buttonsChanged = true;
        string IP;
        public Form4(string ip)
        {
            IP = ip;
            InitializeComponent();
            PobierzZamówienia();
            if (Screen.AllScreens.Length > 1)
                screenCount = 1;
            this.Location = Screen.AllScreens[screenCount].WorkingArea.Location;
            //this.Location = new Point(0, 0);
            this.Size = Screen.AllScreens[screenCount].WorkingArea.Size;
            l_Skladnik = ZbudujListe();
            thr = new Thread(this.Pokazuj);
            thr.Start();
        }
        private void PobierzZamówienia()
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
                str = "SZ";//Przesłanie komunikatu o checi zalogowania
                //str = Szyfrowanie.Encrypt(str, encryptyingCode);
                ba = asen.GetBytes(str);
                stm.Write(ba, 0, ba.Length);
                bb = new byte[256];
                //k = stm.Read(bb, 0, 256);//dubel
                k = stm.Read(bb, 0, 256);
                tekst = "";
                for (int i = 0; i < k; i++) tekst += (Convert.ToChar(bb[i]));
                // tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
                str = "";
            } while (tekst != "OK") ;

            StreamWriter sw = new StreamWriter(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasyZ.txt"));
            k = stm.Read(bb, 0, 256);
            tekst = "";
            for (int i = 0; i < k; i++) tekst += (Convert.ToChar(bb[i]));
            int ilosc = Convert.ToInt32(tekst);
            string test = "";
            UTF8Encoding coderUTF = new UTF8Encoding();
            sw.WriteLine(ilosc);
            for (int i = 0; i < ilosc; i++)
            {
                k = stm.Read(bb, 0, 3);
                tekst = "";
                for (int j = 0; j < k; j++) tekst += (Convert.ToChar(bb[j]));
                bb = new byte[Convert.ToInt32(tekst)];
                k = stm.Read(bb, 0, Convert.ToInt32(tekst));
                tekst = "";
                tekst = System.Text.Encoding.UTF8.GetString(bb);
                //for (int j = 0; j < k; j++) tekst += (Convert.ToChar(bb[j]));
                tekst += "";
                tekst += "";
                test = tekst;
                sw.WriteLine(test);
            }
            sw.Close();
        }
        private List<Skladnik> ZbudujListe()
        {
            List<Skladnik> tmp = new List<Skladnik>();
            StreamReader sr;
            sr = new StreamReader(source);
            string text = sr.ReadLine();
            int ilosc = Convert.ToInt32(text);
            for (int i = 0; i < ilosc; i++)
            {
                text = sr.ReadLine();
                string[] splited = text.Split('#');
                Skladnik skladnik = new Skladnik();
                skladnik.IdSM = Convert.ToInt32(splited[0]);
                skladnik.NazwaSM = splited[1];
                skladnik.RodzajSM = splited[2];
            }
            return tmp;
        }
        private void ObecneZamówienia_MouseClick(object sender, MouseEventArgs e)
        {
            thr.Abort();
            this.Close();
        }
        void Pokazuj()
        {
            int framer = 0;
            while (workWorkMoneyMade)
            {
                framer++;
                if (framer % 200 == 0)
                {
                    SetHour(DateTime.Now.ToString());
                    if (buttonsChanged)//wchodzi tylko jeżeli pojawiła się zmiana
                    {
                        buttonsChanged = !buttonsChanged;
                        Remove(listaBtnów);
                        listaBtnów = new List<Button>();
                        Thread.Sleep(100);
                        Działaj();
                    }
                }
            }
        }
        void Remove(List<Button> c)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<List<Button>>(Remove), new object[] { c });
                return;
            }
            else
            {
                try
                {
                    foreach (Button b in c)
                    {
                        //listaBtnów.Remove(b);
                        this.Controls.Remove(b);
                    }
                }
                catch
                {
                    //System.Diagnostics.Debug.WriteLine(c + " removed");
                }
            }

        }
        private void StwórzButton(int id, string what, DateTime when, int x, int y)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<int, string, DateTime, int, int>(StwórzButton), new object[] { id, what, when, x, y });
                return;
            }
            else
            {
                try
                {
                    string[] whatSpaces = what.Split(',');
                    Button dynamicButton = new Button();

                    dynamicButton.Width = 200;
                    dynamicButton.Height = this.Size.Width - 105;
                    dynamicButton.Font = new Font("Microsoft Sans Serif", 12);
                    //dynamicButton.BackColor = Color.Red;
                    //dynamicButton.ForeColor = Color.Blue;
                    dynamicButton.Location = new Point(x, y);
                    dynamicButton.Text = id.ToString() + Environment.NewLine + " " + when.ToString() + Environment.NewLine + Environment.NewLine;
                    foreach (string s in whatSpaces)
                    {
                        dynamicButton.Text += s + Environment.NewLine;/* what.Trim(new Char[] { ','});*/
                    }
                    dynamicButton.Tag = id;
                    dynamicButton.TextAlign = ContentAlignment.TopCenter;
                    dynamicButton.Click += new EventHandler(DynamicButton_Click);
                    this.Controls.Add(dynamicButton);
                    listaBtnów.Add(dynamicButton);
                }
                catch
                {
                    //MessageBox.Show("Brak pozycji do wyświetlenia");
                }
            }

        }
        private void DynamicButton_Click(object sender, EventArgs e)
        {
            //tu będzie przesył danych do serwera

            //Button clickedButton = sender as Button;
            //Zamówienie.WykonajZamówienie(Convert.ToInt32(clickedButton.Tag));
            //buttonsChanged = true;

        }
        void Działaj()
        {
            int a = 0;//ile w rzędzie
            int ileMaxWrzędzie;
            int ileMaxWkolumnie;
            int x, y;
            int maxX, maxY;
            maxX = this.Size.Width;
            maxY = this.Size.Height;
            ileMaxWrzędzie = maxX / 205;
            ileMaxWkolumnie = maxY / 205;
            ileMaxWkolumnie = 1;
            int Max = ileMaxWkolumnie * ileMaxWrzędzie;//maxymalna ilość btn na ekran?
            x = y = 0;
            y = 50;
            List<Zamówienia> listaZam = Zamówienia.ZbudujZamówienia(source2);
            List<Skladnik> listaSkl = Skladnik.ZbudujSkladniki(source);
            //y = maxY;
            foreach (Zamówienia zamówienie in listaZam)
            {
                if (a >= Max)
                    break;

                StwórzButton(zamówienie.IdZamówienia, Skladnik.GetNazwyZIdZPrzecinkamiKlient(zamówienie.IdZamówień), zamówienie.DataZamówienia, x, y);
                a++;
                if (a % ileMaxWrzędzie == 0 && x != 0)//jeżeli w rzędzie jest już wystarczająco
                {
                    y += 205;
                    x = 0;
                }
                else
                    x += 205;
            }
            SetCount(Max);
        }
        void SetCount(int M)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<int>(SetCount), new object[] { M });
                return;
            }
            else
            {
                List<Zamówienia> listaZam = Zamówienia.ZbudujZamówienia(source2);
                if ((listaZam.Count - M) > 0)
                {
                    label1.Text = "+" + (listaZam.Count - M);
                }
                else
                    label1.Text = "+" + 0;
            }

        }
        void SetHour(string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(SetHour), new object[] { text });
                return;
            }
            else
            {
                label2.Text = DateTime.Now.ToString();
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ObecneZamówienia_Load(object sender, EventArgs e)
        {

        }
    }
}

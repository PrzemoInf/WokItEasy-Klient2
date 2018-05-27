using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public partial class OdbiórZamówień : Form
    {
        static string source = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.txt");
        static string source2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasyZ.txt");
        static string source3 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasyK.txt");
        bool workWorkMoneyMade = true;
        int screenCount = 0;
        List<Skladnik> l_Skladnik = new List<Skladnik>();
        Thread thr;
        string IP;
        public OdbiórZamówień(string ip)
        {
            IP = ip;
            InitializeComponent();
            if (Screen.AllScreens.Length > 1)
                screenCount = 1;
            this.Location = Screen.AllScreens[screenCount].WorkingArea.Location;
            //this.Location = new Point(0, 0);
            this.Size = Screen.AllScreens[screenCount].WorkingArea.Size;
            l_Skladnik = Skladnik.ZbudujSkladniki(source);
            thr = new Thread(this.Pokazuj);
            thr.Start();
        }
        private void PobierzKategorie()
        {
            try
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
                    str = "SK";//Przesłanie komunikatu o checi pobrania listy zamówień
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
                } while (tekst != "OK");

                StreamWriter sw = new StreamWriter(source3);
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
            catch
            {

            }
        }
        private void PobierzZamówienia()
        {
            try
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
                    str = "SZ";//Przesłanie komunikatu o checi pobrania listy zamówień
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
                } while (tekst != "OK");

                StreamWriter sw = new StreamWriter(source2);
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
            catch
            {

            }
           
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
                    if (framer % 1000000 == 0)//wchodzi tylko jeżeli pojawiła się zmiana
                    {
                        //Clear();
                        Działaj();
                        framer = 1;
                        Thread.Sleep(100);
                    }
                }
            }
        }
        void Clear(short from, int a)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<short, int>(Clear), new object[] { from, a });
                    return;
                }
                else
                {
                    switch (from)
                    {
                        case 1:

                            listBox1.Items.Remove(a);
                            break;
                        case 2:
                            listBox2.Items.Remove(a);
                            break;
                    }
                }
            }
            catch
            {

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

        void Działaj()
        {
            PobierzKategorie();
            PobierzZamówienia();
            List<Zamówienia> listZam = new List<Zamówienia>();
            listZam = Zamówienia.ZbudujListeDoOdebrania(source2);
            foreach (Zamówienia zamówienie in listZam)
            {
                if (!zamówienie.Odebrane && zamówienie.Wykonane)//do odbioru
                {
                    if (!listBox2.Items.Contains(zamówienie.IdZamówienia))
                    {
                        Add(2, zamówienie.IdZamówienia);
                        Clear(1, zamówienie.IdZamówienia);
                    }
                }
                if (!zamówienie.Odebrane && !zamówienie.Wykonane)//w trakcie
                {
                    if (!listBox1.Items.Contains(zamówienie.IdZamówienia))
                        Add(1, zamówienie.IdZamówienia);
                }
            }
        }
        void Add(short a, int what)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<short, int>(Add), new object[] { a, what });
                    return;
                }
                else
                {
                    switch (a)
                    {
                        case 1:
                            listBox1.Items.Add(what);
                            break;
                        case 2:
                            listBox2.Items.Add(what);
                            break;
                    }
                    label2.Text = DateTime.Now.ToString();
                }
            }
            catch
            {

            }

        }
        void SetHour(string text)
        {
            try
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
            catch
            {

            }

        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void OdbiórZamówień_MouseClick(object sender, MouseEventArgs e)
        {
            workWorkMoneyMade = false;
            thr.Abort();
            this.Close();
        }

        private void listBox2_MouseClick(object sender, MouseEventArgs e)//usuń zamówienie (odebrane)
        {
            try
            {
                int a = Convert.ToInt32(listBox2.SelectedItem.ToString());
                Clear(2, a);
                //Zamówienie.OdbrierzZamówienie(a);
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
                    str = "OZ";//Przesłanie komunikatu o przesłaniu zrealizowanego zamówienia
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
                } while (tekst != "OK");
                str = Convert.ToString(a);
                ba = asen.GetBytes(str);
                stm.Write(ba, 0, ba.Length);
                Clear(1, a);
                Clear(1, a);
                Clear(1, a);
            }
            catch { }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void OznaczJakoWykonane(object sender, MouseEventArgs e)
        {
            int a = Convert.ToInt32(listBox1.SelectedItem.ToString());
            Clear(2, a);
            //Zamówienie.OdbrierzZamówienie(a);
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
                str = "OD";//Przesłanie komunikatu o przesłaniu zrealizowanego zamówienia
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
            } while (tekst != "OK");
            str = Convert.ToString(a);
            ba = asen.GetBytes(str);
            stm.Write(ba, 0, ba.Length);
            Clear(1, a);
            Clear(1, a);
            Clear(1, a);
        }
    }
}

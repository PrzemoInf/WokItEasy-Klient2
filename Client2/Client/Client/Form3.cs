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

namespace Client
{
    public partial class Form3 : Form
    {
        string source = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.txt");
        List<Skladnik> l_Skladnik = new List<Skladnik>();
        public Form3()
        {
            InitializeComponent();
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
                skladnik.CenaSM = Convert.ToDouble(splited[3]);
                l_Skladnik.Add(skladnik);
            }
            listBox1.Items.Clear();
            l_Skladnik = l_Skladnik.OrderBy(o => o.RodzajSM).ToList();
            foreach (Skladnik składnik in l_Skladnik)
            {
                string dots = Dots(składnik.NazwaSM + " (" + składnik.RodzajSM + ")", 20);
                listBox1.Items.Add(składnik.NazwaSM + " (" + składnik.RodzajSM + ")" + dots + składnik.CenaSM.ToString());

            }

        }
        string Dots(string w, int poIluZnakachCena)
        {
            int b = poIluZnakachCena - w.Length;
            w = "";
            for (int a = 0; a < b; a++)
            {
                w += ". ";

            }
            return w;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

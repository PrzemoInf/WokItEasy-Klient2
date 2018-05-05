using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Client
{
    class Zamówienia
    {
        int idZamówienia;
        DateTime dataZamówienia;
        string idZamówień;

        public int IdZamówienia { get => idZamówienia; set => idZamówienia = value; }
        public DateTime DataZamówienia { get => dataZamówienia; set => dataZamówienia = value; }
        public string IdZamówień { get => idZamówień; set => idZamówień = value; }

        public static List<Zamówienia> ZbudujZamówienia(string src)
        {
            List<Zamówienia> tmp = new List<Zamówienia>();
            StreamReader sr;
            sr = new StreamReader(src);
            string text = sr.ReadLine();
            int ilosc = Convert.ToInt32(text);
            for (int i = 0; i < ilosc; i++)
            {
                text = sr.ReadLine();
                string[] splited = text.Split('#');
                Zamówienia zam = new Zamówienia();
                zam.idZamówienia = Convert.ToInt32(splited[0]);
                zam.dataZamówienia = Convert.ToDateTime(splited[1]);
                zam.idZamówień = splited[2];
                tmp.Add(zam);
            }
            sr.Close();
            return tmp;
        }

    }
}

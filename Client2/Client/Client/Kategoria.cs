using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Client
{
    class Kategoria
    {
        int IDKategori;
        string nazwaKat;
        bool doKuchni;
        private static List<Kategoria> listaKategorii;
        public int IDKat { get => IDKategori; set => IDKategori = value; }
        public string NazwaKat { get => nazwaKat; set => nazwaKat = value; }
        public bool DoKuchni { get => doKuchni; set => doKuchni = value; }
        static string source = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasyK.txt");

        public static List<Kategoria> Zbuduj(string source)
        {
            try
            {
                List<Kategoria> listaKat = new List<Kategoria>();
                StreamReader sr;
                sr = new StreamReader(source);
                string text = sr.ReadLine();
                int iloscd = Convert.ToInt32(text);
                for (int i = 0; i < iloscd; i++)
                {
                    text = sr.ReadLine();
                    string[] splited = text.Split('#');
                    Kategoria kat = new Kategoria();
                    kat.IDKat = Convert.ToInt32(splited[0]);
                    kat.NazwaKat = splited[1];
                    if (splited[2] == "True") kat.DoKuchni = true;
                    else kat.DoKuchni = false;
                    listaKat.Add(kat);
                }
                return listaKat;
            }
            catch
            {
                return null;
            }
        }
        public static bool CzyNależyDoKuchni(string nazwaKategorii)
        {
            listaKategorii = Kategoria.Zbuduj(source);
            foreach (Kategoria k in Kategoria.listaKategorii)
            {
                if (k.NazwaKat == nazwaKategorii)
                {
                    return k.DoKuchni;

                }

            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Client
{
    class Skladnik
    {
        enum Rodzaj
        {
            Podstawa, Baza, Sos, Posypka, Inne, Napoje,
        }
        private int idRodzaj;
        private int idSM;
        private string nazwaSM;
        private string rodzajSM;
        private double cenaSM;
        private DateTime dataDodaniaSM;

        static string source = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.txt");
        public DateTime DataDodaniaSM { get => dataDodaniaSM; set => dataDodaniaSM = value; }
        public double CenaSM { get => cenaSM; set => cenaSM = value; }
        public string NazwaSM { get => nazwaSM; set => nazwaSM = value; }
        public int IdSM { get => idSM; set => idSM = value; }
        public string RodzajSM { get => rodzajSM; set => rodzajSM = value; }
        public int IdRodzaj { get => idRodzaj; set => idRodzaj = value; }

        public static List<Skladnik> ZbudujSkladniki(string src)
        {
            List<Skladnik> tmp = new List<Skladnik>();
            StreamReader sr;
            sr = new StreamReader(src);
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
                switch (splited[2])
                {
                    case "Sos":
                        {
                            skladnik.IdRodzaj = 1;
                            break;
                        }
                    case "Posypka":
                        {
                            skladnik.IdRodzaj = 2;
                            break;
                        }
                    case "Podstawa":
                        {
                            skladnik.IdRodzaj = 3;
                            break;
                        }
                    case "Proteina":
                        {
                            skladnik.IdRodzaj = 4;
                            break;
                        }
                    case "Napoje":
                        {
                            skladnik.IdRodzaj = 5;
                            break;
                        }
                    case "Inne":
                        {
                            skladnik.IdRodzaj = 6;
                            break;
                        }
                    case "Zupa":
                        {
                            skladnik.IdRodzaj = 7;
                            break;
                        }
                    case "Piwo":
                        {
                            skladnik.IdRodzaj = 8;
                            break;
                        }
                    case "Wino":
                        {
                            skladnik.IdRodzaj = 9;
                            break;
                        }
                    case "Wódka":
                        {
                            skladnik.IdRodzaj = 11;
                            break;
                        }
                }
                // skladnik.IdRodzaj = Convert.ToInt32(splited[2]);
                skladnik.CenaSM = Convert.ToDouble(splited[3]);
                tmp.Add(skladnik);
            }
            sr.Close();
            return tmp;
        }
        public static string GetNazwyZIdZPrzecinkamiKlient(string word)
        {
            string returner = "";
            string[] a = word.Split(',');
            List<int> listIds = new List<int>();
            returner = a[0] + "," + a[1] + ",";
            //foreach(string s in a)
            //{
            //    listIds.Add(Convert.ToInt32(s));
            //}
            for (int i = 2; i < a.Length; i++)
            {
                listIds.Add(Convert.ToInt32(a[i]));
            }

            
            foreach (int i in listIds)
            {
                List<Skladnik> tmp = ZbudujSkladniki(source);
                foreach (Skladnik sm in tmp)
                {
                    if (sm.IdSM == i)
                    {
                        returner += sm.nazwaSM;
                        returner += " ,";
                    }
                }

            }
            returner.TrimEnd(',');
            return returner;

        }
    }
}

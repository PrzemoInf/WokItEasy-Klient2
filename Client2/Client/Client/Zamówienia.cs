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
        bool wykonane = false;
        bool odebrane = false;
        bool wykonaneKuchnia = false;

        public int IdZamówienia { get => idZamówienia; set => idZamówienia = value; }
        public DateTime DataZamówienia { get => dataZamówienia; set => dataZamówienia = value; }
        public string IdZamówień { get => idZamówień; set => idZamówień = value; }
        public bool Odebrane { get => odebrane; set => odebrane = value; }
        public bool Wykonane { get => wykonane; set => wykonane = value; }
        public bool WykonaneKuchnia { get => wykonaneKuchnia; set => wykonaneKuchnia = value; }
        public static List<Zamówienia> ZbudujListeDoOdebrania(string src)
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
                if(splited[5]=="0")
                {
                    Zamówienia zam = new Zamówienia();
                    zam.idZamówienia = Convert.ToInt32(splited[0]);
                    zam.dataZamówienia = Convert.ToDateTime(splited[1]);
                    zam.idZamówień = splited[2];
                    if (splited[3] == "1") zam.wykonane = true;
                    else zam.wykonane = false;
                    if (splited[4] == "1") zam.wykonaneKuchnia = true;
                    else zam.wykonaneKuchnia = false;
                    tmp.Add(zam);
                }
                else
                {
                    
                }

            }
            sr.Close();
            return tmp;
        }
        public static List<Zamówienia> ZbudujZamówienia(string src,bool kuchnia)
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
                if(kuchnia==true)
                {
                    if(splited[4]=="0")
                    {
                        Zamówienia zam = new Zamówienia();
                        zam.idZamówienia = Convert.ToInt32(splited[0]);
                        zam.dataZamówienia = Convert.ToDateTime(splited[1]);
                        zam.idZamówień = splited[2];
                        tmp.Add(zam);
                    }
                }
                else
                {
                    if(splited[3]=="0")
                    {
                        Zamówienia zam = new Zamówienia();
                        zam.idZamówienia = Convert.ToInt32(splited[0]);
                        zam.dataZamówienia = Convert.ToDateTime(splited[1]);
                        zam.idZamówień = splited[2];
                        tmp.Add(zam);
                    }
                    
                }
                
            }
            sr.Close();
            return tmp;
        }

    }
}

﻿using System.Collections.Generic;
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
    public partial class Form1 : Form
    {
        //test adfada
        //List<Skladnik> l_Skladnik = new List<Skladnik>();
        public string IP;
        string ID;
        string encryptyingCode = "FISH!";
        public Form1()
        {
            InitializeComponent();
        }

        private void LogButton_Click(object sender, EventArgs e)
        {
            try
            {
                //StreamReader sr;
                ASCIIEncoding asen = new ASCIIEncoding();
                TcpClient tcpclnt = new TcpClient();
                tcpclnt.Connect(IPBox.Text, 8001);
                Stream stm = tcpclnt.GetStream();
                string str;
                byte[] ba;
                byte[] bb;
                string tekst;
                int k;
                do
                {
                    str = "L";//Przesłanie komunikatu o checi zalogowania
                    //str = Szyfrowanie.Encrypt(str, encryptyingCode);
                    ba = asen.GetBytes(str);
                    stm.Write(ba, 0, ba.Length);
                    bb = new byte[3];
                    k = stm.Read(bb, 0, 3);//dubel
                    tekst = "";
                    for (int i = 0; i < k; i++) tekst += (Convert.ToChar(bb[i]));
                    bb = new byte[Convert.ToInt32(tekst)];
                    k = stm.Read(bb, 0, Convert.ToInt32(tekst));
                    tekst = "";
                    for (int i = 0; i < k; i++) tekst += (Convert.ToChar(bb[i]));
                    //tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
                    str = "";
                } while (tekst != "OK");//potwierdzenie od serwera o przyjeciu checi logowania
                bb = new byte[256];
                str = textBox4.Text + " " + textBox5.Text;//login i hasło
                //str = Szyfrowanie.Encrypt(str, encryptyingCode);
                ba = asen.GetBytes(str);
                stm.Write(ba, 0, ba.Length);
                bb = new byte[3];
                k = stm.Read(bb, 0, 3);//dubel
                tekst = "";
                for (int i = 0; i < k; i++) tekst += (Convert.ToChar(bb[i]));
                bb = new byte[Convert.ToInt32(tekst)];
                k = stm.Read(bb, 0, Convert.ToInt32(tekst));
                tekst = "";
                for (int i = 0; i < k; i++) tekst += (Convert.ToChar(bb[i]));
                //tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
                if (tekst == "C")
                {
                    bb = new byte[3];
                    k = stm.Read(bb, 0, 3);//dubel
                    tekst = "";
                    for (int i = 0; i < k; i++) tekst += (Convert.ToChar(bb[i]));
                    bb = new byte[Convert.ToInt32(tekst)];
                    k = stm.Read(bb, 0, Convert.ToInt32(tekst));
                    tekst = "";
                    for (int i = 0; i < k; i++) tekst += (Convert.ToChar(bb[i]));
                    //tekst = Szyfrowanie.Decrypt(tekst, encryptyingCode);
                    ID = tekst;
                    //using (var output = File.Create(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.txt")))
                    //{
                    //    var buffer = new byte[1024];
                    //    int bytesRead;
                    //    while ((bytesRead = stm.Read(buffer, 0, buffer.Length)) > 0)
                    //    {
                    //        output.Write(buffer, 0, bytesRead);
                    //    }
                    //}
                    IP = IPBox.Text;
                    ZamButton.Visible = true;
                    textBox4.Visible = false;
                    textBox5.Visible = false;
                    LogButton.Visible = false;
                    IPBox.Visible = false;
                    pictureBox1.Visible = false;
                    pictureBox2.Visible = true;
                    label6.Visible = false;
                    label4.Visible = false;
                    label5.Visible = false;
                    button1.Visible = true;
                    button2.Visible = true;
                    button3.Visible = true;
                    button4.Visible = true;
                    button5.Visible = true;
                    button11.Visible = true;
                    MessageBox.Show("Połączono");
                }
                else if (tekst == "W") MessageBox.Show("Niepoprawne dane");
                tcpclnt.Close();
            }
            catch
            {
                MessageBox.Show("Błąd połączenia.");
            }
            
        }
        

        private void ZamButton_Click(object sender, EventArgs e)
        {   
            Form2 form2 = new Form2(IPBox.Text, encryptyingCode, ID);
            form2.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            TcpClient tcpclnt = new TcpClient();
            tcpclnt.Connect(IPBox.Text, 8001);
            Stream stm = tcpclnt.GetStream();
            string str;
            byte[] ba;
            byte[] bb;
            string tekst;
            int k;
            do
            {
                str = "W";//Przesłanie komunikatu o checi zalogowania
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

            str = textBox4.Text;//login
           // str = Szyfrowanie.Encrypt(str, encryptyingCode);
            ba = asen.GetBytes(str);
            stm.Write(ba, 0, ba.Length);

            //test
            IP = IPBox.Text;
            pictureBox1.Visible = true;
            pictureBox2.Visible = false;
            ZamButton.Visible = false;
            textBox4.Visible = true;
            textBox5.Visible = true;
            LogButton.Visible = true;
            IPBox.Visible = true;
            label6.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
            button11.Visible = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            TcpClient tcpclnt = new TcpClient();
            tcpclnt.Connect(IPBox.Text, 8001);
            Stream stm = tcpclnt.GetStream();
            string str;
            byte[] ba;
            byte[] bb;
            string tekst;
            int k;
            do
            {
                str = "M";//Przesłanie komunikatu o checi zalogowania
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
            StreamWriter sw = new StreamWriter(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.txt"));
            k = stm.Read(bb, 0, 256);
            tekst = "";
            for (int i = 0; i < k; i++) tekst += (Convert.ToChar(bb[i]));
            int ilosc = Convert.ToInt32(tekst);
            string test="";
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

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 oz = new Form4(IPBox.Text);
            oz.Show();
            if (Screen.AllScreens.Length > 1)
            {
                Screen[] screens = Screen.AllScreens;
                Rectangle bounds = screens[1].Bounds;
                oz.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                oz.StartPosition = FormStartPosition.Manual;
                oz.Location = Screen.AllScreens[1].WorkingArea.Location;
            }
            oz.WindowState = FormWindowState.Maximized;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form4 oz = new Form4(IPBox.Text,true);
            oz.Show();
            if (Screen.AllScreens.Length > 1)
            {
                Screen[] screens = Screen.AllScreens;
                Rectangle bounds = screens[1].Bounds;
                oz.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                oz.StartPosition = FormStartPosition.Manual;
                oz.Location = Screen.AllScreens[1].WorkingArea.Location;
            }
            oz.WindowState = FormWindowState.Maximized;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            OdbiórZamówień oz = new OdbiórZamówień(IP);
            oz.Show();
            if (Screen.AllScreens.Length > 1)
            {
                Screen[] screens = Screen.AllScreens;
                Rectangle bounds = screens[1].Bounds;
                oz.SetBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                oz.StartPosition = FormStartPosition.Manual;
                oz.Location = Screen.AllScreens[1].WorkingArea.Location;
            }
            oz.WindowState = FormWindowState.Maximized;
        }
    }
}

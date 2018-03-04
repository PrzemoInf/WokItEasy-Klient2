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
    public partial class Form1 : Form
    {
        //test adfada
        //List<Skladnik> l_Skladnik = new List<Skladnik>();
        public string IP;
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
                    ba = asen.GetBytes(str);
                    stm.Write(ba, 0, ba.Length);
                    bb = new byte[100];
                    k = stm.Read(bb, 0, 100);
                    tekst = "";
                    for (int i = 0; i < k; i++) tekst += (Convert.ToChar(bb[i]));
                } while (tekst != "OK");//potwierdzenie od serwera o przyjeciu checi logowania

                str = textBox4.Text + " " + textBox5.Text;//login i hasło
                ba = asen.GetBytes(str);
                stm.Write(ba, 0, ba.Length);
                bb = new byte[100];
                k = stm.Read(bb, 0, 100);
                tekst = "";
                for (int i = 0; i < k; i++) tekst += (Convert.ToChar(bb[i]));
                if (tekst == "C")
                {
                    using (var output = File.Create(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\WokItEasy1.txt")))
                    {
                        var buffer = new byte[1024];
                        int bytesRead;
                        while ((bytesRead = stm.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            output.Write(buffer, 0, bytesRead);
                        }
                    }
                    IP = IPBox.Text;
                    ZamButton.Visible = true;
                    textBox4.Visible = false;
                    textBox5.Visible = false;
                    LogButton.Visible = false;
                    IPBox.Visible = false;
                    label6.Visible = false;
                    label4.Visible = false;
                    label5.Visible = false;
                    button1.Visible = true;
                    button2.Visible = true;
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
           
                
            Form2 form2 = new Form2(IPBox.Text);
            form2.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //test
            IP = IPBox.Text;
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }
    }
}

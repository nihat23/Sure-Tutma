using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;//acces kutuphanesini eklıyoruz..

namespace süre_tutma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        OleDbConnection baglantim = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Surem.accdb");
        OleDbDataReader oku;
        OleDbCommand komutver;
        DataTable tablo;
        OleDbDataAdapter liste;
       

        void bilgilerimi_goster()
        {
            baglantim.Open();
            liste = new OleDbDataAdapter("select id as[Sıra],AdSoyad as [Adı Soyadı],Sure as [Süresi] from Bilgiler", baglantim);
            tablo = new DataTable();
            liste.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglantim.Close();
        }
        bool kontrol;
        void mukerrer()
        {
            baglantim.Open();
            komutver = new OleDbCommand("select * from Bilgiler where AdSoyad='" + textBox1.Text + "'", baglantim);
            oku = komutver.ExecuteReader();
            if (oku.Read())
            {
                kontrol = false;
            }
            else
            {
                kontrol = true;
            }
            baglantim.Close();

        }

        int saat = 0, dakika = 0, saniye = 0, salise = 0;
        private void button3Sifirla_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            salise = 0;
            saniye = 0;
            dakika = 0;
            saat = 0;

            textBox2.Text = saat.ToString().PadLeft(2, '0') + ":" + dakika.ToString().PadLeft(2, '0') + ":" + saniye.ToString().PadLeft(2, '0') + ":" + salise.ToString().PadLeft(2, '0');
        }

        private void button2DurDur_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void button4Kaydet_Click(object sender, EventArgs e)
        {
            mukerrer();

            if (kontrol == true)
            {
                if (textBox1.Text!= "" && textBox2.Text != "")
                {
                    baglantim.Open();
                    komutver = new OleDbCommand("insert into Bilgiler (AdSoyad,Sure) values('" + textBox1.Text + "','" + textBox2.Text + "')", baglantim);
                    komutver.ExecuteNonQuery();
                    baglantim.Close();
                    bilgilerimi_goster();

                    textBox1.Clear();
                    timer1.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Alanları doldurunuz..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                
            }
            else
            {
                MessageBox.Show(textBox1.Text+" Kayıt ismi  zaten var..","Uyarı",MessageBoxButtons.OK,MessageBoxIcon.Hand);
            }

            

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/n.beyi");
        }

        private void button1Cikar_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                DialogResult cvp = MessageBox.Show("Silmek istediginizden eminmisiniz..", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (cvp == DialogResult.Yes)
                {
                    baglantim.Open();
                    komutver = new OleDbCommand("delete from Bilgiler where AdSoyad='" + textBox1.Text + "' ", baglantim);
                    komutver.ExecuteNonQuery();
                    baglantim.Close();
                    MessageBox.Show("Kayıt Silindi..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    bilgilerimi_goster();
                    textBox1.Clear();
                }
                else
                {
                    bilgilerimi_goster();
                }
            }
            else
            {
                MessageBox.Show("Silmek istediginiz isim yazınız..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void button1Temizle_Click(object sender, EventArgs e)
        {
        }

        private void button1Arama_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                baglantim.Open();
                liste = new OleDbDataAdapter("select id as[Sıra],AdSoyad as[Adı Soyadı],Sure as[Süresi] from Bilgiler where AdSoyad like '" + textBox1.Text + "%'", baglantim);
                tablo = new DataTable();
                liste.Fill(tablo);

                dataGridView1.DataSource = tablo;
                baglantim.Close();
            }
            else
            {
                MessageBox.Show("Aranacak isim bulunamadı", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand); ;
            }
            

            /*
           int sonuc = listBox1.FindStringExact(textBox1.Text);

           if (sonuc > -1)
           {
               listBox1.SelectedIndex = sonuc;

           }
           else
           {
               MessageBox.Show("Aradıgınız isim listede yok..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
           }
            //sonu

           int siraNo = listBox1.Items.IndexOf(textBox1.Text);

           if (siraNo > -1)
           {
               listBox1.SelectedIndex = siraNo;
           }
           else
           {
               MessageBox.Show(textBox1.Text + " Aradıgınız isim bulunamadı..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
           }
           */
        }

        private void button1Yenile_Click(object sender, EventArgs e)
        {
            bilgilerimi_goster();
        }

        private void button1Baslat_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                timer1.Enabled = true;
            }
            else
            {
                MessageBox.Show("Lütfen isim yazınız..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bilgilerimi_goster();
            textBox2.Text = saat.ToString().PadLeft(2, '0') + ":" + dakika.ToString().PadLeft(2, '0') + ":" + saniye.ToString().PadLeft(2, '0') + ":" + salise.ToString().PadLeft(2, '0');
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox2.Text = saat.ToString().PadLeft(2, '0') + ":" + dakika.ToString().PadLeft(2, '0') + ":" + saniye.ToString().PadLeft(2, '0') + ":" + salise.ToString().PadLeft(2, '0');

            if (salise == 60)
            {
                saniye++;
                salise = 0;
                if (saniye == 60)
                {
                    dakika++;
                    saniye = 0;
                    if (dakika == 60)
                    {
                        saat++;
                        dakika = 0;
                        if (saat == 24)
                        {
                            saat = 0;
                        }
                    }
                }
            }
            salise++;

        }
    }
}

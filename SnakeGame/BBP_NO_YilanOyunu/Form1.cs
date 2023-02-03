using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BBP_NO_YilanOyunu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        yilan_yon yon;
        Yilan oyuncu;
        PictureBox[] parcalar;
        bool yem_var_mi = false;
        Random rastgele;
        PictureBox yem;
        int skor = 0,yemKontrol=0;
        List<PictureBox> duvarlar = new List<PictureBox>();
        /*NOT: FORM1 İÇİNDE BULUNAN AutoScale ÖZELLİĞİNİ FONT YERİNE NONE YAPILMIŞTIR.
         * BU ŞEKİLDE KOORDİNATLAR DAHA SAĞLIKLI ÇALIŞMAKTADIR*/
        private PictureBox pb_ekle()
        {
                PictureBox pb = new PictureBox();
                pb.Size = new Size(10, 10);
                pb.BackColor = Color.Red;
                pb.Location = oyuncu.YerAl(parcalar.Length-1);
                pb.Tag = "yilan";
                panel1.Controls.Add(pb);
                return pb;
        }

        private void yem_olustur()
        {

            rastgele = new Random();
            PictureBox pb = new PictureBox();
            pb.Size = new Size(10, 10);
            pb.BackColor = Color.FromArgb(120, 37, 1);
            pb.Location = new Point(rastgele.Next((panel1.Width-pb.Width-10)/10)*10, rastgele.Next((panel1.Height-pb.Height-10)/10)*10);
            pb.Tag = "yem";
            yem = pb;
            panel1.Controls.Add(yem);
            foreach (PictureBox nesne in panel1.Controls) //<- dinamik olarak duvara yem gelmeme kontrolü
            {
                if (yem.Bounds.IntersectsWith(nesne.Bounds) && nesne.Tag.ToString() == "duvar")
                {
                    yem_var_mi = false;
                    //Ders içerisinde sağlıklı çalışmayan dinamik kontrolde eklemeler yapılmıştır.
                    //Hatanın sebebi eğer yem, bir kere sınır içinde oluşursa diğer duvarların da hâlâ kontrol
                    //edilmesinden kaynaklanıyordu. Bu hata düzeltildikten sonra sınır içinde yer alıp daha sonra
                    //yeniden üretilen yemin üzerinden geçince herhangi bir işlem olmama hatası oluştu. Aşağıdaki
                    //iki satır bu hataları gidermek içindir.
                    panel1.Controls.Remove(yem); //eğer duvar içinde oluşan yem panelden kaldırılmazsa birden fazla yem olması programın çalışmasında hataya sebep olmaktadır.
                    break; //eğer "duvar" etiketli herhangi bir PictureBox'a rastlanırsa diğer PictureBox'ların kontrol edilmemesi için döngü sonlanır.
                }
                else
                {
                    yem_var_mi = true;
                }
            }
            
        }

        private void yem_yedi_mi()
        {
            //iki nesnenin birbirleri üzerine tam gelme koordinatı 
            //(program tasarlanış şekline uyduğu için birebir koordinatlar eşitliği kontrol edilebilir)
            //alternatif ve daha genel kullanılabilecek yöntem kendisine_carpti() fonksiyonunda yer almaktadır.
            if (oyuncu.YerAl(0)==yem.Location) 
            {
                panel1.Controls.Remove(yem);
                yem_var_mi = false;
                skor += 10;
                lbl_skor.Text = skor.ToString();
                yemKontrol++;
                
                if (yemKontrol==2)
                {
                    timer1.Interval -= 5;
                    oyuncu.buyume();
                    Array.Resize(ref parcalar, parcalar.Length + 1);
                    parcalar[parcalar.Length - 1] = pb_ekle();
                    yemKontrol = 0;
                }

                lbl_yem.Text = yemKontrol.ToString();
            }
        }


        private void yer_guncelle()
        {
            Point Yer = oyuncu.YerAl(0);
            for (int i = 0; i < parcalar.Length; i++)
            {
                parcalar[i].Location = oyuncu.YerAl(i); //.Location özelliği sayesinde yeni yerler belirlenir. Bunların değeri ise YerAl(i) fonksiyonundan gelmekte
            }

            if (Yer.X < 0)
            {
                oyuncu.tunel(panel1.Width - 10, Yer.Y);
            }
            if (Yer.X > panel1.Width - 10)
            {
                oyuncu.tunel(0, Yer.Y);
            }
        }

        private void oyun_bitti()
        {
            timer1.Stop();
            MessageBox.Show("Oyun bitti. Yenildin :( \r\nSkorunuz=" + skor);
            button1.Enabled = true;
        }
        private void duvara_carpti()
        {

            foreach (PictureBox nesne in panel1.Controls)
            {
                if (parcalar[0].Bounds.IntersectsWith(nesne.Bounds) && nesne.Tag.ToString()=="duvar")
                {
                    oyun_bitti();
                }
            }
        }

        private void kendisine_carpti()
        {
            for (int i = 1; i < parcalar.Length; i++)
            {
                if (parcalar[0].Bounds.IntersectsWith(parcalar[i].Bounds))
                {
                    oyun_bitti();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (PictureBox nesne in panel1.Controls)
            {
                if (nesne.Tag.ToString()=="duvar")
                {
                    duvarlar.Add(nesne);
                }
            }
            skor = 0; yemKontrol = 0;
            lbl_skor.Text = skor.ToString();
            lbl_yem.Text = skor.ToString();
            yem_var_mi = false;

            panel1.Controls.Clear();

            foreach (PictureBox nesne in duvarlar)
            {
                panel1.Controls.Add(nesne);
            }
            yon = new yilan_yon(10, 0);
            oyuncu = new Yilan();
            parcalar = new PictureBox[0];

            for (int i = 0; i < 3; i++)
            {
                Array.Resize(ref parcalar, parcalar.Length + 1);
                parcalar[i] = pb_ekle();
            }

            parcalar[0].BackColor = Color.Green;

            //büyüme olmadan yılanı getirme kodları
            /*
            for (int i = 0; i < 3; i++)
            {
                PictureBox pb = new PictureBox();
                pb.Size = new Size(10, 10);
                pb.BackColor = Color.Red;
                pb.Location = oyuncu.YerAl(i);
                panel1.Controls.Add(pb);
                parcalar[i] = pb;
            }
            */
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                if (yon.yYon != 10)//Basılan yönün tersinde hareket değeri var ise hiçbir işlem yapılmayacak
                {
                    yon = new yilan_yon(0, -10);
                }      
            }
            if (e.KeyCode == Keys.A)
            {
                if (yon.xYon != 10)
                {
                    yon = new yilan_yon(-10, 0);
                }
            }
            if (e.KeyCode == Keys.S)
            {
                if (yon.yYon != -10)
                {
                    yon = new yilan_yon(0, 10);
                }      
            }
            if (e.KeyCode == Keys.D)
            {
                if (yon.xYon != -10)
                {
                    yon = new yilan_yon(10, 0);
                }         
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            oyuncu.ilerle(yon);
            yer_guncelle();
            if (!yem_var_mi)
            {
                yem_olustur();
            }
            yem_yedi_mi();
            duvara_carpti();
            kendisine_carpti();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1_Load(null, null); //Form1_Load bir fonksiyon olduğu için parametreleri girildiği (null,null) taktirde içindeki kodların tamamı bu şekilde kullanılabilir.
            button1.Enabled = false;
            timer1.Start();
            panel1.Focus();
        }

    }
}

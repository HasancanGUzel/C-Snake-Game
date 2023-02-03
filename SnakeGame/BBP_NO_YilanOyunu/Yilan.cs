using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BBP_NO_YilanOyunu
{

    class yilan_koordinat
    {
        public int xKoordinat;
        public int yKoordinat;

        public yilan_koordinat(int x, int y)
        {
            xKoordinat = x;
            yKoordinat = y;
        }
    }

    class yilan_yon
    {
        public int xYon;
        public int yYon;

        public yilan_yon(int x, int y)
        {
            xYon = x;
            yYon = y;
        }
    }
    class Yilan
    {
        yilan_koordinat[] yilan_parcalari;

        int yilan_buyuklugu;
        yilan_yon yilan_yonu;
        public Yilan()
        {
            yilan_parcalari = new yilan_koordinat[3];
            yilan_buyuklugu = 3;
            yilan_parcalari[0] = new yilan_koordinat(200, 200);
            yilan_parcalari[1] = new yilan_koordinat(190, 200);
            yilan_parcalari[2] = new yilan_koordinat(180, 200);
        }

        public void ilerle(yilan_yon yon)
        {
            yilan_yonu = yon;

            for (int i = yilan_parcalari.Length-1; i>0; i--)
            {
                yilan_parcalari[i] = new yilan_koordinat(yilan_parcalari[i - 1].xKoordinat, yilan_parcalari[i - 1].yKoordinat);
            }

            yilan_parcalari[0] = new yilan_koordinat(yilan_parcalari[0].xKoordinat + yilan_yonu.xYon, yilan_parcalari[0].yKoordinat + yilan_yonu.yYon);
        }

        public void tunel(int x,int y)
        {
            yilan_parcalari[0] = new yilan_koordinat(x, y);
        }
        public void buyume()
        {
            Array.Resize(ref yilan_parcalari, yilan_parcalari.Length + 1);
            yilan_parcalari[yilan_parcalari.Length-1] = new yilan_koordinat(yilan_parcalari[yilan_parcalari.Length - 2].xKoordinat, yilan_parcalari[yilan_parcalari.Length -2].yKoordinat);
        }

        public Point YerAl(int eleman)
        {
            return new Point(yilan_parcalari[eleman].xKoordinat, yilan_parcalari[eleman].yKoordinat);
        }

    }
}

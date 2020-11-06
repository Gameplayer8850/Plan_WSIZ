using System;
using System.Collections.Generic;
using System.Text;

namespace Dane.Plan
{
    public class Dzien
    {
        public int dzien=-1;
        public string data = "";
        List<Zajecie> zajecia = new List<Zajecie>();

        enum dzien_tygodnia
        {
            Pon, Wtor, Sro, Czw, Piat, Sob, Nied
        }
        public void Dodaj_zajecie(string godzina, string nazwa)
        {
            zajecia.Add(new Zajecie() { godzina = godzina, nazwa = nazwa });
        }
        public bool Czy_inny_plan(Dzien stary_dzien)
        {
            if (stary_dzien.zajecia.Count != this.zajecia.Count) return true;
            for (int i = 0; i < this.zajecia.Count + 1; i++) if (!stary_dzien.zajecia[i].Equals(this.zajecia[i])) return true;
            return false;
        }
        class Zajecie
        {
            public string godzina = "";
            public string nazwa = "";
            public override bool Equals(Object obj)
            {
                Zajecie zaj = (Zajecie)obj;
                if (this.godzina.Trim() != zaj.godzina.Trim()) return false;
                if (this.nazwa.Trim() != zaj.nazwa.Trim()) return false;
                return true;
            }
        }
    }
}

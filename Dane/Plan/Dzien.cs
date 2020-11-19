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
        public void Dodaj_zajecie(string godzina, string nazwa, string wykladowca, int grupa)
        {
            zajecia.Add(new Zajecie() { godzina = godzina, nazwa = nazwa, wykladowca=wykladowca, grupa=grupa });
        }
        public bool Czy_inny_plan(Dzien stary_dzien)
        {
            if (stary_dzien.zajecia.Count != this.zajecia.Count) return true;
            for (int i = 0; i < this.zajecia.Count; i++) if (!stary_dzien.zajecia[i].Equals(this.zajecia[i])) return true;
            return false;
        }
        public string Zwroc_dane_do_wiadomosci_dla_grupy(int numer_grupy, List<string[]> linki)
        {
            string wiadomosc = "";
            foreach(Zajecie zaj in zajecia)
            {
                if (zaj.grupa == numer_grupy && zaj.nazwa != "")
                {
                    string[] wykladowca;
                    string link = "[BRAK LINKU]";
                    if (zaj.wykladowca != "")
                    {
                        wykladowca = linki.Find(x => x[0]!=null && x[0].ToLower().Contains(zaj.wykladowca.ToLower()));
                        if (wykladowca != null) link = wykladowca[1];
                    }

                    wiadomosc += "\n" + "`" + zaj.godzina + " - " + zaj.nazwa + "`\n" + link;
                }
            }
            return wiadomosc;
        }
        class Zajecie
        {
            public string godzina = "";
            public string nazwa = "";
            //elearning po zmianach
            public int grupa = 0;
            public string wykladowca = "";
            public override bool Equals(Object obj)
            {
                Zajecie zaj = (Zajecie)obj;
                if (this.godzina.Trim() != zaj.godzina.Trim()) return false;
                if (this.nazwa.Trim() != zaj.nazwa.Trim()) return false;
                return true;
            }
            public override int GetHashCode()
            {
                return (nazwa+godzina).GetHashCode();
            }
        }
    }
}

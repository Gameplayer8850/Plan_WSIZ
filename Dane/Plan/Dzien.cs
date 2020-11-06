using System;
using System.Collections.Generic;
using System.Text;

namespace Dane.Plan
{
    public class Dzien
    {
        int dzien=-1;
        string data = "";
        List<Zajecie> zajecia = new List<Zajecie>();

        enum dzien_tygodnia
        {
            Pon, Wtor, Sro, Czw, Piat, Sob, Nied
        }
        public void Dodaj_zajecie(string godzina, string nazwa)
        {
            zajecia.Add(new Zajecie() { godzina = godzina, nazwa = nazwa });
        }
        class Zajecie
        {
            public string godzina = "";
            public string nazwa = "";
        }
    }
}

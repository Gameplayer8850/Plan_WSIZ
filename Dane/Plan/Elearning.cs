using System;
using System.Collections.Generic;
using System.Text;

namespace Dane.Plan
{
    public class Elearning
    {
        public int numer_semestru = -1;
        public string nazwa_przedmiotu = "";
        public DateTime dzien = default(DateTime);
        public string godzina = "";
        public string grupa = "";
        public string link_do_zajec="";

        public string Zwroc_dane_do_wiadomosci()
        {
            return "`" + godzina + " - " + nazwa_przedmiotu+"`\n"+link_do_zajec;
        }
    }
}

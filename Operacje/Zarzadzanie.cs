using System;
using System.Collections.Generic;
using System.Text;
using Dane.XML;
using Dane;
using Dane.Plan;

namespace Operacje
{
    public class Zarzadzanie
    {
        List<object> pobrane_obiekty_xml=new List<object>();
        XML opxml;
        Web wb;
        Pliki plk;
        bool czy_istnieje_temp = false;
        public Zarzadzanie()
        {
            opxml = new XML();
            wb = new Web();
            plk = new Pliki();
            pobrane_obiekty_xml = new List<object>();
        }
        
        public bool Pobierz_dane_poczatkowe()
        {
            if (!Pobierz_stworz_plik_xml(Globalne.pliki_xml.Config)) return false;
            czy_istnieje_temp = Pobierz_stworz_plik_xml(Globalne.pliki_xml.Temp);
            return true;
        }
        public bool Pobierz_stworz_plik_xml(Globalne.pliki_xml rodzaj)
        {
            object obj = null;
            bool result = true;
            if (!opxml.Czy_istnieje_plik_xml(rodzaj)) result = false;
            if (result) obj = opxml.Pobierz_dane(rodzaj);
            if (!result || obj == null)
            {
                pobrane_obiekty_xml.Add(opxml.Zapisz_szablon(rodzaj));
                return false;
            }
            pobrane_obiekty_xml.Add(obj);
            return true;
        }
        private bool Czy_nowy_plan()
        {
            Temp tmp;
            tmp = (Temp)pobrane_obiekty_xml[(int)Globalne.pliki_xml.Temp];
            List<List<string>> dane = wb.Zwroc_dane_o_najnowszym_planie(((Config)pobrane_obiekty_xml[(int)Globalne.pliki_xml.Config]).link_do_strony_z_planem);
            if (dane == null || dane.Count == 0) return false;
            Temp tmp2 = new Temp() { data_dodania = DateTime.Parse(dane[0][1]), link_do_planu = dane[0][2]};
            opxml.Zapisz_dane(tmp2, Globalne.pliki_xml.Temp);
            if (czy_istnieje_temp && tmp2.Equals(tmp)) return false;
            pobrane_obiekty_xml[(int)Globalne.pliki_xml.Temp] = tmp2;
            return true;
        }
        public void Sprawdz_plan(bool czy_sprawdzic_roznice=false)
        {
            if (!Czy_nowy_plan()) return;
            string link = ((Temp)pobrane_obiekty_xml[(int)Globalne.pliki_xml.Temp]).link_do_planu;
            string wiadomosc = "@everyone Pojawił się nowy plan zajęć!\nLink: "+ link;
            wb.Wyslij_do_webhooka(((Config)pobrane_obiekty_xml[(int)Globalne.pliki_xml.Config]).webhook_nowy_plan, wiadomosc);
            bool istnieje_nowy_plan= (Globalne.rozszerzenia_plikow_planu[(int)Globalne.pliki_plany.Nowy] != "");
            bool istnieje_stary_plan = (Globalne.rozszerzenia_plikow_planu[(int)Globalne.pliki_plany.Stary] != "");
            string stara_lokalizacja = Globalne.lokalizacja + @"/" + Globalne.nazwy_folderow[(int)Globalne.foldery.Plany] + @"/" + Globalne.nazwy_plikow_planu[(int)Globalne.pliki_plany.Stary];
            string nowa_lokalizacja = Globalne.lokalizacja + @"/" + Globalne.nazwy_folderow[(int)Globalne.foldery.Plany] + @"/" + Globalne.nazwy_plikow_planu[(int)Globalne.pliki_plany.Nowy];
            if (istnieje_stary_plan) plk.Usun_plik(stara_lokalizacja + Globalne.rozszerzenia_plikow_planu[(int)Globalne.pliki_plany.Stary]);
            if (istnieje_nowy_plan) plk.Zmien_nazwe(nowa_lokalizacja + Globalne.rozszerzenia_plikow_planu[(int)Globalne.pliki_plany.Nowy], stara_lokalizacja + Globalne.rozszerzenia_plikow_planu[(int)Globalne.pliki_plany.Nowy]);
            Globalne.rozszerzenia_plikow_planu[(int)Globalne.pliki_plany.Stary] = (string)Globalne.rozszerzenia_plikow_planu[(int)Globalne.pliki_plany.Nowy].Clone();
            Globalne.rozszerzenia_plikow_planu[(int)Globalne.pliki_plany.Nowy] = link.Substring(link.LastIndexOf('.'));
            wb.Pobierz_plik(link, nowa_lokalizacja + Globalne.rozszerzenia_plikow_planu[(int)Globalne.pliki_plany.Nowy]);
            if (czy_sprawdzic_roznice) Roznice_w_planie();
        }
        public void Roznice_w_planie()
        {
            if (!Czy_mozna_porownywac()) return;
            Excel ex = new Excel();
            int numer_semestru = ((Config)pobrane_obiekty_xml[(int)Globalne.pliki_xml.Config]).semestr;
            string wiadomosc=ex.Zwroc_roznice(numer_semestru);
            if (wiadomosc == "") wiadomosc = "`Nie znaleziono różnic dla " + numer_semestru + " semestru`";
            wb.Wyslij_do_webhooka(((Config)pobrane_obiekty_xml[(int)Globalne.pliki_xml.Config]).webhook_nowy_plan, wiadomosc);
        }

        public void Podmien_rozszerzenia_plikow()
        {
            List<string> lista_planow = plk.Zwroc_liste_plikow(Globalne.lokalizacja + @"/" + Globalne.nazwy_folderow[(int)Globalne.foldery.Plany]);
            if (lista_planow != null)
                foreach (string nazwa in lista_planow)
                {
                    string nazwa_bez_sciezki = nazwa.Substring(nazwa.LastIndexOf('\\'));
                    if (nazwa.Contains(Globalne.nazwy_plikow_planu[(int)Globalne.pliki_plany.Nowy])) Globalne.rozszerzenia_plikow_planu[(int)Globalne.pliki_plany.Nowy] = nazwa_bez_sciezki.Substring(nazwa_bez_sciezki.LastIndexOf('.'));
                    else if (nazwa.Contains(Globalne.nazwy_plikow_planu[(int)Globalne.pliki_plany.Stary])) Globalne.rozszerzenia_plikow_planu[(int)Globalne.pliki_plany.Stary] = nazwa_bez_sciezki.Substring(nazwa_bez_sciezki.LastIndexOf('.'));
                }
        }

        private bool Czy_mozna_porownywac()
        {
            return (Globalne.rozszerzenia_plikow_planu[0] != "" && Globalne.rozszerzenia_plikow_planu[1] != "");
        }

        public void Elearning_dla_grup()
        {
            Excel ex = new Excel();
            DateTime dzisiaj = DateTime.Now;
            List<string> lista_plikow = plk.Zwroc_liste_plikow(Globalne.lokalizacja + Globalne.nazwy_folderow[(int)Globalne.foldery.Elearning]);
            if (lista_plikow == null || lista_plikow.Count == 0) return;
            List<Elearning> zajecia = ex.Zwroc_zajecia_elearning_dla_grupy(lista_plikow[0], dzisiaj, ((Config)pobrane_obiekty_xml[(int)Globalne.pliki_xml.Config]).semestr);
            if (zajecia == null || zajecia.Count == 0) return;
            string grupa_a = "";
            string grupa_b = "";
            foreach(Elearning zaj in zajecia)
            {
                if (zaj.grupa.Contains(" A")) grupa_a += "\n"+zaj.Zwroc_dane_do_wiadomosci();
                else if (zaj.grupa.Contains(" B")) grupa_b += "\n" + zaj.Zwroc_dane_do_wiadomosci();
                else
                {
                    grupa_a += "\n" + zaj.Zwroc_dane_do_wiadomosci();
                    grupa_b += "\n" + zaj.Zwroc_dane_do_wiadomosci();
                }
            }
            if (grupa_a != "") wb.Wyslij_do_webhooka(((Config)pobrane_obiekty_xml[(int)Globalne.pliki_xml.Config]).webhook_grupa_A, "`Dzień: " + dzisiaj.Date.ToString("dd.MM") + "`" + grupa_a);
            if (grupa_b != "") wb.Wyslij_do_webhooka(((Config)pobrane_obiekty_xml[(int)Globalne.pliki_xml.Config]).webhook_grupa_B, "`Dzień: " + dzisiaj.Date.ToString("dd.MM") + "`" + grupa_b);
        }
    }
}

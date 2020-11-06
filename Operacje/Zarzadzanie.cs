using System;
using System.Collections.Generic;
using System.Text;
using Dane.XML;
using Dane;

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
        public bool Czy_nowy_plan()
        {
            bool result=true;
            Temp tmp;
            tmp = (Temp) pobrane_obiekty_xml[(int)Globalne.pliki_xml.Temp];
            string link=wb.Zwroc_link_do_planu(((Config)pobrane_obiekty_xml[(int)Globalne.pliki_xml.Config]).link_do_strony_z_planem);
            if (czy_istnieje_temp && link == tmp.link_do_planu) result=false;
            tmp.link_do_planu = link;
            return result;
        }
        public void Sprawdz_plan()
        {
            if (!Czy_nowy_plan()) return;
            string link = ((Temp)pobrane_obiekty_xml[(int)Globalne.pliki_xml.Temp]).link_do_planu;
            string wiadomosc = "@everyone Pojawił się nowy plan zajęć!\nLink: "+ link;
            wb.Wyslij_do_webhooka(((Config)pobrane_obiekty_xml[(int)Globalne.pliki_xml.Config]).webhook_nowy_plan, wiadomosc);
            List<string> lista_planow = plk.Zwroc_liste_plikow(Globalne.lokalizacja + @"/" + Globalne.nazwy_folderow[(int)Globalne.foldery.Plany]);
            bool istnieje_nowy_plan=false;
            bool istnieje_stary_plan = false;
            if (lista_planow != null) 
                foreach(string nazwa in lista_planow)
                {
                    if (nazwa == Globalne.nazwy_plikow_planu[(int)Globalne.pliki_plany.Nowy]) istnieje_nowy_plan = true;
                    if (nazwa == Globalne.nazwy_plikow_planu[(int)Globalne.pliki_plany.Stary]) istnieje_stary_plan = true;
                }
            string stara_lokalizacja = Globalne.lokalizacja + @"/" + Globalne.nazwy_folderow[(int)Globalne.foldery.Plany] + @"/" + Globalne.nazwy_plikow_planu[(int)Globalne.pliki_plany.Stary];
            string nowa_lokalizacja = Globalne.lokalizacja + @"/" + Globalne.nazwy_folderow[(int)Globalne.foldery.Plany] + @"/" + Globalne.nazwy_plikow_planu[(int)Globalne.pliki_plany.Nowy];
            if (istnieje_stary_plan) plk.Usun_plik(stara_lokalizacja);
            if (istnieje_nowy_plan) plk.Zmien_nazwe(nowa_lokalizacja, stara_lokalizacja);
            wb.Pobierz_plik(link, nowa_lokalizacja);
        }
        public void Roznice_w_planie()
        {
            Excel ex = new Excel();
            string s=ex.Zwroc_roznice(5);
        }
    }
}

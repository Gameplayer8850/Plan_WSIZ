using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Dane.Plan;
using Dane;
using System.Data;

namespace Operacje
{
    class Excel
    {
        string[] dni_tyg = { "poniedziałek", "wtorek", "środa", "czwartek", "piątek"};
        public string Zwroc_roznice(int numer_semestru)
        {
            string result="";
            List<Tydzien> stary_plan = Zwroc_plan(Globalne.lokalizacja+@"/"+Globalne.nazwy_folderow[(int)Globalne.foldery.Plany]+@"/"+Globalne.nazwy_plikow_planu[(int)Globalne.pliki_plany.Stary] + Globalne.rozszerzenia_plikow_planu[(int)Globalne.pliki_plany.Stary], numer_semestru);
            List<Tydzien> nowy_plan = Zwroc_plan(Globalne.lokalizacja + @"/" + Globalne.nazwy_folderow[(int)Globalne.foldery.Plany] + @"/" + Globalne.nazwy_plikow_planu[(int)Globalne.pliki_plany.Nowy] + Globalne.rozszerzenia_plikow_planu[(int)Globalne.pliki_plany.Nowy], numer_semestru);
            List<Dzien> zmienione_dni = new List<Dzien>();
            if (stary_plan == null || nowy_plan == null) return result;
            foreach(Tydzien tyd in nowy_plan)
            {
                foreach(Tydzien tyd2 in stary_plan)
                {
                    if (tyd.data != tyd2.data) continue;
                    bool flaga = false;
                    foreach(Dzien dzien in tyd.dni_tygodnia)
                    {
                        foreach(Dzien dzien2 in tyd2.dni_tygodnia)
                        {
                            if (dzien.data != dzien2.data) continue;
                            flaga = true;
                            if (dzien.Czy_inny_plan(dzien2)) zmienione_dni.Add(dzien);
                        }
                        if(!flaga) zmienione_dni.Add(dzien);
                    }
                }
            }
            foreach (Dzien dzien in zmienione_dni) result += "`Zmiany w dniu: " + dzien.data+ " ("+dni_tyg[dzien.dzien]+")`\n";
            return result;
        }

        public List<Tydzien> Zwroc_plan(string nazwa_pliku, int numer_semestru)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            List<Tydzien> tygodnie = new List<Tydzien>();
            try
            {
                using (var stream = File.Open(nazwa_pliku, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        do
                        {
                            if (!reader.Name.Contains('-') || !reader.Name.Contains('.')) continue;
                            reader.Read();
                            int col_start = -1;
                            int col_stop = -1;
                            foreach (CellRange cell in reader.MergeCells)
                            {
                                if (cell.FromRow == 0)
                                {
                                    if (reader.GetValue(cell.FromColumn).ToString().ToLower() == "semestr " + numer_semestru)
                                    {
                                        col_start = cell.FromColumn;
                                        col_stop = cell.ToColumn;
                                    }
                                }
                            }
                            if (col_start == -1) continue;
                            Tydzien tyd = new Tydzien();
                            tyd.data = reader.Name;
                            Dzien dzien = null;
                            bool flaga = false;
                            while (reader.Read())
                            {
                                bool czy_pierwsze_puste = false;
                                string godzina = "";
                                for (int column = col_start; column < col_stop + 1; column++)
                                {
                                    string wartosc = reader.GetValue(column) == null ? "" : reader.GetValue(column).ToString();
                                    if (column == col_start)
                                    {
                                        if (wartosc.Trim() == "")
                                        {
                                            czy_pierwsze_puste = true;
                                            continue;
                                        }
                                        if (wartosc.Trim() != "" && flaga)
                                        {
                                            godzina = wartosc;
                                            continue;
                                        }
                                    }
                                    if (czy_pierwsze_puste)
                                    {
                                        int numer_dnia = Zwroc_numer_dnia(wartosc);
                                        if (numer_dnia == -1)
                                        {
                                            czy_pierwsze_puste = false;
                                            break;
                                        }
                                        if (dzien != null) tyd.dni_tygodnia.Add(dzien);
                                        flaga = true;
                                        dzien = new Dzien();
                                        dzien.dzien = numer_dnia;
                                        dzien.data = Zwroc_date_dnia(wartosc, numer_dnia);
                                        break;
                                    }
                                    if (!flaga || wartosc.Trim() == "" || wartosc.Trim() == "przerwa") continue;
                                    dzien.Dodaj_zajecie(godzina, wartosc);

                                }
                            }
                            if (dzien != null) tyd.dni_tygodnia.Add(dzien);
                            if (tyd.dni_tygodnia.Count != 0) tygodnie.Add(tyd);
                        } while (reader.NextResult());
                    }
                }
            }
            catch
            {
                return null;
            }
            return tygodnie;
        }

        public List<Elearning> Zwroc_zajecia_elearning_dla_grupy(string lokalizacja, DateTime data, int numer_semestru)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            List<Elearning> dane = new List<Elearning>();
            try
            {
                using (var stream = File.Open(lokalizacja, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        do
                        {
                            int col_semestr = -1;
                            int col__przedmiot = -1;
                            int col_data = -1;
                            int col_godz = -1;
                            int col_grupa = -1;
                            int col_link = -1;
                            bool first_row = true;
                            while (reader.Read())
                            {
                                Elearning el = new Elearning();
                                bool flaga = false;
                                for (int column = 0; column < reader.FieldCount; column++)
                                {
                                    string wartosc = reader.GetValue(column) == null ? "" : reader.GetValue(column).ToString();
                                    if (first_row)
                                    {
                                        switch (wartosc.ToLower().Trim())
                                        {
                                            case "semestr":
                                                col_semestr = column;
                                                break;
                                            case "przedmiot":
                                                col__przedmiot = column;
                                                break;
                                            case "dzień":
                                                col_data = column;
                                                break;
                                            case "godzina":
                                                col_godz = column;
                                                break;
                                            case "grupa":
                                                col_grupa = column;
                                                break;
                                            case "wideokonferencja":
                                                col_link = column;
                                                break;
                                        }

                                    }
                                    else
                                    {
                                        if (column == col_semestr)
                                        {
                                            if (!wartosc.Contains(numer_semestru.ToString()))
                                            {
                                                flaga = true;
                                                break;
                                            }
                                            el.numer_semestru = numer_semestru;
                                        }
                                        else if (column == col__przedmiot) el.nazwa_przedmiotu = wartosc;
                                        else if (column == col_data)
                                        {
                                            if (data.Date != Convert.ToDateTime(wartosc).Date)
                                            {
                                                flaga = true;
                                                break;
                                            }
                                            el.dzien = data;
                                        }
                                        else if (column == col_godz) el.godzina = wartosc;
                                        else if (column == col_grupa) el.grupa = wartosc;
                                        else if (column == col_link) el.link_do_zajec = wartosc;
                                        continue;
                                    }
                                }
                                if (col_semestr == -1 || col__przedmiot == -1 || col_data == -1 || col_godz == -1 || col_grupa == -1 || col_link == -1) return null;
                                if (first_row) 
                                {
                                    first_row= false;
                                    continue;
                                }
                                if (flaga) continue;
                                dane.Add(el);
                            }
                        } while (reader.NextResult());
                    }
                }
            }
            catch
            {
                return null;
            }
            return dane;
        }

        public int Zwroc_numer_dnia(string wartosc)
        {
            string dzien = wartosc.ToLower();
            for (int i = 0; i < dni_tyg.Length; i++) if (dzien.Contains(dni_tyg[i])) return i;
            return -1;
        }

        public string Zwroc_date_dnia(string wartosc, int dzien)
        {
            return wartosc.ToLower().Replace(dni_tyg[dzien], "").Trim();
        }

    }
}

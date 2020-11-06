using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Dane.Plan;
using Dane;

namespace Operacje
{
    class Excel
    {
        public string Zwroc_roznice(int numer_semestru)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            List<Tydzien> stary_plan = Zwroc_plan(Globalne.lokalizacja+@"/"+Globalne.nazwy_folderow[(int)Globalne.foldery.Plany]+@"/"+Globalne.nazwy_plikow_planu[(int)Globalne.pliki_plany.Stary] + ".xls", numer_semestru);
            return "";
        }

        public List<Tydzien> Zwroc_plan(string nazwa_pliku, int numer_semestru)
        {
            List<Tydzien> tygodnie = new List<Tydzien>();
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
                                if(reader.GetValue(cell.FromColumn).ToString().ToLower()=="semestr "+numer_semestru)
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
                        while (reader.Read()) //Each ROW
                        {
                            string godzina = "";
                            for (int column = col_start; column < col_stop+1; column++)
                            {
                                if (column == col_start)
                                {
                                    int numer_dnia = Zwroc_numer_dnia(reader.GetValue(column).ToString());
                                    if (numer_dnia != -1)
                                    {
                                        //if(dzien!=null) dzien
                                    }
                                }
                                //Console.WriteLine(reader.GetString(column));//Will blow up if the value is decimal etc. 
                                Console.WriteLine(reader.GetValue(column));//Get Value returns object
                            }
                        }
                    } while (reader.NextResult()); //Move to NEXT SHEET
                }
            }
            return tygodnie;
        }

        public int Zwroc_numer_dnia(string wartosc)
        {
            string dzien = wartosc.ToLower();
            if (dzien.Contains("poniedziałek")) return 0;
            if (dzien.Contains("wtorek")) return 1;
            if (dzien.Contains("środa")) return 2;
            if (dzien.Contains("czwartek")) return 3;
            if (dzien.Contains("piątek")) return 4;
            return -1;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Dane
{
    public class Globalne
    {
        public static string lokalizacja = AppDomain.CurrentDomain.BaseDirectory;
        public static List<string> nazwy_plikow = new List<string>() { "Config.xml", "Temp.xml" };
        public static List<string> nazwy_folderow = new List<string>() { "Dane", "Plany_Zajec", "E-learning" };
        public static List<string> nazwy_plikow_planu = new List<string>() {"nowy_plan", "stary_plan"};
        public static List<string> rozszerzenia_plikow_planu = new List<string>() { "", "" };
        public static List<Type> obiekty_xml = new List<Type>() { typeof(Dane.XML.Config), typeof(Dane.XML.Temp) };
        public enum foldery
        {
            Glowny, Plany, Elearning
        }
        public enum pliki_xml
        {
            Config, Temp
        }
        public enum pliki_plany
        {
            Nowy, Stary
        }
    }
}

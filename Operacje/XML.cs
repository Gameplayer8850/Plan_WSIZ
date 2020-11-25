using Dane;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Operacje
{
    class XML
    {
        string lokalizacja_danych = null;
        public XML()
        {
            lokalizacja_danych = Path.Combine(Globalne.lokalizacja, Globalne.nazwy_folderow[(int)Globalne.foldery.Glowny]);
        }
        public bool Czy_istnieje_plik_xml(Globalne.pliki_xml rodzaj)
        {
            Directory.CreateDirectory(lokalizacja_danych);
            return File.Exists(Path.Combine(lokalizacja_danych, Globalne.nazwy_plikow[(int)rodzaj]));
        }
        public Object Pobierz_dane(Globalne.pliki_xml rodzaj)
        {
            Type klasa = Globalne.obiekty_xml[(int)rodzaj];
            Object obj = null;
            try
            {
                using (var stream = new FileStream(Path.Combine(lokalizacja_danych, Globalne.nazwy_plikow[(int)rodzaj]), FileMode.Open))
                {
                    var XML = new System.Xml.Serialization.XmlSerializer(klasa);
                    obj = XML.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
            return obj;
        }
        public void Zapisz_dane(Object obj, Globalne.pliki_xml rodzaj)
        {
            Type klasa = Globalne.obiekty_xml[(int)rodzaj];
            using (var stream = new FileStream(Path.Combine(lokalizacja_danych, Globalne.nazwy_plikow[(int)rodzaj]), FileMode.Create))
            {
                var XML = new System.Xml.Serialization.XmlSerializer(klasa);
                XML.Serialize(stream, obj);
            }
        }
        public object Zapisz_szablon(Globalne.pliki_xml rodzaj)
        {
            object obj = Activator.CreateInstance(Dane.Globalne.obiekty_xml[(int)rodzaj]);
            ((Dane.XML.IXML)obj).Stworz_szablon();
            Zapisz_dane(obj, rodzaj);
            return obj;
        }
    }
}

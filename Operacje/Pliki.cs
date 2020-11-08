using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Operacje
{
    class Pliki
    {
        List<string> rozszerzenia = new List<string> { "xls", "xlsx"};
        public List<string> Zwroc_liste_plikow(string lokalizacja)
        {
            List<string> lista_plikow = null;
            try
            {
                Directory.CreateDirectory(lokalizacja);
                lista_plikow = Directory.EnumerateFiles(lokalizacja, "*.*", SearchOption.AllDirectories).Where(s => rozszerzenia.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant())).ToList<string>();
                return lista_plikow;
            }
            catch
            {
                return null;
            }
        }
        public bool Usun_plik(string lokalizacja)
        {
            try
            {
                File.Delete(lokalizacja);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Zmien_nazwe(string stara_lokalizacja, string nowa_lokalizacja)
        {
            try
            {
                File.Move(stara_lokalizacja, nowa_lokalizacja);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace Operacje
{
    class Web
    {
        public void Wyslij_do_webhooka(string url, string wiadomosc)
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    web.UploadValues(url, new NameValueCollection() {{ "content", wiadomosc}});
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Nie udało się wysłać wiadomości");
            }
        }
        public bool Pobierz_plik(string url, string lokalizacja)
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    web.DownloadFile(url, lokalizacja);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public List<List<string>> Zwroc_dane_o_najnowszym_planie(string url)
        {
            try
            {
                HtmlDocument document = new HtmlDocument();
                string strona = "";
                using (WebClient web = new WebClient())
                {
                    strona = web.DownloadString(url);
                }

                document.LoadHtml(strona);

                List<List<string>> table = document.DocumentNode.SelectNodes("//table[@class='table table-striped']")[1]
                .Descendants("tr")
                .Where(tr => tr.Elements("td").Count() > 1)
                .Select(tr => tr.Elements("td").Select(td =>  (td.FirstChild.Attributes.Count > 0 ? td.FirstChild.Attributes[0].Value.ToString(): (td.InnerText.Trim()))).ToList())
                //.FirstOrDefault(null)
                .ToList();

                return table;
            }
            catch
            {
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using ScrapySharp.Extensions;

namespace Operacje
{
    class Web
    {
        public void Wyslij_do_webhooka(string url, string wiadomosc)
        {
            using (WebClient web = new WebClient())
            {
                web.UploadValues(url, new NameValueCollection() {
                { "content", wiadomosc}
            });
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
        public void Zwroc_tabele(string link)
        {
            WebClient webClient = new WebClient();
            string page = webClient.DownloadString(link);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(page);

            var table = doc.DocumentNode.CssSelect("table");
            System.Diagnostics.Debug.WriteLine(table.ToString());
            //((HtmlAgilityPack.HtmlNode)table).ge
            /*
            List<List<string>> table = doc.DocumentNode.SelectSingleNode("//table[@class='mydata']")
                        .Descendants("tr")
                        .Skip(1)
                        .Where(tr => tr.Elements("td").Count() > 1)
                        .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                        .ToList();
            */
        }
        public string Zwroc_link_do_planu(string url)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(url);
            return document.DocumentNode.CssSelect("table.table table-striped").ToString();
        }
    }
}

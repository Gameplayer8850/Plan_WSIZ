using System;
using System.Collections.Generic;
using System.Text;

namespace Dane.XML
{
    public class Temp : IXML
    {
        public string link_do_planu { get; set; } = null;
        public DateTime data_dodania { get; set; } = default(DateTime);
        public void Stworz_szablon()
        {
            link_do_planu = "";
            data_dodania = default(DateTime);
        }
    }
}

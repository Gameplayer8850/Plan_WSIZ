using System;
using System.Collections.Generic;
using System.Text;

namespace Dane.XML
{
    public class Config : IXML
    {
        public string link_do_strony_z_planem { get; set; } = null;
        public string webhook_nowy_plan { get; set; } = null;
        public string webhook_grupa_A { get; set; } = null;
        public string webhook_grupa_B { get; set; } = null;
        public int semestr { get; set; } = 0;

        public void Stworz_szablon()
        {
            link_do_strony_z_planem = "[LINK DO ZAKŁADKI Z PLANAMI ZAJĘĆ]";
            webhook_nowy_plan = "[WEBHOOK DO INFORMACJI O NOWYM PLANIE]";
            webhook_grupa_A = "[WEBHOOK DO WYSYŁANIE LINKÓW DO ZAJĘĆ DLA GRUPY A]";
            webhook_grupa_B = "[WEBHOOK DO WYSYŁANIE LINKÓW DO ZAJĘĆ DLA GRUPY B]";
            semestr = -1;
        }

    }
}

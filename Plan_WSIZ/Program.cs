using System;
using Operacje;

namespace Plan_WSIZ
{
    class Program
    {
        static void Main(string[] args)
        {
            Zarzadzanie zar = new Zarzadzanie();
            //zar.Podmien_rozszerzenia_plikow();
            //zar.Roznice_w_planie();
            //if (!zar.Pobierz_config()) return;
            string cos=zar.Zwroc_link_plan("https://www.wsiz.wroc.pl/plany-zajec/");


            foreach (string argument in args)
            {
                switch (argument)
                {
                    case "nowy_plan":

                        break;
                }
            }
            Console.WriteLine("Hello World!");
        }
    }
}

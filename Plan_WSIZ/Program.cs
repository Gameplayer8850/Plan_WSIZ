using System;
using Operacje;

namespace Plan_WSIZ
{
    class Program
    {
        static void Main(string[] args)
        {
            Zarzadzanie zar = new Zarzadzanie();
            if (!zar.Pobierz_dane_poczatkowe()) return;
            zar.Podmien_rozszerzenia_plikow();

            foreach (string argument in args)
            {
                switch (argument)
                {
                    case "-s":
                        zar.Sprawdz_plan();
                        break;
                    case "-sr":
                        zar.Sprawdz_plan(true);
                        break;
                    case "-r":
                        zar.Roznice_w_planie();
                        break;
                    case "-e":
                        zar.Elearning_dla_grup();
                        break;
                    case "-e2":
                        zar.Nowy_elearning();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

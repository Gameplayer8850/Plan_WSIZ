using System;
using Operacje;

namespace Plan_WSIZ
{
    class Program
    {
        static void Main(string[] args)
        {
            Zarzadzanie zar = new Zarzadzanie();
            zar.Roznice_w_planie();
            //if (!zar.Pobierz_config()) return;


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

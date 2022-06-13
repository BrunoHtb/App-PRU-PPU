using System;
using System.Collections.Generic;
using System.Text;

namespace rodovias.ViewModels
{
    public class ClsDadosTemporarios
    {

        // variaveis temporario para recuperar na tela de cadastro
        public static string tmpRegional_der { get; set; }
        public static string tmpRodovia { get; set; }

        // incluir nomes de todos os txts que o usuario gravou
        static List<string> _list;
        static ClsDadosTemporarios()
        {
            _list = new List<string>();
        }

        public static void GravarNaLisa(string valor)
        {
            _list.Add(valor);
        }

        public static void Visualizar()
        {
            foreach (var valor in _list)
            {
                Console.WriteLine(valor);
            }
        }
       
    }
}

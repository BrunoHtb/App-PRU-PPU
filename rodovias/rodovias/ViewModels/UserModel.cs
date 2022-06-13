using System;
using System.Collections.Generic;
using System.Text;

namespace rodovias.ViewModels
{
    public class UserModel
    {
        public string CaminhoAppArmazenaFotosPRU = @"/storage/emulated/0/Android/data/com.esteio.rodoviasPRUePPU/files/" + "Pictures/PRU";
        public string CaminhoAppArmazenaFotosPPU = @"/storage/emulated/0/Android/data/com.esteio.rodoviasPRUePPU/files/" + "Pictures/PPU";

        public int ds_id { get; set; }
        public string regional_der { get; set; }
        public string rodovia { get; set; }
        //public string PrefixoRodovia { get; set; }
        public string codigo { get; set; }
        public string lado { get; set; }
        public string sentido { get; set; }
        public string diamesano { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string latitude_fim { get; set; }
        public string longitude_fim { get; set; }
        public string nome_fotopanoramica { get; set; }
        public string nome_fotodetalhe_1 { get; set; }
        public string nome_fotodetalhe_2 { get; set; }
        public string condicao { get; set; }
        public string observacao { get; set; }
        public string usuariologado { get; set; }
        public string status_interno { get; set; }
        public string datainclusaonovo { get; set; }
        public string app_numero_dipositivo { get; set; }
        //public string regional_cidade { get; set; }
        public string kmInicio { get; set; }
        public string kmFim { get; set; }

        public string atendimentoNorma { get; set; }
        public string sinalizacaoVertical { get; set; }
        public string auditoria { get; set; }
        public string id { get; set; }
        public int valorLista { get; set; }
        public string tipoFaixa { get; set; }

    }
}

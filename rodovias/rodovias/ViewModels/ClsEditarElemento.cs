using Npgsql;
using System;
using Xamarin.Essentials;

namespace rodovias.ViewModels
{
    public class ClsEditarElemento
    {
        public static string Regional_der { get; set; }
        public static string PrefixoRodovia { get; set; }
        public static string Rodovia { get; set; }
        public static string Regional { get; set; }
        public static string Lado { get; set; }
        public static string Sentido { get; set; }
        public static string Condicao { get; set; }
        public static string LatitudeInicio { get; set; }
        public static string LatitudeFim { get; set; }
        public static string LongitudeInicio { get; set; }
        public static string LongitudeFim { get; set; }
        public static string ObservacaoCampo { get; set; }
        public static string CodigoElemento { get; set; }
        public static string nome_fotopanoramica { get; set; }
        public static string nome_fotodetalhe_1 { get; set; }
        public static string nome_fotodetalhe_2 { get; set; }
        public static string status_interno { get; set; }

        private string numDispositivo = Preferences.Get("IdDispositivo", string.Empty);
        private string NomeUsuario = Preferences.Get("NomeUsuario", string.Empty);

        public static string KmInicio { get; set; }
        public static string KmFim { get; set; }

        public static string SinalizaçãoVertical { get; set; }
        public static string AtendimentoNorma { get; set; }
        public static string Auditoria { get; set; }
        public static string ID { get; set; }
        public static string  TipoFaixa { get; set; }

        public void UpdateRegistro(string regional_der, string rodovia, string codigo, string lado, string sentido, string latitude, string longitude, string latitude_fim, string longitude_fim, string nome_fotopanoramica, string nome_fotodetalhe_1, string nome_fotodetalhe_2, string condicao, string observacao, string _regionalCidade, string codigoAntigo, string _kmInicio, string _KmFim)
        {
            DAL.DAL.Conectar();
            string cmdInsert = string.Format($"UPDATE tb_dispositivosdeseguranca SET regional_der='{regional_der}', rodovia='{rodovia}', codigo='{codigo}', lado='{lado}',sentido='{sentido}', alteracao_dia='{DateTime.Now.ToString("dd-MM-yyyy")}', latitude='{latitude}', longitude='{longitude}', latitude_fim='{latitude_fim}', longitude_fim='{longitude_fim}', nome_fotopanoramica='{nome_fotopanoramica}', nome_fotodetalhe_1='{nome_fotodetalhe_1}', nome_fotodetalhe_2='{nome_fotodetalhe_2}', condicao='{condicao}', observacao='{observacao}', usuariologado='{NomeUsuario}', status_interno='ATUALIZADO-APP', app_numero_dispositivo='{numDispositivo}', regional_cidade='{_regionalCidade}', km_inicio='{_kmInicio}', km_fim='{_KmFim}' WHERE codigo='{codigoAntigo}';");
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(cmdInsert, DAL.DAL.Conn);
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                App.Current.MainPage.DisplayAlert("Erro Conexão", "Erro ao atualizar " + ex.Message.ToString() + "\n\r contate o suporte", "OK");
                throw ex;
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Erro Conexão", "Erro ao atualizar " + ex.Message.ToString(), "OK");
                return;
            }
            finally
            {
                DAL.DAL.Desconectar();
            }

        }
    }
}

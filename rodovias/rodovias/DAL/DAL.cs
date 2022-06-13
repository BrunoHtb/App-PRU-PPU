using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;
using Microsoft.VisualBasic;
using System.IO;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using rodovias.ViewModels;
using System.Collections.ObjectModel;
using PCLExt.FileStorage.Folders;
using System.Threading.Tasks;
using System.Linq;

namespace rodovias.DAL
{
    public class DAL
    {
        ObservableCollection<UserModel> ListarElementosExterno = new ObservableCollection<UserModel>();

        private static string _servidor = "177.220.159.198"; // ip
        private static string _porta = "5432"; // porta de acesso default
        private static string usuario = "postgres"; // nome do administrador 
        private static string password = "cadastro"; // senha de acesso ao bando de dados
        private static string databaseName = "Esteio"; // nome do banco de dados
        //NpgsqlConnection pgsqlConnection = null;
        private static string connString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", _servidor, _porta, usuario, password, databaseName);

        public static NpgsqlConnection Conn { get; set; }
        public DAL()
        {
            //var con = new NpgsqlConnection(cs);
            //connString = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", serverName,port, userNameme, password, databaseName);
        }
        // Inserir novo Registro
        public static void Conectar()
        {
            try
            {
                Conn = new NpgsqlConnection(connString);
                Conn.Open();
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Erro", "Erro ao conectar" + ex.Message.ToString(), "OK");
                return;
            }
        }

        public static void Desconectar()
        {
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
        }
        public bool JaExiste(string _codigo)
        {
            bool bExiste = false;
            try
            {
                Conectar();
                using (NpgsqlCommand comm = new NpgsqlCommand($"SELECT count(codigo) from tb_dispositivosdeseguranca where codigo='{_codigo}';", Conn))
                {
                    Int32 count = Convert.ToInt32(comm.ExecuteScalar());
                    if(count > 0)
                    {
                        bExiste = true;
                    } else
                    {
                        bExiste = false;
                    }
                }
            } catch(NpgsqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Desconectar();
            }
           
            return bExiste;
        }

        public void ImportarRegistrosPlanejadosPPU()
        {
            try
            {
                Conectar();
                var root = new LocalRootFolder();
                string path = root.Path + "/files/Pictures/PPU";

                using (NpgsqlCommand comm = new NpgsqlCommand($"SELECT regional_der, rodovia, codigo, lado,sentido, dia_mes_ano, latitude_inicio, longitude_inicio, latitude_fim, longitude_fim, nome_fotopanoramica, nome_fotodetalhe_1, nome_fotodetalhe_2, condicao, observacao_ec, km_inicio, km_fim, status_interno, usuario_logado, numero_dispositivo FROM tb_ppu WHERE numero_dispositivo='{Preferences.Get("IdDispositivo", string.Empty)}' AND status_interno='{"PLANEJADO"}';", Conn))
                {
                    NpgsqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        string lines = string.Format(
                            reader.GetString(0) + ";" + reader.GetString(1) + ";" +
                            reader.GetString(2) + ";" + reader.GetString(3) + ";" +
                            reader.GetString(4) + ";" + reader.GetString(5) + ";" +
                            reader.GetString(6) + ";" + reader.GetString(7) + ";" +
                            reader.GetString(8) + ";" + reader.GetString(9) + ";" +
                            "" + ";" + "" + ";" +
                            "" + ";" + reader.GetString(13) + ";" +
                            reader.GetString(14) + ";" + reader.GetString(15) + ";" +
                            reader.GetString(16) + ";" + reader.GetString(17) + ";" +
                            reader.GetString(18) + ";" + reader.GetString(19));

                        string fileName = reader.GetString(0) + "_" + reader.GetString(1) + "_" + "PPU" + "_" + reader.GetString(15) + "_" + reader.GetString(16) + "_" + reader.GetString(4) + "_" + reader.GetString(3) + ".txt";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        string pathFileName = System.IO.Path.Combine(path, fileName);
                        using (var streamWriter = new System.IO.StreamWriter(pathFileName, false))
                        {
                            streamWriter.WriteLine(lines);
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Desconectar();
            }
        }

        public void ImportarRegistrosPlanejadosPRU()
        {
            try
            {               
                Conectar();
                var root = new LocalRootFolder();
                string path = root.Path + "/files/Pictures/PRU";

                using (NpgsqlCommand comm = new NpgsqlCommand($"SELECT regional_der, rodovia, codigo_1, lado,sentido, diamesano, latitude, longitude, latfim, longfim, nome_fotopanoramica, nome_fotodetalhe_1, nome_fotodetalhe_2, condicao, observacao_ec, km_inicio, km_fim, status_interno, sinalizacaovertical, atendimentonorma, auditoria, usuario_logado, numero_dispositivo, ru_id, tipo FROM tb_restricaodeultrapassagem WHERE numero_dispositivo='{Preferences.Get("IdDispositivo", string.Empty)}' AND status_interno='{"PLANEJADO"}';", Conn))
                {
                    NpgsqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        string lines = string.Format(
                            reader.GetString(0) + ";" + reader.GetString(1) + ";" +
                            reader.GetString(2) + ";" + reader.GetString(3) + ";" +
                            reader.GetString(4) + ";" + reader.GetString(5) + ";" +
                            reader.GetString(6) + ";" + reader.GetString(7) + ";" +
                            reader.GetString(8) + ";" + reader.GetString(9) + ";" +
                            "" + ";" + "" + ";" +
                            "" + ";" + reader.GetString(13) + ";" +
                            reader.GetString(14) + ";" + reader.GetString(15) + ";" +
                            reader.GetString(16) + ";" + reader.GetString(17) + ";" +
                            reader.GetString(18) + ";" + reader.GetString(19) + ";" +
                            reader.GetInt32(20) + ";" + reader.GetString(21) + ";" +
                            reader.GetString(22) + ";" + reader.GetInt32(23)) + ";" +
                            reader.GetString(24);

                        string fileName = reader.GetString(0) + "_" + reader.GetString(1) + "_" + "PRU" + "_" + reader.GetString(15) + "_" + reader.GetString(16) + "_" + reader.GetString(4) + "_" + reader.GetString(3) + "_PLANEJADO.txt";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        string pathFileName = System.IO.Path.Combine(path, fileName);
                        using (var streamWriter = new System.IO.StreamWriter(pathFileName, false))
                        {
                            streamWriter.WriteLine(lines);
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Desconectar();
            }
        }

        private void OnPropertyChanged()
        {
            throw new NotImplementedException();
        }

        private Task DisplayAlert(string v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }

        private void Aguarde()
        {
            throw new NotImplementedException();
        }

        public void InserirNovoRegistroPRU(string regional_der, string rodovia, string codigo, string lado, string sentido, string diamesano, string latitude, string longitude, string latitude_fim, string longitude_fim, string nome_fotopanoramica, string nome_fotodetalhe_1, string nome_fotodetalhe_2, string condicao, string observacao, string _kmInicio, string _kmFim, string sinalizacaoVertical, string atendimentoNorma, int auditoria, string usuariologado, string app_numero_dispositivo, string tipoFaixa)
        {
            Conectar();
            int ultPK = 0;
            string datainclusaonovo = DateTime.Now.ToString("dd/MM/yyyy");
            using (NpgsqlCommand cmdSerial = new NpgsqlCommand("SELECT max(ru_id) from tb_restricaodeultrapassagem;", Conn))
            {
                NpgsqlDataReader reader = cmdSerial.ExecuteReader();
                while (reader.Read())
                {
                    ultPK = Int32.Parse(reader[0].ToString());
                }
                Desconectar();
            }

            Conectar();
            string cmdVerificaRepetido = "SELECT COUNT(*) from tb_restricaodeultrapassagem WHERE codigo_1='" + codigo + "';";
            Int64 countRows;
            using (NpgsqlCommand cmdContador = new NpgsqlCommand(cmdVerificaRepetido, Conn))
            {
                countRows = (Int64)cmdContador.ExecuteScalar();
                Desconectar();
            }

            if(countRows == 0)
            {
                Conectar();
                string cmdInsert = string.Format($"INSERT INTO tb_restricaodeultrapassagem (ru_id, regional_der, rodovia, codigo_1, lado,sentido, diamesano, latitude, longitude, latfim, longfim, nome_fotopanoramica, nome_fotodetalhe_1, nome_fotodetalhe_2, condicao, observacao_ec, usuario_logado, status_interno, inclusaodianovo, numero_dispositivo, km_inicio, km_fim,  sinalizacaovertical, atendimentonorma, tipo ) VALUES ({ultPK + 1},'{regional_der}','{rodovia}', '{codigo}', '{lado}', '{sentido}', '{diamesano}', '{latitude}', '{longitude}', '{latitude_fim}', '{longitude_fim}', '{nome_fotopanoramica}', '{nome_fotodetalhe_1}', '{nome_fotodetalhe_2}', '{condicao}', '{observacao}', '{usuariologado}', 'NOVO', '{datainclusaonovo}', '{app_numero_dispositivo}', '{_kmInicio}', '{_kmFim}', '{sinalizacaoVertical}', '{atendimentoNorma}', '{tipoFaixa}')");
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cmdInsert, Conn);
                    cmd.ExecuteNonQuery();
                }
                catch (NpgsqlException ex)
                {
                    App.Current.MainPage.DisplayAlert("Erro Conexão", "Erro ao inserir " + ex.Message.ToString() + "\n\r contate o suporte", "OK");
                    return;
                }
                catch (Exception ex)
                {
                    App.Current.MainPage.DisplayAlert("Erro Conexão", "Erro ao inserir " + ex.Message.ToString(), "OK");
                    throw ex;
                }
                finally
                {
                    Desconectar();
                }
            } 
        }

        public void AtualizarRegistroPRU(string regional_der, string rodovia, string codigo, string lado, string sentido, string diamesano, string latitude, string longitude, string latitude_fim, string longitude_fim, string nome_fotopanoramica, string nome_fotodetalhe_1, string nome_fotodetalhe_2, string condicao, string observacao, string _kmInicio, string _kmFim, string sinalizacaoVertical, string atendimentoNorma, int auditoria, string usuariologado, string app_numero_dispositivo, int id, string tipoFaixa)
        {
            string datainclusaonovo = DateTime.Now.ToString("dd/MM/yyyy");
            Conectar();
            string cmdInsert = string.Format($"UPDATE tb_restricaodeultrapassagem SET regional_der='{regional_der}', rodovia='{rodovia}', codigo_1='{codigo}', lado='{lado}', sentido='{sentido}', diamesano='{diamesano}', latitude='{latitude}', longitude='{longitude}', latfim='{latitude_fim}', longfim='{longitude_fim}', nome_fotopanoramica='{nome_fotopanoramica}', nome_fotodetalhe_1='{nome_fotodetalhe_1}', nome_fotodetalhe_2='{nome_fotodetalhe_2}', condicao='{condicao}', observacao_ec='{observacao}', usuario_logado='{usuariologado}', status_interno='NOVO', inclusaodianovo='{datainclusaonovo}', numero_dispositivo='{app_numero_dispositivo}', km_inicio='{_kmInicio}', km_fim='{_kmFim}', auditoria={auditoria}, sinalizacaovertical='{sinalizacaoVertical}', atendimentonorma='{atendimentoNorma}', tipo='{tipoFaixa}' WHERE ru_id={id}; ");
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(cmdInsert, Conn);
                cmd.ExecuteNonQuery();              
            }
            catch (NpgsqlException ex)
            {
                App.Current.MainPage.DisplayAlert("Erro Conexão", "Erro ao inserir " + ex.Message.ToString() + "\n\r contate o suporte", "OK");
                return;
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Erro Conexão", "Erro ao inserir " + ex.Message.ToString(), "OK");
                throw ex;
            }
            finally
            {
                Desconectar();
            }
        }

        public void AtualizarRegistroPPU(string regional_der, string rodovia, string codigo, string lado, string sentido, string diamesano, string latitude, string longitude, string latitude_fim, string longitude_fim, string nome_fotopanoramica, string nome_fotodetalhe_1, string nome_fotodetalhe_2, string condicao, string observacao, string _kmInicio, string _kmFim, string _status, int _auditoria, string usuariologado, string app_numero_dispositivo, int id)
        {
            string datainclusaonovo = DateTime.Now.ToString("dd/MM/yyyy");
            Conectar();
            string cmdInsert = string.Format($"UPDATE tb_ppu SET regional_der='{regional_der}', rodovia='{rodovia}', codigo='{codigo}', lado='{lado}', sentido='{sentido}', dia_mes_ano='{diamesano}', latitude_inicio='{latitude}', longitude_inicio='{longitude}', latitude_fim='{latitude_fim}', longitude_fim='{longitude_fim}', nome_fotopanoramica='{nome_fotopanoramica}', nome_fotodetalhe_1='{nome_fotodetalhe_1}', nome_fotodetalhe_2='{nome_fotodetalhe_2}', condicao='{condicao}', observacao_ec='{observacao}', usuario_logado='{usuariologado}', status_interno='NOVO', inclusaodianovo='{datainclusaonovo}', numero_dispositivo='{app_numero_dispositivo}', km_inicio='{_kmInicio}', km_fim='{_kmFim}', auditoria={_auditoria},' WHERE ppuu_id={id}; ");
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(cmdInsert, Conn);
                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                App.Current.MainPage.DisplayAlert("Erro Conexão", "Erro ao inserir " + ex.Message.ToString() + "\n\r contate o suporte", "OK");
                return;
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Erro Conexão", "Erro ao inserir " + ex.Message.ToString(), "OK");
                throw ex;
            }
            finally
            {
                Desconectar();
            }
        }

        public void InserirNovoRegistroPPU(string regional_der, string rodovia, string codigo, string lado, string sentido, string diamesano, string latitude, string longitude, string latitude_fim, string longitude_fim, string nome_fotopanoramica, string nome_fotodetalhe_1, string nome_fotodetalhe_2, string condicao, string observacao, string _kmInicio, string _kmFim, string _status, int _auditoria, string usuariologado, string app_numero_dispositivo)
        {
            Conectar();
            int ultPK = 0;
            string datainclusaonovo = DateTime.Now.ToString("dd/MM/yyyy");
            regional_der = "DR" + regional_der;
            using (NpgsqlCommand cmdSerial = new NpgsqlCommand("SELECT max(ppu_id) from tb_ppu;", Conn))
            {
                NpgsqlDataReader reader = cmdSerial.ExecuteReader();
                while (reader.Read())
                {
                    ultPK = Int32.Parse(reader[0].ToString());
                }
                Desconectar();
            }
            Conectar();
            string cmdVerificaRepetido = "SELECT COUNT(*) from tb_restricaodeultrapassagem WHERE codigo='" + codigo + "';";
            Int64 countRows;
            using (NpgsqlCommand cmdContador = new NpgsqlCommand(cmdVerificaRepetido, Conn))
            {
                countRows = (Int64)cmdContador.ExecuteScalar();
                Desconectar();
            }
            if(countRows == 0)
            {
                Conectar();
                string cmdInsert = string.Format($"INSERT INTO tb_ppu (ppu_id, regional_der, rodovia, codigo, lado,sentido, dia_mes_ano, latitude_inicio, longitude_inicio, latitude_fim, longitude_fim, nome_fotopanoramica, nome_fotodetalhe_1, nome_fotodetalhe_2, condicao, observacao_ec, usuario_logado, status_interno, inclusaodianovo, numero_dispositivo, km_inicio, km_fim, auditoria ) VALUES ({ultPK + 1},'{regional_der}','{rodovia}', '{codigo}', '{lado}', '{sentido}', '{diamesano}', '{latitude}', '{longitude}', '{latitude_fim}', '{longitude_fim}', '{nome_fotopanoramica}', '{nome_fotodetalhe_1}', '{nome_fotodetalhe_2}', '{condicao}', '{observacao}', '{usuariologado}', '{_status}', '{datainclusaonovo}', '{app_numero_dispositivo}', '{_kmInicio}', '{_kmFim}', '{_auditoria}');");
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(cmdInsert, Conn);
                    cmd.ExecuteNonQuery();
                }
                catch (NpgsqlException ex)
                {
                    App.Current.MainPage.DisplayAlert("Erro Conexão", "Erro ao inserir " + ex.Message.ToString() + "\n\r contate o suporte", "OK");
                    return;
                }
                catch (Exception ex)
                {
                    App.Current.MainPage.DisplayAlert("Erro Conexão", "Erro ao inserir " + ex.Message.ToString(), "OK");
                    throw ex;
                }
                finally
                {
                    Desconectar();
                }
            }
            
        }

        public int totalResultado { get; private set; }
        private string numDispositivo = Preferences.Get("IdDispositivo", string.Empty);

        public ObservableCollection<UserModel> ObterRegistro(string _caminhoDoArquivoTxt, int valor)
        {
            string[] line = File.ReadAllLines(_caminhoDoArquivoTxt);
            for(int i = 0; i < line.Length; i++)
            {
                try
                {
                    StringBuilder b = new StringBuilder(line[i]);
                    string a = b.ToString();
                    string[] words = a.Split(new char[] {';'}, StringSplitOptions.None);
                    this.ListarElementosExterno.Add(new UserModel
                    {
                        valorLista = valor,
                        regional_der = words[0].ToString(),
                        rodovia = words[1].ToString(),
                        codigo = words[2].ToString(),
                        lado = words[3].ToString(),
                        sentido = words[4].ToString(),
                        diamesano = words[5].ToString(),
                        latitude = words[6].ToString(),
                        longitude = words[7].ToString(),
                        latitude_fim = words[8].ToString(),
                        longitude_fim = words[9].ToString(),
                        nome_fotopanoramica = words[10].ToString(),
                        nome_fotodetalhe_1 = words[11].ToString(),
                        nome_fotodetalhe_2 = words[12].ToString(),
                        condicao = words[13].ToString(),
                        observacao = words[14].ToString(),   
                        kmInicio = words[15].ToString(),
                        kmFim = words[16].ToString(),
                        status_interno = words[17].ToString(),
                        sinalizacaoVertical = words[18].ToString(),
                        atendimentoNorma = words[19].ToString(),
                        auditoria = words[20].ToString(),
                        id = words[23].ToString(),
                        tipoFaixa = words[24].ToString(),
                    });
                } catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            totalResultado = ListarElementosExterno.Count;

            return ListarElementosExterno;
        }

        private ObservableCollection<UserModel> _items = new ObservableCollection<UserModel>();
        public ObservableCollection<UserModel> MeuModelo
        {
            get { return _items; }
        }

        public ObservableCollection<UserModel>ObterRegistroOrdenado(string[] _caminhoDoArquivoTxt, string tipoOrdenacao, string projeto)
        {
            Console.WriteLine("Tamanho linha: " + _caminhoDoArquivoTxt.Length);

            for (int i = 0; i < _caminhoDoArquivoTxt.Length; i++)
            {
                FileInfo fi = new FileInfo(Path.GetFullPath(_caminhoDoArquivoTxt[i]));
                string[] line = File.ReadAllLines(_caminhoDoArquivoTxt[i]);

                for (int f = 0; f < line.Length; f++)
                {
                    try
                    {
                        StringBuilder b = new StringBuilder(line[f]);
                        string a = b.ToString();
                        string[] words = a.Split(new char[] { ';' }, StringSplitOptions.None);
                        
                        if(projeto == "PRU")
                        {
                            this.ListarElementosExterno.Add(new UserModel
                            {
                                valorLista = i,
                                regional_der = words[0].ToString(),
                                rodovia = words[1].ToString(),
                                codigo = words[2].ToString(),
                                lado = words[3].ToString(),
                                sentido = words[4].ToString(),
                                diamesano = words[5].ToString(),
                                latitude = words[6].ToString(),
                                longitude = words[7].ToString(),
                                latitude_fim = words[8].ToString(),
                                longitude_fim = words[9].ToString(),
                                nome_fotopanoramica = words[10].ToString(),
                                nome_fotodetalhe_1 = words[11].ToString(),
                                nome_fotodetalhe_2 = words[12].ToString(),
                                condicao = words[13].ToString(),
                                observacao = words[14].ToString(),
                                kmInicio = words[15].ToString(),
                                kmFim = words[16].ToString(),
                                status_interno = words[17].ToString(),
                                sinalizacaoVertical = words[18].ToString(),
                                atendimentoNorma = words[19].ToString(),
                                auditoria = words[20].ToString(),
                                id = words[23].ToString(),
                                tipoFaixa = words[24].ToString()
                            });
                        }
                        else if(projeto == "PPU")
                        {
                            this.ListarElementosExterno.Add(new UserModel
                            {
                                regional_der = words[0].ToString(),
                                rodovia = words[1].ToString(),
                                codigo = words[2].ToString(),
                                lado = words[3].ToString(),
                                sentido = words[4].ToString(),
                                diamesano = words[5].ToString(),
                                latitude = words[6].ToString(),
                                longitude = words[7].ToString(),
                                latitude_fim = words[8].ToString(),
                                longitude_fim = words[9].ToString(),
                                nome_fotopanoramica = words[10].ToString(),
                                nome_fotodetalhe_1 = words[11].ToString(),
                                nome_fotodetalhe_2 = words[12].ToString(),
                                condicao = words[13].ToString(),
                                observacao = words[14].ToString(),
                                kmInicio = words[15].ToString(),
                                kmFim = words[16].ToString(),
                                status_interno = words[17].ToString(),
                                auditoria = words[18].ToString(),
                                id = words[21].ToString(),
                            });
                        }
                        
                    }
                    catch (Exception ex) 
                    { 
                        Console.WriteLine(ex.Message); 
                    }          
                }
            }

            ObservableCollection<UserModel> users;
            if (tipoOrdenacao == "STATUS")
            {
                users = new ObservableCollection<UserModel>(this.ListarElementosExterno.OrderBy(X => X.status_interno));
            }
            else
            {
                users = new ObservableCollection<UserModel>(this.ListarElementosExterno.OrderBy(X => X.kmInicio));
            }
            totalResultado = ListarElementosExterno.Count;

            return users;
        }



        public ObservableCollection<UserModel> ObterRegistroPPU(string _caminhoDoArquivoTxt)
        {
            string[] line = File.ReadAllLines(_caminhoDoArquivoTxt);

            for (int i = 0; i < line.Length; i++)
            {
                try
                {
                    StringBuilder b = new StringBuilder(line[i]);
                    string a = b.ToString();
                    string[] words = a.Split(new char[] { ';' }, StringSplitOptions.None);
                    this.ListarElementosExterno.Add(new UserModel
                    {
                        regional_der = words[0].ToString(),
                        rodovia = words[1].ToString(),
                        codigo = words[2].ToString(),
                        lado = words[3].ToString(),
                        sentido = words[4].ToString(),
                        diamesano = words[5].ToString(),
                        latitude = words[6].ToString(),
                        longitude = words[7].ToString(),
                        latitude_fim = words[8].ToString(),
                        longitude_fim = words[9].ToString(),
                        nome_fotopanoramica = words[10].ToString(),
                        nome_fotodetalhe_1 = words[11].ToString(),
                        nome_fotodetalhe_2 = words[12].ToString(),
                        condicao = words[13].ToString(),
                        observacao = words[14].ToString(),
                        kmInicio = words[15].ToString(),
                        kmFim = words[16].ToString(),
                        status_interno = words[17].ToString(),
                        auditoria = words[18].ToString(),
                        id = words[21].ToString(),
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            totalResultado = ListarElementosExterno.Count;
            return ListarElementosExterno;
        }

        public void AtualizarRegistro(string _regional, string _rodovia, string _codigoElemento, string _status)
        {
            try
            {
                using (NpgsqlConnection pgsqlConnection = new NpgsqlConnection(connString))
                {
                    // Abrir conexao com o postgres
                    pgsqlConnection.Open();

                    string cmdUpdate = string.Format($"UPDATE tb_dispositivosdeseguranca SET regional='{_regional}', rodovia='{_rodovia}', codigo_elemento='{_codigoElemento}', status='{_status}')");
                    using (NpgsqlCommand pgsqlcommand = new NpgsqlCommand(cmdUpdate, pgsqlConnection))
                    {
                        pgsqlcommand.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                App.Current.MainPage.DisplayAlert("Erro Conexão", "Erro ao atualizar elemento" + ex.Message.ToString(), "OK");
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Desconectar();
            }
        }
    }
}

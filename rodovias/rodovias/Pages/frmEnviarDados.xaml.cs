using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using rodovias.ViewModels;
using Xamarin.Forms;
using System.Net;
using Xamarin.Forms.Xaml;
using System.Threading;

namespace rodovias.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class frmEnviarDados : ContentPage
    {
        UserModel ModeloUsuario = new UserModel();
        int totalArquivosNoDiretorio = 0;
        public frmEnviarDados()
        {
            InitializeComponent();
            ContadorArquivos();
        }

        private void ContadorArquivos()
        {
            // na inicialização da tela, obter total de arquivos .jpg
            if (!Directory.Exists(ModeloUsuario.CaminhoAppArmazenaFotosPRU) && !Directory.Exists(ModeloUsuario.CaminhoAppArmazenaFotosPPU))
            {
                _ = DisplayAlert("ATENÇÃO", "Sem dados para enviar", "OK");
                return;
            }
            else
            {
                if (!Directory.Exists(ModeloUsuario.CaminhoAppArmazenaFotosPRU))
                {
                    Directory.CreateDirectory(ModeloUsuario.CaminhoAppArmazenaFotosPRU);
                }
                if (!Directory.Exists(ModeloUsuario.CaminhoAppArmazenaFotosPPU))
                {
                    Directory.CreateDirectory(ModeloUsuario.CaminhoAppArmazenaFotosPPU);
                }
                var pesquisarPRU = Directory.GetFiles(ModeloUsuario.CaminhoAppArmazenaFotosPRU, "*.*").Where(s => s.EndsWith(".jpg") || s.EndsWith(".txt"));
                totalArquivosNoDiretorio = pesquisarPRU.Count();
                // mostrar o resultado de totald e arquivos
                lblTotalDeArquivosNoDiretorioPRU.Text = "Total de arquivos PRU no diretório: " + Convert.ToString(totalArquivosNoDiretorio);
                if (totalArquivosNoDiretorio == 0)
                {
                    btnAtualizarBancoDeDadosPRU.IsEnabled = false;
                }

                var pesquisarPPU = Directory.GetFiles(ModeloUsuario.CaminhoAppArmazenaFotosPPU, "*.*").Where(s => s.EndsWith(".jpg") || s.EndsWith(".txt"));
                totalArquivosNoDiretorio = pesquisarPPU.Count();
                // mostrar o resultado de totald e arquivos
                lblTotalDeArquivosNoDiretorioPPU.Text = "Total de arquivos PPU no diretório: " + Convert.ToString(totalArquivosNoDiretorio);
                if (totalArquivosNoDiretorio == 0)
                {
                    btnAtualizarBancoDeDadosPPU.IsEnabled = false;
                }

                _ = DisplayAlert("ATENÇÃO", "Não inicie o envio dos dados\n\rSem antes está conectado a uma rede wireless estável.", "OK");
            }
        }

        private async void btnVoltar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
        public static Task<string[]> ReadAllLinesAsync(string path, Encoding uTF8)
        {
            return ReadAllLinesAsync(path, Encoding.UTF8);
        }

        private async void atualizarBancoPPU()
        {
            DAL.DAL InserirElemento = new DAL.DAL();
            await Task.Run(Aguarde);
            framLoading.IsVisible = true;
            lblLoading.IsVisible = true;
            OnPropertyChanged();
            await Task.Run(Aguarde);

            DirectoryInfo dir = new DirectoryInfo(ModeloUsuario.CaminhoAppArmazenaFotosPPU);
            FileInfo[] ArquivosTxtAnalise = dir.GetFiles("*.txt");
            int totalArquivosAnalise = ArquivosTxtAnalise.Length;

            await Task.Run(Aguarde);

            // verificar o tamanho do arquivo (sujeira) para exlcuir os txt problematicos 
            for (int j = 0; j < ArquivosTxtAnalise.Length; j++)
            {
                if (ArquivosTxtAnalise[j].Length == 0)
                {
                    File.Delete(ArquivosTxtAnalise[j].FullName);
                }
            }

            FileInfo[] ArquivosTxt = dir.GetFiles("*.txt");
            int totalArquivos = ArquivosTxt.Length;
            await Task.Run(Aguarde);
            for (int i = 0; i < totalArquivos; i++)
            {
                await Task.Run(Aguarde);
                string[] line = File.ReadAllLines(ArquivosTxt[i].FullName);
                await Task.Run(Aguarde);
                StringBuilder b = new StringBuilder(line[0]);
                string a = b.ToString();
                string[] words = a.Split(new char[] { ';' }, StringSplitOptions.None);
                await Task.Run(Aguarde);
                bool existe = InserirElemento.JaExiste(words[2].ToString());
                if (existe == true)
                {
                    Array.Clear(line, 0, line.Length);
                    Array.Clear(words, 0, words.Length);
                    File.Delete(ArquivosTxt[i].FullName);
                    continue;
                }
                
                if (ArquivosTxt[i].FullName.Contains("PLANEJADO"))
                {
                    InserirElemento.AtualizarRegistroPPU(words[0].ToString(), words[1].ToString(), words[2].ToString(), words[3].ToString(), words[4].ToString(),
                    words[5].ToString(), words[6].ToString(), words[7].ToString(), words[8].ToString(), words[9].ToString(), words[10].ToString(),
                    words[11].ToString(), words[12].ToString(), words[13].ToString(), words[14].ToString(), words[15].ToString(), words[16].ToString(),
                    words[17].ToString(), Convert.ToInt32(words[18]), words[19].ToString(), words[20].ToString(), Convert.ToInt32(words[21]));
                }
                else
                {
                    bool existePPU = InserirElemento.JaExiste(words[2].ToString());
                    if (existePPU == true)
                    {
                        Array.Clear(line, 0, line.Length);
                        Array.Clear(words, 0, words.Length);
                        File.Delete(ArquivosTxt[i].FullName);
                        continue;
                    }

                    InserirElemento.InserirNovoRegistroPPU(words[0].ToString(), words[1].ToString(), words[2].ToString(), words[3].ToString(), words[4].ToString(),
                    words[5].ToString(), words[6].ToString(), words[7].ToString(), words[8].ToString(), words[9].ToString(), words[10].ToString(),
                    words[11].ToString(), words[12].ToString(), words[13].ToString(), words[14].ToString(), words[15].ToString(), words[16].ToString(),
                    words[17].ToString(), Convert.ToInt32(words[18]), words[19].ToString(), words[20].ToString());

                }
                await Task.Run(Aguarde);
                Array.Clear(line, 0, line.Length);
                Array.Clear(words, 0, words.Length);
                //File.Delete(ArquivosTxt[i].FullName);
            }
            await Task.Run(Aguarde);

            await DisplayAlert("e-coleta Rodovias", "Feito! Banco de dados atualizado!", "OK");

            // desligando msg de aguarde
            framLoading.IsVisible = false;
            OnPropertyChanged();
            lblLoading.IsVisible = false;

            backupFotosCelular("PPU");
        }

        private async void atualizarBancoPRU ()
        {
            DAL.DAL InserirElemento = new DAL.DAL();
            await Task.Run(Aguarde);
            framLoading.IsVisible = true;
            lblLoading.IsVisible = true;
            OnPropertyChanged();
            await Task.Run(Aguarde);
  
            DirectoryInfo dir = new DirectoryInfo(ModeloUsuario.CaminhoAppArmazenaFotosPRU);
            FileInfo[] ArquivosTxtAnalise = dir.GetFiles("*.txt");
            int totalArquivosAnalise = ArquivosTxtAnalise.Length;

            await Task.Run(Aguarde);

            // verificar o tamanho do arquivo (sujeira) para exlcuir os txt problematicos 
            for (int j = 0; j < ArquivosTxtAnalise.Length; j++)
            {
                if (ArquivosTxtAnalise[j].Length == 0)
                {
                    File.Delete(ArquivosTxtAnalise[j].FullName);
                }
            }

            FileInfo[] ArquivosTxt = dir.GetFiles("*.txt");
            int totalArquivos = ArquivosTxt.Length;
            await Task.Run(Aguarde);
            for (int i = 0; i < totalArquivos; i++)
            {
                await Task.Run(Aguarde);
                string[] line = File.ReadAllLines(ArquivosTxt[i].FullName);
                await Task.Run(Aguarde);
                StringBuilder b = new StringBuilder(line[0]);
                string a = b.ToString();
                string[] words = a.Split(new char[] { ';' }, StringSplitOptions.None);

                await Task.Run(Aguarde);
                if (ArquivosTxt[i].FullName.Contains("PLANEJADO"))
                {
                    if(words[17].ToString() != "PLANEJADO")
                    {
                        InserirElemento.AtualizarRegistroPRU(words[0].ToString(), words[1].ToString(), words[2].ToString(), words[3].ToString(), words[4].ToString(),
                        words[5].ToString(), words[6].ToString(), words[7].ToString(), words[8].ToString(), words[9].ToString(), words[10].ToString(),
                        words[11].ToString(), words[12].ToString(), words[13].ToString(), words[14].ToString(), words[15].ToString(), words[16].ToString(),
                        words[18].ToString(), words[19].ToString(), Convert.ToInt32(words[20]), words[21], words[22], Convert.ToInt32(words[23]), words[24]);
                    }                    
                }
                else
                {
                    bool existe = InserirElemento.JaExiste(words[2].ToString());
                    if (existe == true)
                    {
                        Array.Clear(line, 0, line.Length);
                        Array.Clear(words, 0, words.Length);
                        File.Delete(ArquivosTxt[i].FullName);
                        continue;
                    }

                    InserirElemento.InserirNovoRegistroPRU(words[0].ToString(), words[1].ToString(), words[2].ToString(), words[3].ToString(), words[4].ToString(),
                        words[5].ToString(), words[6].ToString(), words[7].ToString(), words[8].ToString(), words[9].ToString(), words[10].ToString(),
                        words[11].ToString(), words[12].ToString(), words[13].ToString(), words[14].ToString(), words[15].ToString(), words[16].ToString(),
                        words[18].ToString(), words[19].ToString(), Convert.ToInt32(words[20]), words[21], words[22], words[24]);
                }
                await Task.Run(Aguarde);
                Array.Clear(line, 0, line.Length);
                Array.Clear(words, 0, words.Length);
                //File.Delete(ArquivosTxt[i].FullName);
            }
            await Task.Run(Aguarde);

            await DisplayAlert("e-coleta Rodovias", "Feito! Banco de dados atualizado!", "OK");

            // desligando msg de aguarde
            framLoading.IsVisible = false;
            OnPropertyChanged();
            lblLoading.IsVisible = false;

            backupFotosCelular("PRU");
        }

        private async void backupFotosCelular(string dirProjeto)
        {
            bool habilitarBotaoBackup = await DisplayAlert("e-coleta", "Foi tudo bem no envio dos dados?\n\rSe sim, que tal fazer um backup?", "Sim, só se for agora!", "Não, deixa pra depois.");
            Console.Write("Botão de BACKUP RETORNOU: " + habilitarBotaoBackup);
            if (habilitarBotaoBackup == true)
            {
                framLoading.IsVisible = true;
                lblLoading.IsVisible = true;
                lblLoading.Text = "Enviando arquivos para pasta 'DOWNLOAD' do dispositivo...";
                OnPropertyChanged();
                await Task.Run(Aguarde);
                MoverArquivos(dirProjeto);
                await Task.Run(Aguarde);
                framLoading.IsVisible = false;
                lblLoading.IsVisible = false;
                _ = DisplayAlert("e-coleta", "Opa! Backup realizado.", "OK");
            }
        }

        private void MoverArquivos(string dirProjeto)
        {
            try
            {
                string documentPath = "/storage/emulated/0/Download/" + dirProjeto;

                if (!Directory.Exists(documentPath))
                {
                    Directory.CreateDirectory(documentPath);
                }

                int totalArquivoEnviados = 0;
                if (Directory.Exists(documentPath))
                {
                    DirectoryInfo dir;

                    if(dirProjeto == "PRU")
                    {
                        dir = new DirectoryInfo(ModeloUsuario.CaminhoAppArmazenaFotosPRU);
                    }
                    else
                    {
                        dir = new DirectoryInfo(ModeloUsuario.CaminhoAppArmazenaFotosPPU);
                    }

                    FileInfo[] ArquivosNoDiretorio = dir.GetFiles("*");
                    if (ArquivosNoDiretorio.Length > 0)
                    {
                        for (int arquivos = 0; arquivos < ArquivosNoDiretorio.Length; arquivos++)
                        {
                            File.Move(Path.GetFullPath(ArquivosNoDiretorio[arquivos].FullName), Path.Combine(documentPath + "//" + ArquivosNoDiretorio[arquivos].Name));
                            totalArquivoEnviados++;
                        }
                        _ = DisplayAlert("e-coleta", $"Arquivos enviados {totalArquivoEnviados.ToString()}", "OK");
                    }
                    else
                    {
                        _ = DisplayAlert("e-coleta", "Sem arquivos para backup", "OK");
                    }
                }
                else
                {
                    _ = DisplayAlert("e-coleta", "Não Existe a pasta 'Download' no dispoitivo. Crie o diretório para enviar dados do backup", "OK");
                }
            } catch(Exception ex)
            {
                DisplayAlert("e-coleta", $"Erro ao tentar mover arquivos {ex.Message}", "OK");
            }
           
        }

        public void EnviarArquivoFTP(string NomeArquivo, string _ano, string _mes, string _Sdia, string _LocalArquivo, string _projeto)
        {
            try
            {
                string FtpUrl = "ftp://177.220.159.198";

                /*1 a 9 --> o mês deverá receber o valor 0 antes*/
                if (int.Parse(_mes) < 10)
                {
                    _mes = "0" + _mes;
                }
                if (int.Parse(_Sdia) < 10)
                {
                    _Sdia = "0" + _Sdia;
                }

                string UploadDirectory;
                if (_projeto == "PRU")
                { 
                    UploadDirectory = @"/PRU"; 
                }                
                else
                {
                    UploadDirectory = @"/PPU";
                }

                string _strNomeDoArquivo = new FileInfo(NomeArquivo).Name;

                string caminhoFTPAno = string.Format(@"{0}/{1}/{2}", FtpUrl, UploadDirectory, _ano);

                CriaDiretorioAnoFTP(caminhoFTPAno);
                CriaDiretorioMesFTP(String.Format(@"{0}/{1}", caminhoFTPAno, _mes));
                CriaDiretorioDiaFTP(string.Format(@"{0}/{1}/{2}", caminhoFTPAno, _mes, _Sdia));

                string caminhoCompletoAnoMesDia = String.Format(@"{0}/{1}/{2}/{3}", UploadDirectory, _ano, _mes, _Sdia);

                String uploadUrl = String.Format("{0}/{1}/{2}", FtpUrl, caminhoCompletoAnoMesDia, _strNomeDoArquivo);

                bool JaTemArquivo = VerificarSeExisteArquivoNoDiretorioFtp(uploadUrl, _LocalArquivo);
                if (JaTemArquivo == false)
                {
                    FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(uploadUrl);

                    req.Proxy = null;

                    req.Method = WebRequestMethods.Ftp.UploadFile;
                    req.Credentials = new NetworkCredential("Esteio", "Reynaldo1151!@#");
                    req.UseBinary = true;

                    byte[] data = File.ReadAllBytes(NomeArquivo);
                    req.ContentLength = data.Length;
                    Stream stream = req.GetRequestStream();
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                    FtpWebResponse res = (FtpWebResponse)req.GetResponse();
                    Console.WriteLine("Upload do arquivo Completo, status {0}", res.StatusDescription);
                }
                else
                {
                    return;
                }
            }
            catch (WebException ex)
            {
                DisplayAlert("Erro ao enviar dados para FTP.Verifique se a conexão está estável", ex.Message, "OK");
                return;
            }
        }

        int _ano = 0;
        int _mes = 0;
        int _dia = 0;

        private async void Aguarde()
        {
            await Task.Delay(1000);
        }

        private async void LiberarBotaoAtualizarBancoDados(string dirProj)
        {
            OnPropertyChanged();

            // desligando msg de aguarde
            framLoading.IsVisible = false;

            lblLoading.IsVisible = false;
            bool HabilitarBotaoEnviarDados = await DisplayAlert("e-coleta Rodovias", "Término do envio das fotos " + dirProj + ". Quer atualizar o banco de dados?", "Sim, vamos lá!", "Não. Deixa pra depois");
            if (HabilitarBotaoEnviarDados == true)
            {
                if(dirProj == "PRU")
                {
                    btnAtualizarBancoDeDadosPRU.IsEnabled = true;
                }
                if(dirProj == "PPU")
                {
                    btnAtualizarBancoDeDadosPPU.IsEnabled = true;
                }
            }
            else
            {
                if (dirProj == "PRU")
                {
                    btnAtualizarBancoDeDadosPRU.IsEnabled = false;
                }
                if (dirProj == "PPU")
                {
                    btnAtualizarBancoDeDadosPPU.IsEnabled = false;
                }
            }
        }

        private async void btnEnviarArquivosFTP_Clicked(object sender, EventArgs e)
        {
            await Task.Run(Aguarde);
            if (Directory.Exists(ModeloUsuario.CaminhoAppArmazenaFotosPRU))
            {
                enviarArquivosFtpPRU();
            }
            await Task.Run(Aguarde);
            await Task.Run(Aguarde);
            if (Directory.Exists(ModeloUsuario.CaminhoAppArmazenaFotosPPU))
            {
                enviarArquivosFtpPPU();
            }
            await Task.Run(Aguarde);
            LiberarBotaoAtualizarBancoDados("PRU");
        }

        private async void enviarArquivosFtpPRU()
        {
            framLoading.IsVisible = true;
            lblLoading.IsVisible = true;

            DirectoryInfo dir = new DirectoryInfo(ModeloUsuario.CaminhoAppArmazenaFotosPRU);

            FileInfo[] NomeFotos = dir.GetFiles("*");
            int totalArquivos = NomeFotos.Length;

            OnPropertyChanged();

            int contadorDeArquivos = 0;

            if (totalArquivos == 0) { _ = DisplayAlert("e-coleta", "Sem arquivos de PRU para enviar", "OK"); return; }
            else
            {
                foreach (FileInfo file in NomeFotos)
                {
                    await Task.Run(Aguarde);
                    DateTime dataArquivoCriado = File.GetCreationTime(Path.Combine(ModeloUsuario.CaminhoAppArmazenaFotosPRU, file.Name));
                    _ano = dataArquivoCriado.Year;
                    _mes = dataArquivoCriado.Month;
                    _dia = dataArquivoCriado.Day;

                    await Task.Run(Aguarde);
                    string ArquivoFoto = Path.Combine(ModeloUsuario.CaminhoAppArmazenaFotosPRU, file.Name);
                    string ArquivoNoMedia = Path.Combine(ModeloUsuario.CaminhoAppArmazenaFotosPRU, ".nomedia");
                    if (ArquivoFoto == ArquivoNoMedia)
                    {
                        File.Delete(ArquivoFoto);
                    }
                    else
                    {
                        EnviarArquivoFTP(ArquivoFoto, _ano.ToString(), _mes.ToString(), _dia.ToString(), ArquivoFoto, "PRU");
                        contadorDeArquivos++;
                    }
                    await Task.Run(Aguarde);

                    lblTotalDeArquivosEnviadosPRU.Text = "Total de Arquivos Enviados: " + contadorDeArquivos.ToString();
                    OnPropertyChanged();
                }
                LiberarBotaoAtualizarBancoDados("PRU");
            }
        }

        private async void enviarArquivosFtpPPU()
        {
            DirectoryInfo dir = new DirectoryInfo(ModeloUsuario.CaminhoAppArmazenaFotosPPU);

            FileInfo[] NomeFotos = dir.GetFiles("*");
            int totalArquivos = NomeFotos.Length;

            OnPropertyChanged();

            int contadorDeArquivos = 0;

            if (totalArquivos == 0) { _ = DisplayAlert("e-coleta", "Sem arquivos PPU +para enviar", "OK"); return; }
            else
            {
                foreach (FileInfo file in NomeFotos)
                {
                    await Task.Run(Aguarde);
                    DateTime dataArquivoCriado = File.GetCreationTime(Path.Combine(ModeloUsuario.CaminhoAppArmazenaFotosPPU, file.Name));
                    _ano = dataArquivoCriado.Year;
                    _mes = dataArquivoCriado.Month;
                    _dia = dataArquivoCriado.Day;

                    await Task.Run(Aguarde);
                    string ArquivoFoto = Path.Combine(ModeloUsuario.CaminhoAppArmazenaFotosPPU, file.Name);
                    string ArquivoNoMedia = Path.Combine(ModeloUsuario.CaminhoAppArmazenaFotosPRU, ".nomedia");
                    if (ArquivoFoto == ArquivoNoMedia)
                    {
                        File.Delete(ArquivoFoto);
                    }
                    else
                    {
                        EnviarArquivoFTP(ArquivoFoto, _ano.ToString(), _mes.ToString(), _dia.ToString(), ArquivoFoto, "PPU");
                        contadorDeArquivos++;
                    }

                    await Task.Run(Aguarde);

                    lblTotalDeArquivosEnviadosPPU.Text = "Total de Arquivos Enviados: " + contadorDeArquivos.ToString();
                    OnPropertyChanged();
                }
                LiberarBotaoAtualizarBancoDados("PPU");
            }
        }

        private bool CriaDiretorioAnoFTP(string _dirAno)
        {
            try
            {
                //Criar Diretorio ANO
                FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri(_dirAno));
                requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
                requestDir.Credentials = new NetworkCredential("Esteio", "Reynaldo1151!@#");
                requestDir.UsePassive = true;
                requestDir.UseBinary = true;
                requestDir.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();

                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return true;
                }
                else
                {
                    response.Close();
                    return false;
                }
            }
        }

        private bool CriaDiretorioMesFTP(string _dirAnoEMes)
        {
            try
            {
                //Criar Diretorio Mes
                FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri(_dirAnoEMes));
                requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
                requestDir.Credentials = new NetworkCredential("Esteio", "Reynaldo1151!@#");
                requestDir.UsePassive = true;
                requestDir.UseBinary = true;
                requestDir.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();

                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return true;
                }
                else
                {
                    response.Close();
                    return false;
                }
            }
        }

        private bool CriaDiretorioDiaFTP(string _dirAnoMesDia)
        {
            try
            {
                //Criar Diretorio Dia
                FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri(_dirAnoMesDia));
                requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
                requestDir.Credentials = new NetworkCredential("Esteio", "Reynaldo1151!@#");
                requestDir.UsePassive = true;
                requestDir.UseBinary = true;
                requestDir.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();

                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return true;
                }
                else
                {
                    response.Close();
                    return false;
                }

            }
        }

        private void RenomearArquivosNoDiretorioFtp(string FtpUrlCompletoComArquivo, string NovoNomeParaOArquivoFtp)
        {
            Stream ftpStream = null;
            try
            {
                var requestParaMudar = (FtpWebRequest)WebRequest.Create(FtpUrlCompletoComArquivo);
                requestParaMudar.Credentials = new NetworkCredential("Esteio", "Reynaldo1151!@#");
                requestParaMudar.Method = WebRequestMethods.Ftp.Rename;
                requestParaMudar.RenameTo = NovoNomeParaOArquivoFtp;
                requestParaMudar.UseBinary = true;
                FtpWebResponse RespostaDoWebRequest = (FtpWebResponse)requestParaMudar.GetResponse();
                ftpStream = RespostaDoWebRequest.GetResponseStream();
                ftpStream.Close();
                RespostaDoWebRequest.Close();
            }
            catch (Exception ex)
            {
                if (ftpStream != null)
                {
                    ftpStream.Close();
                    ftpStream.Dispose();
                }
                throw new Exception(ex.Message.ToString());
            }
        }

        private bool VerificarSeExisteArquivoNoDiretorioFtp(string _caminhoCompleto, string _arquivoLocalParaAnalise)
        {
            var request = (FtpWebRequest)WebRequest.Create(_caminhoCompleto);
            request.Credentials = new NetworkCredential("Esteio", "Reynaldo1151!@#");
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            bool TemOArquivo = false;
            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                if (string.IsNullOrEmpty(response.LastModified.Year.ToString()))
                {
                    TemOArquivo = false;
                }
                else
                {
                    byte[] ArquivoFoto = System.IO.File.ReadAllBytes(_arquivoLocalParaAnalise);
                    string Tamanho = response.ContentLength.ToString();

                    if (response.ContentLength != ArquivoFoto.Length)
                    {
                        RenomearArquivosNoDiretorioFtp(_caminhoCompleto, "err_" + Path.GetFileName(_arquivoLocalParaAnalise));
                        FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(_caminhoCompleto);
                        req.Proxy = null;
                        req.Method = WebRequestMethods.Ftp.UploadFile;
                        req.Credentials = new NetworkCredential("Esteio", "Reynaldo1151!@#");
                        req.UseBinary = true;
                        byte[] data = File.ReadAllBytes(_arquivoLocalParaAnalise);
                        req.ContentLength = data.Length;
                        Stream stream = req.GetRequestStream();
                        stream.Write(data, 0, data.Length);
                        stream.Close();
                        FtpWebResponse res = (FtpWebResponse)req.GetResponse();
                    }
                    TemOArquivo = true;
                }
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    TemOArquivo = false;
                }
                else
                {
                    TemOArquivo = false;
                }
            }
            return TemOArquivo;
        }

        private void btnFazerBackup_Clicked(object sender, EventArgs e)
        {
            MoverArquivos("PRU");
        }

        private void btnEnviarFotosPPU(object sender, EventArgs e)
        {
            if (Directory.Exists(ModeloUsuario.CaminhoAppArmazenaFotosPPU))
            {
                enviarArquivosFtpPPU();
                
            }
        }

        private void btnEnviarFotosPRU(object sender, EventArgs e)
        {
            if (Directory.Exists(ModeloUsuario.CaminhoAppArmazenaFotosPRU))
            {
                enviarArquivosFtpPRU();  
            }
        }

        private void BtnAtualizarBancoDeDadosPRU(object sender, EventArgs e)
        {
            if (Directory.Exists(ModeloUsuario.CaminhoAppArmazenaFotosPRU))
            {
                atualizarBancoPRU();
            }
        }

        private void BtnAtualizarBancoDeDadosPPU(object sender, EventArgs e)
        {
            if (Directory.Exists(ModeloUsuario.CaminhoAppArmazenaFotosPPU))
            {
                atualizarBancoPPU();
            }
        }

        private async void BtnImportarPlanejamentoPRU(object sender, EventArgs e)
        {

            await Task.Run(Aguarde);
            framLoading.IsVisible = true;
            lblLoading.IsVisible = true;
            OnPropertyChanged();
            await Task.Run(Aguarde);

            DAL.DAL ImpotarElemento = new DAL.DAL();
            ImpotarElemento.ImportarRegistrosPlanejadosPRU();

            // desligando msg de aguarde
            framLoading.IsVisible = false;
            OnPropertyChanged();
            lblLoading.IsVisible = false;

            ContadorArquivos();
        }

        private async void BtnImportarPlanejamentoPPU(object sender, EventArgs e)
        {
            await Task.Run(Aguarde);
            framLoading.IsVisible = true;
            lblLoading.IsVisible = true;
            OnPropertyChanged();
            await Task.Run(Aguarde);

            DAL.DAL ImpotarElemento = new DAL.DAL();
            ImpotarElemento.ImportarRegistrosPlanejadosPPU();

            // desligando msg de aguarde
            framLoading.IsVisible = false;
            OnPropertyChanged();
            lblLoading.IsVisible = false;

            ContadorArquivos();
        }
    }
}
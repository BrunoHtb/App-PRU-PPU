using PCLExt.FileStorage.Folders;
using Plugin.Media;
using Plugin.Media.Abstractions;
using rodovias.DAL;
using rodovias.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rodovias.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaCadastroEditarPRU : ContentPage
    {

        ClsPopular PreencherDadosCombox = new ClsPopular();
        ViewModels.UserModel Elemento = new ViewModels.UserModel();
        string caminhoSalvarFotos = string.Empty;

        string codigoAntigoWhere = string.Empty;
        
        string NovaFoto1, NovaFoto2, NovaFoto3 = string.Empty;

        string codigoElementoAntigoComTxt = string.Empty;
        string codigoElementoAtualParaValidacao = string.Empty;

        private string _id = ClsEditarElemento.ID;
        private string _latitudeInicio = ClsEditarElemento.LatitudeInicio;
        private string _longitudeInicio = ClsEditarElemento.LongitudeInicio;
        private string _latitudeFim =  ClsEditarElemento.LatitudeFim;
        private string _longitudeFim = ClsEditarElemento.LongitudeFim;
        private string _rodovia = ClsEditarElemento.Rodovia;
        private string _sentido = ClsEditarElemento.Sentido;
        private string _ladoDaPista = ClsEditarElemento.Lado;
        private string _estadoconservacao = ClsEditarElemento.Condicao;
        private string _kmInicio = ClsEditarElemento.KmInicio;
        private string _kmFim = ClsEditarElemento.KmFim;
        private string _observacao = ClsEditarElemento.ObservacaoCampo;
        private string _der = ClsEditarElemento.Regional_der;
        private string _nomeDoElemento = ClsEditarElemento.CodigoElemento;
        private string _status = ClsEditarElemento.status_interno;
        string _nomeDaFotoPanoramica = ClsEditarElemento.nome_fotopanoramica;
        string _nomeDaFotoDetalhe1 = ClsEditarElemento.nome_fotodetalhe_1;
        string _nomeDaFotoDetalhe2 = ClsEditarElemento.nome_fotodetalhe_2;
        private string _sinalizacaoVertical = ClsEditarElemento.SinalizaçãoVertical;
        private string _atendimentoNorma = ClsEditarElemento.AtendimentoNorma;
        private string _auditoria = ClsEditarElemento.Auditoria;
        private string _tipoFaixa = ClsEditarElemento.TipoFaixa;

        string _nomeDoArquivoAntigo;
        string _selectStatusInterno;

        private bool _existFotoPanoramica = false;
        private bool _existFotoDetalhe1 = false;
        private bool _existFotoDetalhe2 = false;


        public PaginaCadastroEditarPRU()
        {
            InitializeComponent();

            if (!Directory.Exists("/storage/emulated/0/Android/data/com.esteio.rodoviasPRUePPU/files/Pictures/PRU"))
            {
                Directory.CreateDirectory("/storage/emulated/0/Android/data/com.esteio.rodoviasPRUePPU/files/Pictures/PRU");
            }
            caminhoSalvarFotos = "/storage/emulated/0/Android/data/com.esteio.rodoviasPRUePPU/files/Pictures/PRU";


            if (!string.IsNullOrEmpty(_nomeDaFotoPanoramica))
            {
                FotoPanoramica.Text = "Foto Panorâmica ✔";
                FotoPanoramica.BackgroundColor = Color.Green;
                _existFotoPanoramica = true;
            }
            if (!string.IsNullOrEmpty(_nomeDaFotoDetalhe1))
            {
                FotoDetalhe1.Text = "Foto Detalhe 1 ✔";
                FotoDetalhe1.BackgroundColor = Color.Green;
                _existFotoDetalhe1 = true;
            }
            if (!string.IsNullOrEmpty(_nomeDaFotoDetalhe2))
            {
                FotoDetalhe2.Text = "Foto Detalhe 2 ✔";
                FotoDetalhe2.BackgroundColor = Color.Green;
                _existFotoDetalhe2 = true;
            }

            var indexDR = ClsEditarElemento.Regional_der;
            DER.SelectedItem = indexDR;

            Rodovia.Text = ClsEditarElemento.Rodovia;

            var indexLado = ClsEditarElemento.Lado;
            LadoDaPista.SelectedItem = indexLado;

            var indexSentido = ClsEditarElemento.Sentido;
            Sentido.SelectedItem = indexSentido;

            var indexCondicao = ClsEditarElemento.Condicao;
            EstadoConservacao.SelectedItem = indexCondicao;

            var indexSinalizacao = ClsEditarElemento.SinalizaçãoVertical;
            SinalizacaoVertical.SelectedItem = indexSinalizacao;

            var indexNorma = ClsEditarElemento.AtendimentoNorma;
            AtendimentoNorma.SelectedItem = indexNorma;

            var indexFaixa = ClsEditarElemento.TipoFaixa;
            TipoFaixa.SelectedItem = indexFaixa;

            LatitudeInicial.Text = ClsEditarElemento.LatitudeInicio;
            LongitudeInicial.Text = ClsEditarElemento.LongitudeInicio;

            LatitudeFinal.Text = ClsEditarElemento.LatitudeFim;
            LongitudeFinal.Text = ClsEditarElemento.LongitudeFim;

            Observacao.Text = ClsEditarElemento.ObservacaoCampo.Replace(';', '.');

            EntryCodigoElemento.Text = ClsEditarElemento.CodigoElemento;

            //codigoAntigoWhere = txtCodigoElemento.Text;
            KmInicio.Text = ClsEditarElemento.KmInicio;
            KmFinal.Text = ClsEditarElemento.KmFim;

            if(ClsEditarElemento.status_interno == "PLANEJADO")
            {
                _nomeDoArquivoAntigo = "DR" + indexDR + "_" + ClsEditarElemento.Rodovia + "_" + "PRU" + "_" + ClsEditarElemento.KmInicio +
                                       "_" + ClsEditarElemento.KmFim + "_" + indexSentido + "_" + indexLado + "_PLANEJADO.txt";
                _selectStatusInterno = "PLANEJADO";
            }
            else
            {
                _nomeDoArquivoAntigo = "DR" + indexDR + "_" + ClsEditarElemento.Rodovia + "_" + "PRU" + "_" + ClsEditarElemento.KmInicio +
                                       "_" + ClsEditarElemento.KmFim + "_" + indexSentido + "_" + indexLado + ".txt";
                _selectStatusInterno = "NOVO";
            }
        }

        private async void GetCoordenadasInicio(object sender, EventArgs e)
        {
            var location = await Geolocation.GetLocationAsync();

            _latitudeInicio = location.Latitude.ToString();
            _longitudeInicio = location.Longitude.ToString();

            LatitudeInicial.Text = _latitudeInicio;
            LongitudeInicial.Text = _longitudeInicio;
        }

        private async void GetCoordenadasFinal(object sender, EventArgs e)
        {
            var location = await Geolocation.GetLocationAsync();

            _latitudeFim = location.Latitude.ToString();
            _longitudeFim = location.Longitude.ToString();

            LatitudeFinal.Text = _latitudeFim;
            LongitudeFinal.Text = _longitudeFim;
        }

        private void EntryRodovia(object sender, EventArgs e)
        {
            try
            {
                _rodovia = Rodovia.Text.ToString();
            }
            catch (Exception) { }
        }

        private void PckDER(object sender, EventArgs e)
        {
            try
            {
                _der = DER.Items[DER.SelectedIndex];
            }
            catch (Exception) { }
        }

        private void PckSentido(object sender, EventArgs e)
        {
            try
            {
                _sentido = Sentido.Items[Sentido.SelectedIndex];
            }
            catch (Exception) { }
        }

        private void PckLadoDaPista(object sender, EventArgs e)
        {
            try
            {
                _ladoDaPista = LadoDaPista.Items[LadoDaPista.SelectedIndex];
            }
            catch (Exception) { }
        }

        private void PckEstadoDeConservacao(object sender, EventArgs e)
        {
            try
            {
                _estadoconservacao = EstadoConservacao.Items[EstadoConservacao.SelectedIndex];
            }
            catch (Exception) { }
        }

        private async void EntryKmInicio(object sender, EventArgs e)
        {
            try
            {
                _kmInicio = KmInicio.Text.ToString();
            }
            catch (Exception) { }
        }

        private async void EntryKmFinal(object sender, EventArgs e)
        {
            try
            {
                _kmFim = KmFinal.Text.ToString();
            }
            catch (Exception) { }
        }
        private void EntryObservacao(object sender, EventArgs e)
        {
            try
            {
                _observacao = Observacao.Text.ToString();
            }
            catch (Exception) { }
        }

        private void VerificarCampoNaoPreenchido()
        {
            if (_rodovia == "")
            {
                Rodovia.Focus();
            }
            else if (_der == "")
            {
                DER.Focus();
            }
            else if (_ladoDaPista == "")
            {
                LadoDaPista.Focus();
            }
            else if (_sentido == "")
            {
                Sentido.Focus();
            }
        }

        private async void BtnFotoPanoramica(object sender, EventArgs e)
        {
            LerCampos(sender, e);
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                await DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");
                return;
            }

            if (_der == "" || string.IsNullOrEmpty(Rodovia.Text) || _ladoDaPista == "" || _sentido == "")
            {
                await DisplayAlert("Alerta de campo sem preenchimento", "Campo obrigatório não preenchido", "OK");
                VerificarCampoNaoPreenchido();
                return;
            }

            _nomeDaFotoPanoramica = "PRU_DR" + DER.Items[DER.SelectedIndex] + "_" + Rodovia.Text.ToUpper().Replace(" ", "") + "_KMI_" + KmInicio.Text + "_" +
                                    Sentido.Items[Sentido.SelectedIndex].Substring(0, 3) + "_" + LadoDaPista.Items[LadoDaPista.SelectedIndex].Substring(0, 1) +
                                    "_" + DateTime.Now.ToString("dd-MM-yyyy_HHmmss") + "_01.jpg";

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Name = _nomeDaFotoPanoramica,
                    Directory = "PRU",
                    CompressionQuality = 60
                });

            if (file != null)
            {
                FotoPanoramica.Text = "Foto Panorâmica ✔";
                FotoPanoramica.BackgroundColor = Color.Green;
                return;
            }
        }

        private async void BtnFotoDetalhe1(object sender, EventArgs e)
        {
            LerCampos(sender, e);
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                await DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");
                return;
            }

            if (_der == "" || string.IsNullOrEmpty(Rodovia.Text) || _ladoDaPista == "" || _sentido == "")
            {
                await DisplayAlert("Alerta de campo sem preenchimento", "Campo obrigatório não preenchido", "OK");
                VerificarCampoNaoPreenchido();
                return;
            }

            _nomeDaFotoDetalhe1 = "PRU_DR" + DER.Items[DER.SelectedIndex] + "_" + Rodovia.Text.ToUpper().Replace(" ", "") + "_KMI_" + KmInicio.Text + "_" +
                                    Sentido.Items[Sentido.SelectedIndex].Substring(0, 3) + "_" + LadoDaPista.Items[LadoDaPista.SelectedIndex].Substring(0, 1) +
                                    "_" + DateTime.Now.ToString("dd-MM-yyyy_HHmmss") + "_02.jpg";
            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Name = _nomeDaFotoDetalhe1,
                    Directory = "PRU",
                    CompressionQuality = 60
                });

            if (file != null)
            {
                FotoDetalhe1.Text = "Foto Detalhe 1 ✔";
                FotoDetalhe1.BackgroundColor = Color.Green;
                return;
            }

        }

        private async void BtnFotoDetalhe2(object sender, EventArgs e)
        {
            LerCampos(sender, e);
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                await DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");
                return;
            }

            if (_der == "" || string.IsNullOrEmpty(Rodovia.Text) || _ladoDaPista == "" || _sentido == "")
            {
                await DisplayAlert("Alerta de campo sem preenchimento", "Campo obrigatório não preenchido", "OK");
                VerificarCampoNaoPreenchido();
                return;
            }

            _nomeDaFotoDetalhe2 = "PRU_DR" + DER.Items[DER.SelectedIndex] + "_" + Rodovia.Text.ToUpper().Replace(" ", "") + "_KMI_" + KmInicio.Text + "_" +
                                    Sentido.Items[Sentido.SelectedIndex].Substring(0, 3) + "_" + LadoDaPista.Items[LadoDaPista.SelectedIndex].Substring(0, 1) +
                                    "_" + DateTime.Now.ToString("dd-MM-yyyy_HHmmss") + "_03.jpg";

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Name = _nomeDaFotoDetalhe2,
                    Directory = "PRU",
                    CompressionQuality = 60
                });

            if (file != null)
            {
                FotoDetalhe2.Text = "Foto Detalhe 2 ✔";
                FotoDetalhe2.BackgroundColor = Color.Green;
                return;
            }
        }

        private async void BtnObterNomenclaturaDoElemento(object sender, EventArgs e)
        {
            if (Sentido.SelectedIndex == -1 || LadoDaPista.SelectedIndex == -1)
            {
                await DisplayAlert("e-coleta", "Por favor, confirme a seleção do LADO e SENTIDO", "OK");
                return;
            }
            _nomeDoElemento = "DR" + _der + "_" + _rodovia + "_PRU_" + "KM_" + KmInicio.Text + "_" + KmFinal.Text + "_PSL" + LadoDaPista.Items[LadoDaPista.SelectedIndex].Substring(0, 1);
            EntryCodigoElemento.Text = _nomeDoElemento.ToUpper();
        }

        private void LerCampos(object sender, EventArgs e)
        {
            EntryRodovia(sender, e);
            EntryKmInicio(sender, e);
            EntryKmFinal(sender, e);
            PckDER(sender, e);
            EntryObservacao(sender, e);     
            EntryLatitudeInicial(sender, e);
            EntryLongitudeInicial(sender, e);
            EntryLatitudeFinal(sender, e);
            EntryLongitudeFinal(sender, e);
        }    
        private void MudarNomeFoto(string path)
        {
            if(_existFotoPanoramica)
            {
                string nomeFotoPanoramicaNova = "PRU_DR" + DER.Items[DER.SelectedIndex] + "_" + Rodovia.Text.ToUpper().Replace(" ", "") + "_KMI_" + KmInicio.Text + "_" +
                                               Sentido.Items[Sentido.SelectedIndex].Substring(0, 3) + "_" + LadoDaPista.Items[LadoDaPista.SelectedIndex].Substring(0, 1) +
                                               "_" + DateTime.Now.ToString("dd-MM-yyyy_HHmmss") + "_01.jpg";
                System.IO.File.Move(System.IO.Path.Combine(path, _nomeDaFotoPanoramica), System.IO.Path.Combine(path, nomeFotoPanoramicaNova));
                _nomeDaFotoPanoramica = nomeFotoPanoramicaNova;
            }
            if(_existFotoDetalhe1)
            {
                string nomeFotoDetalhe1Nova = "PRU_DR" + DER.Items[DER.SelectedIndex] + "_" + Rodovia.Text.ToUpper().Replace(" ", "") + "_KMI_" + KmInicio.Text + "_" +
                                          Sentido.Items[Sentido.SelectedIndex].Substring(0, 3) + "_" + LadoDaPista.Items[LadoDaPista.SelectedIndex].Substring(0, 1) +
                                          "_" + DateTime.Now.ToString("dd-MM-yyyy_HHmmss") + "_02.jpg";
                System.IO.File.Move(System.IO.Path.Combine(path, _nomeDaFotoDetalhe1), System.IO.Path.Combine(path, nomeFotoDetalhe1Nova));
                _nomeDaFotoDetalhe1 = nomeFotoDetalhe1Nova;
            }
            if(_existFotoDetalhe2)
            {
                string nomeFotoDetalhe2Nova = "PRU_DR" + DER.Items[DER.SelectedIndex] + "_" + Rodovia.Text.ToUpper().Replace(" ", "") + "_KMI_" + KmInicio.Text + "_" +
                                         Sentido.Items[Sentido.SelectedIndex].Substring(0, 3) + "_" + LadoDaPista.Items[LadoDaPista.SelectedIndex].Substring(0, 1) +
                                         "_" + DateTime.Now.ToString("dd-MM-yyyy_HHmmss") + "_03.jpg";
                System.IO.File.Move(System.IO.Path.Combine(path, _nomeDaFotoDetalhe2), System.IO.Path.Combine(path, nomeFotoDetalhe2Nova));
                _nomeDaFotoDetalhe2 = nomeFotoDetalhe2Nova;
            }   
        }

        private async void BtnSalvarDados(object sender, EventArgs e)
        {
            LerCampos(sender, e);
            BtnObterNomenclaturaDoElemento(sender, e);

            if (_der == "" || string.IsNullOrEmpty(Rodovia.Text))
            {
                await DisplayAlert("Alerta de campo sem preenchimento", "Campo obrigatório não preenchido", "OK");
                VerificarCampoNaoPreenchido();
                return;
            }

            string _date = DateTime.Now.ToString("dd/MM/yyyy");
            var root = new LocalRootFolder();
            string path = root.Path + "/files/Pictures/PRU";
            string fileName;

            if (_selectStatusInterno == "PLANEJADO")
            {
                fileName = "DR" + _der + "_" + _rodovia + "_" + "PRU" + "_" + KmInicio.Text + "_" + KmFinal.Text + "_" + _sentido + "_" + _ladoDaPista + "_PLANEJADO.txt";
            }
            else
            {
                fileName = "DR" + _der + "_" + _rodovia + "_" + "PRU" + "_" + KmInicio.Text + "_" + KmFinal.Text + "_" + _sentido + "_" + _ladoDaPista + ".txt";
            }

            MudarNomeFoto(path);
            _status = "EDITADO";
            string lines =
                "DR" + _der + ";" +
                _rodovia.ToUpper() + ";" +
                _nomeDoElemento + ";" +
                _ladoDaPista + ";" +
                _sentido + ";" +
                _date + ";" +
                _latitudeInicio + ";" +
                _longitudeInicio + ";" +
                _latitudeFim + ";" +
                _longitudeFim + ";" +
                _nomeDaFotoPanoramica + ";" +
                _nomeDaFotoDetalhe1 + ";" +
                _nomeDaFotoDetalhe2 + ";" +
                _estadoconservacao + ";" +
                _observacao + ";" +
                _kmInicio + ";" +
                _kmFim + ";" +
                _status + ";" +
                _sinalizacaoVertical + ";" +
                _atendimentoNorma + ";" +
                _auditoria + ";" +
                Preferences.Get("NomeUsuario", string.Empty).ToUpper() + ";" +
                Preferences.Get("IdDispositivo", string.Empty) + ";" +
                _id + ";" +
                _tipoFaixa;
                

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string pathFileNameOld = System.IO.Path.Combine(path, _nomeDoArquivoAntigo);
            string pathFileNameNew = System.IO.Path.Combine(path, fileName);

            if (File.Exists(pathFileNameOld))
            {
                File.Delete(pathFileNameOld);
            }

            using (var streamWriter = new System.IO.StreamWriter(pathFileNameNew, false))
            {
                streamWriter.WriteLine(lines);
            }

            await DisplayAlert("Dados Salvos", "As informações foram salvas com sucesso", "OK");
            RemoverPageRelacaoElementos();
            await App.Current.MainPage.Navigation.PushAsync(new Pages.PaginaPRURelacao());
        }

        private void BtnLimparDados(object sender, EventArgs e)
        {
            lbLatitudeInicio.Text = "Latitude: ";
            lbLongitudeInicio.Text = "Longitude: ";
            Rodovia.Text = string.Empty;
            DER.SelectedIndex = -1;
            KmInicio.Text = string.Empty;
            KmFinal.Text = string.Empty;
            Sentido.SelectedIndex = -1;
            LadoDaPista.SelectedIndex = -1;
            EstadoConservacao.SelectedIndex = -1;
            lbLatitudeFim.Text = "Latitude: ";
            lbLongitudeFim.Text = "Longitude: ";
            Observacao.Text = string.Empty;
        }

        private void PckSinalizacaoVertical(object sender, EventArgs e)
        {
            try
            {
                _sinalizacaoVertical = SinalizacaoVertical.Items[SinalizacaoVertical.SelectedIndex];
            }
            catch (Exception) { }
        }

        private void PckAtendimentoNorma(object sender, EventArgs e)
        {
            try
            {
                _atendimentoNorma = AtendimentoNorma.Items[AtendimentoNorma.SelectedIndex];
            }
            catch (Exception) { }
        }
        private void PckTipoFaixa(object sender, EventArgs e)
        {
            try
            {
                _tipoFaixa = TipoFaixa.Items[TipoFaixa.SelectedIndex];
            }
            catch (Exception) { }
        }

        private void EntryLatitudeInicial(object sender, EventArgs e)
        {
            try
            {
                _latitudeInicio = LatitudeInicial.Text.ToString();
            }
            catch (Exception) { }
        }

        private void EntryLongitudeInicial(object sender, EventArgs e)
        {
            try
            {
                _longitudeInicio = LongitudeInicial.Text.ToString();
            }
            catch (Exception) { }
        }

        private void EntryLatitudeFinal(object sender, EventArgs e)
        {
            try
            {
                _latitudeFim = LatitudeFinal.Text.ToString();
            }
            catch (Exception) { }
        }

        private void EntryLongitudeFinal(object sender, EventArgs e)
        {
            try
            {
                _longitudeFim = LongitudeFinal.Text.ToString();
            }
            catch (Exception) { }
        }

        private async void BtnCancelarCadastro(object sender, EventArgs e)
        {
            bool cancelar = await DisplayAlert("Atenção", "Tem certeza que deseja cancelar esse cadastro?", "SIM", "NÃO");

            if (cancelar == true)
            {
                RemoverPageRelacaoElementos();
                await App.Current.MainPage.Navigation.PushAsync(new Pages.PaginaPRURelacao());
            }
            else
            {
                return;
            }
        }

        private void RemoverPageRelacaoElementos()
        {
            var removePage = this.Navigation.NavigationStack;
            for (int i = 0; i < removePage.Count; i++)
            {
                var name = removePage[i].Title;
                if (name == "Cadastro de PRU")
                {
                    Navigation.RemovePage(this.Navigation.NavigationStack[i]);
                }
            }
        }

    }
}
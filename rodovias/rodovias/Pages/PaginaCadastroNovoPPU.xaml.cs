using PCLExt.FileStorage.Folders;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
    public partial class PaginaCadastroNovoPPU : ContentPage
    {
        private string _latitudeInicio = "";
        private string _longitudeInicio = "";
        private string _latitudeFim = "";
        private string _longitudeFim = "";
        private string _rodovia = "";
        private string _sentido = "";
        private string _ladoDaPista = "";
        private string _estadoconservacao = "";
        private string _kmInicio = "";
        private string _kmFim = "";
        private string _observacao = "";
        private string _der = "";
        private string _tipoDaPista = "";
        private string _nomeDaFotoPanoramica = "";
        private string _nomeDaFotoDetalhe1 = "";
        private string _nomeDaFotoDetalhe2 = "";
        private string _nomeDoElemento = "";
        private string _status = "NOVO";

        public PaginaCadastroNovoPPU()
        {
            InitializeComponent();
        }

        private async void GetCoordenadasInicio(object sender, EventArgs e)
        {
            var location = await Geolocation.GetLocationAsync();

            _latitudeInicio = location.Latitude.ToString();
            _longitudeInicio = location.Longitude.ToString();

            lbLatitudeInicio.Text = "Latitude:  " + _latitudeInicio;
            lbLongitudeInicio.Text = "Longitude:  " + _longitudeInicio;
        }

        private async void GetCoordenadasFinal(object sender, EventArgs e)
        {
            var location = await Geolocation.GetLocationAsync();

            _latitudeFim = location.Latitude.ToString();
            _longitudeFim = location.Longitude.ToString();

            lbLatitudeFim.Text = "Latitude:  " + _latitudeFim;
            lbLongitudeFim.Text = "Longitude:  " + _longitudeFim;
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
                if (KmInicio.Text.Length < 7)
                {
                    await DisplayAlert("KM Início", "Campo não preenchido totalmente", "OK");
                    KmInicio.Focus();
                }
                _kmInicio = KmInicio.Text.ToString();
            }
            catch (Exception) { }
        }

        private async void EntryKmFinal(object sender, EventArgs e)
        {
            try
            {
                if (KmFinal.Text.Length < 7)
                {
                    await DisplayAlert("KM Fim", "Campo não preenchido totalmente", "OK");
                    return;
                }
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
            else if (_kmInicio == "")
            {
                KmInicio.Focus();
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

            if (_der == "" || string.IsNullOrEmpty(Rodovia.Text) || string.IsNullOrEmpty(KmInicio.Text) || _ladoDaPista == "" || _sentido == "")
            {
                await DisplayAlert("Alerta de campo sem preenchimento", "Campo obrigatório não preenchido", "OK");
                VerificarCampoNaoPreenchido();
                return;
            }

            _nomeDaFotoPanoramica = "PPU_DR" + DER.Items[DER.SelectedIndex] + "_" + Rodovia.Text.ToUpper().Replace(" ", "") + "_KMI_" + KmInicio.Text + "_" +
                                    Sentido.Items[Sentido.SelectedIndex].Substring(0, 3) + "_" + LadoDaPista.Items[LadoDaPista.SelectedIndex].Substring(0, 1) +
                                    "_" + DateTime.Now.ToString("dd-MM-yyyy_HHmmss") + "_01.jpg";

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Name = _nomeDaFotoPanoramica,
                    Directory = "PPU",
                    CompressionQuality = 80
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

            if (_der == "" || string.IsNullOrEmpty(Rodovia.Text) || string.IsNullOrEmpty(KmInicio.Text) || _ladoDaPista == "" || _sentido == "")
            {
                await DisplayAlert("Alerta de campo sem preenchimento", "Campo obrigatório não preenchido", "OK");
                VerificarCampoNaoPreenchido();
                return;
            }

            _nomeDaFotoDetalhe1 = "PPU_DR" + DER.Items[DER.SelectedIndex] + "_" + Rodovia.Text.ToUpper().Replace(" ", "") + "_KMI_" + KmInicio.Text + "_" +
                                    Sentido.Items[Sentido.SelectedIndex].Substring(0, 3) + "_" + LadoDaPista.Items[LadoDaPista.SelectedIndex].Substring(0, 1) +
                                    "_" + DateTime.Now.ToString("dd-MM-yyyy_HHmmss") + "_02.jpg";
            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Name = _nomeDaFotoDetalhe1,
                    Directory = "PPU",
                    CompressionQuality = 80
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

            if (_der == "" || string.IsNullOrEmpty(Rodovia.Text) || string.IsNullOrEmpty(KmInicio.Text) || _ladoDaPista == "" || _sentido == "")
            {
                await DisplayAlert("Alerta de campo sem preenchimento", "Campo obrigatório não preenchido", "OK");
                VerificarCampoNaoPreenchido();
                return;
            }

            _nomeDaFotoDetalhe2 = "PPU_DR" + DER.Items[DER.SelectedIndex] + "_" + Rodovia.Text.ToUpper().Replace(" ", "") + "_KMI_" + KmInicio.Text + "_" +
                                    Sentido.Items[Sentido.SelectedIndex].Substring(0, 3) + "_" + LadoDaPista.Items[LadoDaPista.SelectedIndex].Substring(0, 1) +
                                    "_" + DateTime.Now.ToString("dd-MM-yyyy_HHmmss") + "_03.jpg";
            
            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Name = _nomeDaFotoDetalhe2,
                    Directory = "PPU",
                    CompressionQuality = 80
                });

            if (file != null)
            {
                FotoDetalhe2.Text = "Foto Detalhe 2 ✔";
                FotoDetalhe2.BackgroundColor = Color.Green;
                return;
            }
        }

        private void LerCampos(object sender, EventArgs e)
        {
            EntryRodovia(sender, e);
            PckDER(sender, e);
            EntryKmInicio(sender, e);
            EntryKmFinal(sender, e);
            EntryObservacao(sender, e);
        }

        private async void BtnSalvarDados(object sender, EventArgs e)
        {
            LerCampos(sender, e);
            BtnObterNomenclaturaDoElemento(sender, e);

            if (_der == "" || string.IsNullOrEmpty(Rodovia.Text))
            {
                await DisplayAlert("Alerta de campo sem preenchimento", "Campo obrigatório não preenchido", "OK");
                return;
            }

            _der = "DR" + _der;
            string _date = DateTime.Now.ToString("dd/MM/yyyy");
            string lines =
                _der + ";" +
                _rodovia + ";" +
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
                "1" + ";" +
                Preferences.Get("NomeUsuario", string.Empty).ToUpper() + ";" +
                Preferences.Get("IdDispositivo", string.Empty) + ";" +
                "9999999999";

            var root = new LocalRootFolder();
            string path = root.Path + "/files/Pictures/PPU";
            string fileName = _der + "_" + _rodovia + "_" + "PPU" + "_" + _kmInicio + "_" + _kmFim + "_" + _sentido + "_" + _ladoDaPista + ".txt";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string pathFileName = System.IO.Path.Combine(path, fileName);

            using (var streamWriter = new System.IO.StreamWriter(pathFileName, false))
            {
                streamWriter.WriteLine(lines);
            }

            await DisplayAlert("Dados Salvos", "As informações foram salvas com sucesso", "OK");
            RemoverPageRelacaoElementos();
            await App.Current.MainPage.Navigation.PushAsync(new Pages.PaginaPPURelacao());
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
            EntryCodigoElemento.Text = string.Empty;
        }

        private async void BtnObterNomenclaturaDoElemento(object sender, EventArgs e)
        {
            LerCampos(sender, e);
            if (Sentido.SelectedIndex == -1 || LadoDaPista.SelectedIndex == -1)
            {
                await DisplayAlert("e-coleta", "Por favor, confirme a seleção do LADO e SENTIDO", "OK");
                return;
            }
            _nomeDoElemento = "DR" + _der + "_" + _rodovia + "_PPU_" + "KM_" + _kmInicio + "_" + _kmFim + "_PSL" + LadoDaPista.Items[LadoDaPista.SelectedIndex].Substring(0, 1);
            EntryCodigoElemento.Text = _nomeDoElemento.ToUpper();
        }

        private async void BtnCancelarCadastro(object sender, EventArgs e)
        {
            bool cancelar = await DisplayAlert("Atenção", "Tem certeza que deseja cancelar esse cadastro?", "SIM", "NÃO");

            if (cancelar == true)
            {
                RemoverPageRelacaoElementos();
                await App.Current.MainPage.Navigation.PushAsync(new Pages.PaginaPPURelacao());
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
                if (name == "Cadastro de PPU")
                {
                    Navigation.RemovePage(this.Navigation.NavigationStack[i]);
                }
            }
        }
    }
}

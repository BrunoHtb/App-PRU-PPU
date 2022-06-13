using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Xamarin.Essentials;
using rodovias.ViewModels;

using Rg.Plugins.Popup.Services;
using PCLExt.FileStorage.Folders;

namespace rodovias.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaPPURelacao : ContentPage
    {
        UserModel Elemento = new UserModel();

        public class Dispositivo
        {
            public string Dipositivo { get; set; } = Preferences.Get("IdDispositivo", string.Empty);
        }
        private object senderr;
        private EventArgs ee;

        public PaginaPPURelacao()
        {
            InitializeComponent();
            BtnCarregarPlanejados_Clicked(senderr, ee);
            NavigationPage.SetTitleIcon(this, "@drawable/motorway.png");
        }

        private async void CmdRefresh()
        {
            await Task.Delay(2000);
        }

        private async void BtnCarregarPlanejados_Clicked(object sender, EventArgs e)
        {
            string dispositivoCadastrado = Preferences.Get("IdDispositivo", string.Empty);
            //string strDataSelecionada = pckData.Date.ToString("dd/MM/yyyy");
            var root = new LocalRootFolder();
            string _caminhoDoArquivoTxt = root.Path + "/files/Pictures/PPU";

            if (string.IsNullOrEmpty(dispositivoCadastrado) || dispositivoCadastrado == "0")
            {
                _ = App.Current.MainPage.DisplayAlert("Falta algo?", "Por favor, informe um número para o dispositivo", "OK");
                return;
            }
            DAL.DAL SelecionarTodos = new DAL.DAL();
            bool DiretorioDoApp = Directory.Exists(_caminhoDoArquivoTxt);
            if (DiretorioDoApp == false)
            {
                if (SelecionarTodos.totalResultado == 0) { await DisplayAlert("Que pena...", $"Sem dados para mostrar para o Dispositivo: '{dispositivoCadastrado}'", "OK"); return; }
                return;
            }
            else
            {
                //listReceberDadosExterno.ItemsSource = SelecionarTodos.ObterRegistro(strDataSelecionada);
                string[] totalDeArquivosTxtNoDiretorio = Directory.GetFiles(_caminhoDoArquivoTxt, "*.txt");
                for (int i = 0; i < totalDeArquivosTxtNoDiretorio.Length; i++)
                {
                    FileInfo fi = new FileInfo(Path.GetFullPath(totalDeArquivosTxtNoDiretorio[i]));
                    if (fi.Length == 0)
                    {
                        continue;
                    }
                    else
                    {
                        listReceberDadosExterno.ItemsSource = SelecionarTodos.ObterRegistroPPU(Path.GetFullPath(totalDeArquivosTxtNoDiretorio[i]));
                    }
                }

                if (totalDeArquivosTxtNoDiretorio.Length == 0)
                {
                    await DisplayAlert("e-coleta", $"Sem dados para visualizar", "OK");
                    return;
                }
                await DisplayAlert("VIVA!", $"Lista ATUALIZADA.\n\rTotal do dia: {SelecionarTodos.totalResultado}", "OK");
                OnPropertyChanged();
            }
        }

        private async void btnIncluirNovoElemento_Clicked(object sender, EventArgs e)
        {
            // navega para a pagina de novo cadastro 
            await Navigation.PushAsync(new Pages.PaginaCadastroNovoPPU());
        }

        private async void btnEditarElemento_Clicked(object sender, EventArgs e)
        {
            var index = listReceberDadosExterno.SelectedItem;

            if (index == null)
            {
                _ = DisplayAlert("Erro", "Sem seleção", "OK");
                return;
            }
            // navega para a pagina de edicao
            await Navigation.PushAsync(new Pages.PaginaCadastroEditarPPU());
        }

        private async void listReceberDadosExterno_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var valor = e.Item as UserModel;
            //_ = DisplayAlert("Item selecionado", valor.codigo, "OK");

            var _CodigoRegionalDR = e.Item as UserModel;
            //var prefixoRodovia = e.Item as UserModel;
            var _Rodovia = e.Item as UserModel; // recebe rodovia e prefixo
            //var _RegionalCidade = e.Item as UserModel;
            var _Lado = e.Item as UserModel;
            var _Sentido = e.Item as UserModel;
            var _Condicao = e.Item as UserModel;
            var _CodElemento = e.Item as UserModel;
            var _LatitudeInicio = e.Item as UserModel;
            var _LatitudeFim = e.Item as UserModel;
            var _LongitudeInicio = e.Item as UserModel;
            var _LongitudeFim = e.Item as UserModel;
            var _ObsCampo = e.Item as UserModel;
            var _NomePanoramica = e.Item as UserModel;
            var _NomeFotoDetalhe1 = e.Item as UserModel;
            var _NomeFotoDetalhe2 = e.Item as UserModel;
            var _KmInicio = e.Item as UserModel;
            var _KmFim = e.Item as UserModel;
            var _Status = e.Item as UserModel;
            var _Auditoria = e.Item as UserModel;
            var _ID = e.Item as UserModel;

            ClsEditarElemento.Regional_der = _CodigoRegionalDR.regional_der.Replace("DR", "");
            //ClsEditarElemento.PrefixoRodovia = _Rodovia.rodovia.Substring(0, 3);
            ClsEditarElemento.Rodovia = _Rodovia.rodovia;
            //ClsEditarElemento.Regional = _RegionalCidade.regional_cidade;
            ClsEditarElemento.Lado = _Lado.lado;
            ClsEditarElemento.Sentido = _Sentido.sentido;
            ClsEditarElemento.Condicao = _Condicao.condicao;
            ClsEditarElemento.CodigoElemento = _CodElemento.codigo;
            ClsEditarElemento.LatitudeInicio = _LatitudeInicio.latitude;
            ClsEditarElemento.LatitudeFim = _LatitudeFim.latitude_fim;
            ClsEditarElemento.LongitudeInicio = _LongitudeInicio.longitude;
            ClsEditarElemento.LongitudeFim = _LongitudeFim.longitude_fim;
            ClsEditarElemento.ObservacaoCampo = _ObsCampo.observacao;
            ClsEditarElemento.nome_fotopanoramica = _NomePanoramica.nome_fotopanoramica;
            ClsEditarElemento.nome_fotodetalhe_1 = _NomeFotoDetalhe1.nome_fotodetalhe_1;
            ClsEditarElemento.nome_fotodetalhe_2 = _NomeFotoDetalhe2.nome_fotodetalhe_2;
            ClsEditarElemento.KmInicio = _KmInicio.kmInicio;
            ClsEditarElemento.KmFim = _KmFim.kmFim;
            ClsEditarElemento.status_interno = _Status.status_interno;
            ClsEditarElemento.Auditoria = _Auditoria.auditoria;
            ClsEditarElemento.ID = _ID.id;

            var page = new PopUpPPU();
            await PopupNavigation.PushAsync(page);
        }

        private void pckData_DateSelected(object sender, DateChangedEventArgs e)
        {

        }

        private async void btnVoltarInicio_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private async void btnOrdenarElemento_Clicked(object sender, EventArgs e)
        {
            var ordenar = await DisplayActionSheet("Ordenar por: ", "Cancelar", null, "STATUS", "KM");
            var root = new LocalRootFolder();
            string _caminhoDoArquivoTxt = root.Path + "/files/Pictures/PPU";
            DAL.DAL SelecionarTodos = new DAL.DAL();
            bool DiretorioDoApp = Directory.Exists(_caminhoDoArquivoTxt);
            string dispositivoCadastrado = Preferences.Get("IdDispositivo", string.Empty);

            if (DiretorioDoApp == false)
            {
                if (SelecionarTodos.totalResultado == 0) { await DisplayAlert("Que pena...", $"Sem dados para mostrar para o Dispositivo: '{dispositivoCadastrado}'", "OK"); return; }
                return;
            }
            else
            {
                string[] totalDeArquivosTxtNoDiretorio = Directory.GetFiles(_caminhoDoArquivoTxt, "*.txt");

                if (DiretorioDoApp == false)
                {
                    await DisplayAlert("e-coleta", $"Sem dados para visualizar", "OK");
                    return;
                }
                switch (ordenar)
                {
                    case "STATUS":
                        if (DiretorioDoApp == false)
                        {
                            if (SelecionarTodos.totalResultado == 0) { await DisplayAlert("Atenção", $"Sem dados para mostrar para o Dispositivo: '{dispositivoCadastrado}'", "OK"); return; }
                            return;
                        }
                        else
                        {
                            listReceberDadosExterno.BeginRefresh();
                            listReceberDadosExterno.ItemsSource = null;
                            if (totalDeArquivosTxtNoDiretorio.Length == 0)
                            {
                                await DisplayAlert("e-coleta", $"Sem dados para visualizar", "OK");
                                return;
                            }
                            listReceberDadosExterno.ItemsSource = SelecionarTodos.ObterRegistroOrdenado(totalDeArquivosTxtNoDiretorio, "STATUS", "PPU");
                            OnPropertyChanged();
                            listReceberDadosExterno.EndRefresh();
                        }
                        break;
                    case "KM":
                        if (DiretorioDoApp == false)
                        {
                            if (SelecionarTodos.totalResultado == 0) { await DisplayAlert("Atenção", $"Sem dados para mostrar para o Dispositivo: '{dispositivoCadastrado}'", "OK"); return; }
                            return;
                        }
                        else
                        {
                            listReceberDadosExterno.BeginRefresh();
                            if (totalDeArquivosTxtNoDiretorio.Length == 0)
                            {
                                await DisplayAlert("e-coleta", $"Sem dados para visualizar", "OK");
                                return;
                            }
                            listReceberDadosExterno.ItemsSource = null;
                            listReceberDadosExterno.ItemsSource = SelecionarTodos.ObterRegistroOrdenado(totalDeArquivosTxtNoDiretorio, "KM", "PPU");
                            OnPropertyChanged();
                            listReceberDadosExterno.EndRefresh();
                        }
                        break;
                    case "Cancelar":
                        break;
                }
            }
            OnPropertyChanged();
        }
    }
}
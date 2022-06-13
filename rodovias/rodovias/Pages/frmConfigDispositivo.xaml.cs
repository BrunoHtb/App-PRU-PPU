using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace rodovias.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmConfigDispositivo : ContentPage
    {
        public frmConfigDispositivo()
        {
            InitializeComponent();
            txtIdDispositivoGravar.Text = Preferences.Get("IdDispositivo", string.Empty);
            txtNomeUtilizador.Text = Preferences.Get("NomeUsuario", string.Empty);
        }
        private async void CmdRefresh()
        {
            await Task.Delay(1000);
        }

        private void btnGravarConfigDisp_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdDispositivoGravar.Text) || string.IsNullOrEmpty(txtNomeUtilizador.Text))
            {
                DisplayAlert("Alerta", "Preencha todos os campos. Preenha antes de continuar","OK");
                return;
            } else
            {
                Preferences.Set("IdDispositivo", txtIdDispositivoGravar.Text);
                Preferences.Set("NomeUsuario", txtNomeUtilizador.Text);
            }
            DisplayAlert("DADOS GRAVADOS", $"Nº Dispositivo: {txtIdDispositivoGravar.Text}\n\rUtilizador: {txtNomeUtilizador.Text}", "OK");
        }

        private async void btnCancelarConfigDisp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage(), false);
        }
    }
}
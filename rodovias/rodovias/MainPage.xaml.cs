using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using Xamarin.Essentials;
using rodovias.Pages;


namespace rodovias
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnExit_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private async void btnEntrar_Clicked(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(txtNumDispositivo.Text) || string.IsNullOrEmpty(txtNomeUsuario.Text))
            //{
            //    await DisplayAlert("Alerta", "Preencha todos os campos", "Ok");
            //    return;
            //}
        }

        private async void btnConfigDispositivo_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.frmConfigDispositivo(), false);
        }

        private async void btnEnviarDados_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.frmEnviarDados(), false);
        }

        private async void BtnAbrirRelacaoPRU(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.PaginaPRURelacao(), false);
        }

        private async void BtnAbrirRelacaoPPU(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.PaginaPPURelacao(), false);
        }
    }
}

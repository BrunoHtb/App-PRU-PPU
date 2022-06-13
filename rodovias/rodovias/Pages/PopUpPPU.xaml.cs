using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup;
using Rg.Plugins;
using Xamarin.Forms;
using System.IO;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using rodovias.ViewModels;

namespace rodovias.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpPPU : Rg.Plugins.Popup.Pages.PopupPage
    {
        string filePath = "/storage/emulated/0/Android/data/com.esteio.rodoviasPRUePPU/files/" + "Pictures/PPU";
        string[] FotoRegistrosDoElemento = new string[3];

        public PopUpPPU()
        {
            InitializeComponent();

            FotoRegistrosDoElemento[0] = ClsEditarElemento.nome_fotopanoramica;
            FotoRegistrosDoElemento[1] = ClsEditarElemento.nome_fotodetalhe_1;
            FotoRegistrosDoElemento[2] = ClsEditarElemento.nome_fotodetalhe_2;

            if (!File.Exists(filePath + "//" + ClsEditarElemento.nome_fotopanoramica))
            {
                imgPrincipal.Source = "logosemfoto.png";
            }
            else
            {
                imgPrincipal.Source = filePath + "//" + ClsEditarElemento.nome_fotopanoramica;
                //lblNomeDoElemento.Text = "Elemento: " + ClsEditarElemento.CodigoElemento;
            }
            lblNomeDoElemento.Text = "Elemento: " + ClsEditarElemento.CodigoElemento;
        }
        int TotalClick = 0;
        private void btnPopNavegacao_Clicked(object sender, EventArgs e)
        {
            TotalClick += 1;
            if (TotalClick <= FotoRegistrosDoElemento.Length - 1)
            {
                imgPrincipal.Source = filePath + "//" + FotoRegistrosDoElemento[TotalClick];
                if (!File.Exists(filePath + "//" + FotoRegistrosDoElemento[TotalClick]))
                {
                    imgPrincipal.Source = "logosemfoto.png";
                }
                switch (TotalClick)
                {
                    case 1:
                        btnPopNavegacao.Text = "FOTO DETALHE 1";
                        break;
                    case 2:
                        btnPopNavegacao.Text = "FOTO DETALHE 2";
                        break;
                    default:
                        btnPopNavegacao.Text = "Navegar por fotos existentes do elemento";
                        break;
                }

            }
            else if (TotalClick > FotoRegistrosDoElemento.Length - 1)
            {
                TotalClick = 0;
                imgPrincipal.Source = filePath + "//" + FotoRegistrosDoElemento[TotalClick];
                btnPopNavegacao.Text = "FOTO PANORÂMICA";

            }
        }
    }
}
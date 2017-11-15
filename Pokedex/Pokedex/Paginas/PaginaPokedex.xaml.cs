using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.IO;
using Pokedex.Servicios;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;

namespace Pokedex.Paginas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaPokedex : ContentPage
    {
        static MemoryStream streamCopy;

        public PaginaPokedex()
        {
            InitializeComponent();
        }

        async Task ObtenerImagen(bool camara)
        {
            var archivo = await ServicioImagenes.TomarFoto(camara);
            lblResultado.Text = "---";

            imgFoto.Source = ImageSource.FromStream(() => {
                var stream = archivo.GetStream();
                streamCopy = new MemoryStream();
                stream.CopyTo(streamCopy);
                stream.Seek(0, SeekOrigin.Begin);
                archivo.Dispose();
                return stream;
            });
        }

        private async void btnCamara_Clicked(object sender, EventArgs e)
        {
            await ObtenerImagen(true);
        }

        private async void btnGaleria_Clicked(object sender, EventArgs e)
        {
            await ObtenerImagen(false);
        }

        private async void btnClasificar_Clicked(object sender, EventArgs e)
        {
            if (streamCopy != null)
            {
                streamCopy.Seek(0, SeekOrigin.Begin);
                var resultado = await ServicioClasificador.ClasificarImagen(streamCopy);
                lblResultado.Text = $"Es... {resultado}";
            }
            else
            {
                lblResultado.Text = "---No has seleccionado una imagen---";
            }
        }
    }
}
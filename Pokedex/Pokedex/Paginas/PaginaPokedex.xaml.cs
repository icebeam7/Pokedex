using System;
using Xamarin.Forms;
using System.IO;
using Pokedex.Servicios;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using Pokedex.Modelos;
using Pokedex.Helpers;

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

        void MostrarIndicador(bool mostrar)
        {
            actBuscando.IsVisible = mostrar;
            actBuscando.IsRunning = mostrar;
            actBuscando.IsEnabled = mostrar;
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
                MostrarIndicador(true);
                streamCopy.Seek(0, SeekOrigin.Begin);
                var resultado = await ServicioClasificador.ClasificarImagen(streamCopy);
                lblResultado.Text = $"Es... {resultado}";

                if (resultado != Constantes.PokemonNoIdentificado)
                {
                    Pokemon pokemon = await ServicioPokedexInfo.ObtenerInfo(resultado);

                    lblDetalle.Text = $"Tipo(s): {pokemon.Tipo1}/{pokemon.Tipo2}\n\nEspecie: {pokemon.Especie}\n\n{pokemon.Descripcion}";
                    ServicioAudio.PlayAudio(pokemon.ID);
                    await ServicioTextToSpeech.Speak($"{pokemon.Nombre}, {pokemon.Especie}, {pokemon.Descripcion}");
                }

                MostrarIndicador(false);
            }
            else
            {
                lblResultado.Text = "---No has seleccionado una imagen---";
            }
        }
    }
}
using Plugin.TextToSpeech;
using Plugin.TextToSpeech.Abstractions;
using System.Threading.Tasks;

namespace Pokedex.Servicios
{
    public static class ServicioTextToSpeech
    {
        public static async Task Speak(string texto)
        {
            var locale = new CrossLocale();
            locale.Country = "ES";
            locale.Language = "es";
            await CrossTextToSpeech.Current.Speak(texto, locale);
        }
    }
}

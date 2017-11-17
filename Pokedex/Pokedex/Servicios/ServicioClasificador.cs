using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Pokedex.Helpers;
using System;
using Pokedex.Modelos;
using Newtonsoft.Json;
using System.Linq;

namespace Pokedex.Servicios
{
    public static class ServicioClasificador
    {
        public async static Task<string> ClasificarImagen(MemoryStream stream)
        {
            try
            {
                var url = Constantes.PredictionURL;

                using (var cliente = new HttpClient())
                {
                    cliente.DefaultRequestHeaders.Add("Prediction-Key", Constantes.PredictionKey);

                    using (var content = new ByteArrayContent(stream.ToArray()))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        var post = await cliente.PostAsync(url, content);
                        var resultado = await post.Content.ReadAsStringAsync();
                        var cv = JsonConvert.DeserializeObject<CustomVisionResult>(resultado);

                        if (cv.Predictions.Count > 0)
                        {
                            var prediccion = ObtenerPrediccion(cv);
                            return prediccion.Probability > 0.5 ? prediccion.Tag : Constantes.PokemonNoIdentificado;
                            //return $"{prediccion.Tag} ({Math.Round(prediccion.Probability * 100, 2):0.##} %)";
                        }
                        else
                        {
                            return "Error al realizar la predicción. Intenta de nuevo.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Ocurrió una excepción: " + ex.Message;
            }
        }

        static Prediction ObtenerPrediccion(CustomVisionResult cv)
        {
            return cv.Predictions.OrderByDescending(x => x.Probability).Take(1).First();
        }
    }
}

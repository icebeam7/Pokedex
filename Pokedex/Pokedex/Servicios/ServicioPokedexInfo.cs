using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Pokedex.Helpers;
using System;
using Pokedex.Modelos;
using Newtonsoft.Json;

namespace Pokedex.Servicios
{
    public static class ServicioPokedexInfo
    {
        public async static Task<Pokemon> ObtenerInfo(string nombrePokemon)
        {
            try
            {
                var url = Constantes.PokedexFunctionURL;

                using (var cliente = new HttpClient())
                {
                    string json = "{\"NombrePokemon\":\"" + nombrePokemon + "\"}";
                    HttpContent content = new StringContent(json);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var post = await cliente.PostAsync(url, content);
                    var resultado = await post.Content.ReadAsStringAsync();
                    var pokemon = JsonConvert.DeserializeObject<Pokemon>(resultado);

                    return pokemon;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

using Plugin.SimpleAudioPlayer;

namespace Pokedex.Servicios
{
    public static class ServicioAudio
    {
        public static void PlayAudio(string ID)
        {
            var player = CrossSimpleAudioPlayer.Current;
            player.Load($"Pokesonidos/{ID}.wav");
            player.Play();
        }
    }
}

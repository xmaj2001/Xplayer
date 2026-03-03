using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLibrary
{
    public class Reprodutor : ListPlayItem
    {
        public Uri Reproduzir(bool LengendaFalada = false)
        {
            var video = this.Play();
            video.lengenda.Falar = LengendaFalada;
            return video.Ur;
        }

        public Uri Reproduzir(PlayItem item,bool LengendaFalada = false)
        {
            var video = this.Play(item);
            video.lengenda.Falar = LengendaFalada;
            return video.Ur;
        }

        public PlayItem PlayAtual()
        {
            return this.Play();
        }
    }
}

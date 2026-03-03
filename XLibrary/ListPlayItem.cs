using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLibrary
{
    public class ListPlayItem
    {
        private List<PlayItem> Videos = new List<PlayItem>();
        private int item = 0;
        private PlayItem _Video = null;
        public void addVideos(PlayItem video)
        {
            if (!Videos.Exists(vd => vd.Nome == video.Nome))
            {
                Videos.Add(video);
            }
        }

        public int Itens()
        {
            if (Videos != null)
            {
                return Videos.Count;
            }
            return 0;
        }

        public List<PlayItem> Itens(bool lista = true)
        {
            if (Videos != null)
            {
                return Videos;
            }
            return null;
        }
        public void Next()
        {
            item++;
            if (item > (Videos.Count - 1)) item = 0;
            _Video = Videos[item];
        }

        public void Prev()
        {
            item--;
            if (item < 0) item = (Videos.Count - 1);
            _Video = Videos[item];
        }

        public void Remover(int index)
        {
            if (index < Videos.Count)
                Videos.RemoveAt(index);
        }

        public PlayItem Play()
        {
            if (_Video == null && Videos.Count > 0)
            {
                _Video = Videos[0];
                return _Video;
            }
            else if (_Video != null)
            {
                return _Video;
            }
            else
            {
                return null;
            }
        }

        public PlayItem Play(PlayItem item)
        {
            _Video = Videos.Find(x => x == item);
            if (_Video != null) return _Video; else return null;
        }
        public void Remover(PlayItem video)
        {
            Videos.Remove(video);
        }
    }
}

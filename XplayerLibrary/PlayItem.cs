using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XplayerLibrary
{
    public class PlayItem
    {
        public string Nome { get; set; }
        public Uri Ur { get; set; }
        public DirectoryInfo Pasta { get; set; }
        public Tempo tempo { get; set; }

        public PlayItem() { }
        public PlayItem(string url)
        {
            if (File.Exists(url))
            {
                var file = new FileInfo(url);
                this.Ur = new Uri(file.FullName);
                this.Pasta = file.Directory;
            }
        }
    }
}

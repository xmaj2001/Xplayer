using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XLibrary
{
    public class PlayItem
    {
        public string Nome { get; set; }
        public Uri Ur { get; set; }
        public DirectoryInfo Pasta { get; set; }
        public Legenda lengenda { get; set; }
        public Tempo tempo { get; set; }
        public PlayItem() { }
        public PlayItem(string url)
        {
            if (File.Exists(url))
            {
                lengenda = new Legenda();
                var file = new FileInfo(url);
                this.Nome = file.Name.Replace(file.Extension, string.Empty);
                this.Ur = new Uri(file.FullName);
                this.Pasta = file.Directory;
                if (this.Pasta != null)
                {
                    var files = this.Pasta.GetFiles(this.Nome + ".srt");
                    if (files != null && files.Length > 0)
                    {
                        var srt = files[0];
                        lengenda.LoadSubtitulo(srt.FullName);
                    }
                }
            }
        }
    }
}

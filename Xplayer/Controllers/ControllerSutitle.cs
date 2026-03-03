using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xplayer.Model;

namespace Xplayer.Controllers
{
   public class ControllerSutitle
    {
        private string srtFilePath = "";

        public ControllerSutitle (string url)
        {
            this.srtFilePath = url;
        }
        public List<Subtitle> LoadSubtitles()
        {
            List<Subtitle> subtitles = new List<Subtitle>();
            
            return subtitles;
        }
    }
}

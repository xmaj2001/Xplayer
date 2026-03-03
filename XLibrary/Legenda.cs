using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Speech.Synthesis;
namespace XLibrary
{
    public class Legenda
    {
        public List<Subtitulo> Subtitulos { get; set; }
        private SpeechSynthesizer sp = new SpeechSynthesizer();
        private string falou = "";
        public bool Falar = false;
        public string GetLegenda(TimeSpan CorrentTime)
        {
            if (Subtitulos != null && Subtitulos.Count > 0)
            {
                var sub = Subtitulos.FirstOrDefault(x => CorrentTime >= x.tempo.StartTime && CorrentTime <= x.tempo.EndTime);
                if (sub != null)
                {
                    if (Falar)
                    {
                        sp.SpeakAsync(sub.Text);
                        falou = sub.Text;
                    }
                    else
                    {
                        if(sp.State == SynthesizerState.Speaking) sp.SpeakAsyncCancelAll();
                    }
                    return sub.Text;
                }
            }

            return "";
        }
        //Carregar Subtitulos ou Legendas .SRT
        public void LoadSubtitulo(string url)
        {
            Subtitulos = new List<Subtitulo>();
            string[] lines = File.ReadAllLines(url);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var match = Regex.Match(line, @"(\d+:\d+:\d+,\d+) --> (\d+:\d+:\d+,\d+)");
                    if (match.Success)
                    {
                        var ptext = i;
                        var sub = new Subtitulo();
                        sub.tempo.StartTime = TimeSpan.Parse(match.Groups[1].Value.Replace(',', '.'));
                        sub.tempo.EndTime = TimeSpan.Parse(match.Groups[2].Value.Replace(',', '.'));
                        while (true)
                        {
                            ptext++;
                            if (ptext >= lines.Length) break;
                            line = lines[ptext];
                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                sub.Text += line;
                            }
                            else
                            {
                                Subtitulos.Add(sub);
                                break;
                            }

                        }
                    }
                }
            }
        }
    }
}

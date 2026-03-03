using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace XLibrary
{
    public class Tema
    {
        public Brush Texto { get; set; }
        public Brush Fundo { get; set; }
       
        public Tema() { }
        public Tema(Brush texto,Brush fundo)
        {
            this.Texto = texto;
            this.Fundo = fundo;
        }
    }

    public class ListaTema
    {
        private List<Tema> Temas = new List<Tema>();

        public ListaTema()
        {
            var defaultTemaText = new SolidColorBrush(Color.FromArgb(255, 53, 234, 0));
            var defaultTemaFundo = new SolidColorBrush(Color.FromArgb(255, 5, 5, 5));

            add(new Tema(defaultTemaText, defaultTemaFundo));
            add(new Tema(defaultTemaText, Brushes.White));

            add(new Tema(Brushes.White, defaultTemaFundo));
            add(new Tema(defaultTemaFundo, Brushes.White));

            add(new Tema(Brushes.DeepPink, defaultTemaFundo));
            add(new Tema(Brushes.White,Brushes.DeepPink));
            add(new Tema(Brushes.DeepPink, Brushes.White));

            add(new Tema(Brushes.Aqua, defaultTemaFundo));
            add(new Tema(Brushes.Aqua, Brushes.White));

            add(new Tema(Brushes.Red, Brushes.White));
            add(new Tema(Brushes.White, Brushes.Red));

            add(new Tema(Brushes.White, Brushes.Maroon));
            add(new Tema(Brushes.Maroon, Brushes.White));

            add(new Tema(Brushes.White, Brushes.DeepSkyBlue));
            add(new Tema(Brushes.DeepSkyBlue, Brushes.White));
            add(new Tema(Brushes.DeepSkyBlue, defaultTemaFundo));



        }
        public void add(Tema tema)
        {
            if(!Temas.Exists(t=> t == tema))
            {
                Temas.Add(tema);
            }
        }

        public List<Tema> GET()
        {
            return Temas;
        }
        public Tema Tema(int index)
        {
            if (index >= Temas.Count) index = Temas.Count - 1;
            return Temas[index];
        }
        public Tema dark()
        {
            return Temas[0];
        }

        public Tema white()
        {
            return Temas[1];
        }
    }
}

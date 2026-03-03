using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Xplayer;
using System.Speech.Synthesis;
namespace Xplayer
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer Update;
        private SpeechSynthesizer sp = new SpeechSynthesizer();
        private Controllers.ControllerSutitle subs = new Controllers.ControllerSutitle(@"C:\Users\MAXI5\Videos\Cinema\Arcane League of Legends\Eps\Arcane League of Legends\1ª Temporada\Arcane.League.of.Legends.S01E01.720p.WEB-DL.DUAL.5.1.srt");
        public MainWindow()
        {
            InitializeComponent();
        }

        void StarTimer()
        {
            Update = new DispatcherTimer();
            Update.Interval = TimeSpan.FromMilliseconds(100);
            Update.Tick += Update_Tick;
            Update.Start();
        }
        string tt = "";
        private void Update_Tick(object sender, EventArgs e)
        {
            if (ML.NaturalDuration.HasTimeSpan)
            {
                TempoRolando(ML.Position, ML.NaturalDuration.TimeSpan);

                var sub = subs.LoadSubtitles().FirstOrDefault(x => ML.Position >= x.StartTime && ML.Position <= x.EndTime);
                if (sub != null)
                {
                    subtitleText.Text = sub.Text;
                   if(tt != sub.Text) sp.SpeakAsync(sub.Text);
                    tt = sub.Text;
                }
                else
                {
                    subtitleText.Text = string.Empty;
                    //sp.SpeakAsyncCancelAll();
                }
            }
        }

        public void TempoRolando(TimeSpan Posicao, TimeSpan Duracao)
        {
            _timeline.Maximum = ML.NaturalDuration.TimeSpan.TotalSeconds;
            _timeline.Value = ML.Position.TotalSeconds;
            tmp.Text = Posicao.ToString(@"hh\:mm\:ss");
            tmpf.Text = Duracao.ToString(@"hh\:mm\:ss");
            if (Posicao == Duracao)
            {
                _timeline.Value = 0.0;
                tmp.Foreground = Brushes.Gray;
                tmpf.Foreground = Brushes.Gray;
                Update.Stop();
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _draw.IsBottomDrawerOpen = !_draw.IsBottomDrawerOpen;
        }

        private void ViewPalyer_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length > 0 && File.Exists(files[0]))
                {
                    ML.Source = new Uri(files[0]);
                    StarTimer();
                    ML.Play();
                }
            }
            _draw.IsBottomDrawerOpen = false;
        }

        private void _timeline_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void ML_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private void _timeline_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            tmp.Text = TimeSpan.FromSeconds(_timeline.Value).ToString(@"hh\:mm\:ss");
        }

        private void _timeline_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ML.Position = TimeSpan.FromSeconds(_timeline.Value);
            ML.Play();
            Update.Start();
        }

        private void _timeline_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ML.Pause();
            Update.Stop();
        }
    }
}

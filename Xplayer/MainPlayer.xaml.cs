using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Speech.Synthesis;
using System.IO;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;

namespace Xplayer
{
    /// <summary>
    /// Lógica interna para MainPlayer.xaml
    /// </summary>
    public partial class MainPlayer : Window
    {
        private SpeechSynthesizer sp = new SpeechSynthesizer();
        private XLibrary.ListaTema Temas = new XLibrary.ListaTema();
        private XLibrary.Reprodutor rep = new XLibrary.Reprodutor();
        private DispatcherTimer Update;
        private bool Auto_Next = false;
        private bool Auto_Repite = false;

        private bool play = false;
        public MainPlayer()
        {
            InitializeComponent();
            LoadTema(Temas.white());
            Auto_Next = true;
            Update = new DispatcherTimer();
            Update.Interval = TimeSpan.FromMilliseconds(100);
            Update.Tick += Update_Tick;
        }

        private void BarraTop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }

        public void StartTime()
        {
            Update.Start();
            ML.Play();
            play = true;
            _listaPro.SelectedItem = rep.PlayAtual();
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
                Update.Stop();
            }
        }

        private void Update_Tick(object sender, EventArgs e)
        {
            if (ML.NaturalDuration.HasTimeSpan)
            {
                TempoRolando(ML.Position, ML.NaturalDuration.TimeSpan);
                _legenda.Text = rep.Play().lengenda.GetLegenda(ML.Position);
            }
        }

        public void LoadTema(XLibrary.Tema _tema)
        {
            if (_listTemas.ItemsSource == null) _listTemas.ItemsSource = Temas.GET();
            tema.Foreground = _tema.Texto;
            tema.Background = _tema.Fundo;
        }
        private void MainController_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            if (btn == btn_close)
            {
                sp.SpeakAsyncCancelAll();
                this.Close();
            }
            if (btn == btn_min) this.WindowState = WindowState.Minimized;
        }

        private void _timeline_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ML.Source != null)
            {
                var pose = TimeSpan.FromSeconds(_timeline.Value);
                if (pose != ML.Position) ML.Position = pose;
                StartTime();
            }
        }

        private void _draw_Drop(object sender, DragEventArgs e)
        {
            var exten = new List<string>() { ".mp4", ".avi", ".mkv", ".ts" };
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (var item in files)
                {
                    if (File.Exists(item))
                    {
                        var fl = new FileInfo(item);
                        if (exten.Contains(fl.Extension))
                            rep.addVideos(new XLibrary.PlayItem(item));
                    }
                }

                if (rep.Itens() != 0)
                {
                    ML.Source = rep.Reproduzir();
                    _listaPro.ItemsSource = rep.Itens(true);
                    _listaPro.Items.Refresh();
                    StartTime();
                  
                }
            }
            _draw.IsBottomDrawerOpen = false;
        }

        private void ML_MediaOpened(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _draw.Focus();
            _draw.IsBottomDrawerOpen = !_draw.IsBottomDrawerOpen;
        }

        private void _timeline_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ML.Source != null)
            {
                Update.Stop();
                ML.Pause();
                play = false;
            }
        }

        private void _timeline_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ML.Source != null)
            {
                tmp.Text = TimeSpan.FromSeconds(_timeline.Value).ToString(@"hh\:mm\:ss");
            }
        }

        private void ML_MediaEnded(object sender, RoutedEventArgs e)
        {
            Update.Stop();
            ML.Stop();
            play = false;
            if (rep.Itens() > 1 && Auto_Next && !Auto_Repite)
            {
                rep.Next();
                ML.Source = rep.Reproduzir();
                StartTime();
            }
            else if (Auto_Repite)
            {
                ML.Stop();
                StartTime();
            }
        }

        private void btn_controller_video_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;

            if (btn == btn_next)
            {
                if (rep.Itens() > 1)
                {
                    rep.Next();
                    ML.Source = rep.Reproduzir();
                    StartTime();
                }
            }

            if (btn == btn_prev)
            {
                if (rep.Itens() > 1)
                {
                    rep.Prev();
                    ML.Source = rep.Reproduzir();
                    StartTime();
                }
            }

            if (btn == btn_stop)
            {
                if (ML.Source != null)
                {
                    ML.Stop();
                    Update.Stop();
                    play = false;
                    _timeline.Value = 0.0;
                    tmp.Text = "00:00:00";
                    tmpf.Text = "00:00:00";
                }
            }

            if (btn == btn_repitir)
            {
                Auto_Repite = !Auto_Repite;
                if (Auto_Repite == true) ((PackIcon)btn.Content).Kind = PackIconKind.CircleMedium; else ((PackIcon)btn.Content).Kind = PackIconKind.Circle;
            }

            if (btn == btn_player)
            {
                if (ML.Source != null)
                {
                    if (play)
                    {
                        ML.Pause();
                        Update.Stop();
                        play = false;
                        ((PackIcon)btn.Content).Kind = PackIconKind.Pause;
                    }
                    else
                    {

                        ((PackIcon)btn.Content).Kind = PackIconKind.Play;
                        StartTime();
                    }

                }
            }
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            //var g = (Grid)sender;
            //g.Opacity = 1;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            //var g = (Grid)sender;
            //g.Opacity = 0.5;
        }

        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                BarraTop.Visibility = Visibility.Visible;
                _draw.Margin = new Thickness(0, 30, 0, 0);
                BarraBottom.Margin = new Thickness(0, 0, 0, 0);
                backSlider.Padding = new Thickness(4, 0, 4, 0);
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                BarraTop.Visibility = Visibility.Collapsed;
                _draw.Margin = new Thickness(0, 0, 0, 0);
                BarraBottom.Margin = new Thickness(0, 0, 0, 10);
                backSlider.Padding = new Thickness(10, 0, 10, 0);
            }
            _draw.Focus();

        }
        private void _draw_KeyUp(object sender, KeyEventArgs e)
        {
            if (ML.Source != null)
            {
                switch (e.Key)
                {
                    case Key.Right:
                        Update.Stop();
                        play = false;
                        ML.Pause();
                        ML.Position += TimeSpan.FromSeconds(10);
                        StartTime();
                        break;
                    case Key.Left:
                        Update.Stop();
                        play = false;
                        ML.Pause();
                        ML.Position -= TimeSpan.FromSeconds(10);
                        StartTime();
                        break;
                    case Key.Space:
                        if (play)
                        {
                            ML.Pause();
                            Update.Stop();
                            play = false;
                            ((PackIcon)btn_player.Content).Kind = PackIconKind.Pause;
                        }
                        else
                        {
                            ((PackIcon)btn_player.Content).Kind = PackIconKind.Play;
                            StartTime();
                        }
                        break;
                    case Key.N:
                        if (rep.Itens() > 1)
                        {
                            rep.Next();
                            ML.Source = rep.Reproduzir();
                            StartTime();
                        }
                        break;
                    case Key.P:
                        if (rep.Itens() > 1)
                        {
                            rep.Prev();
                            ML.Source = rep.Reproduzir();
                            StartTime();
                        }
                        break;
                    case Key.M:
                        ML.IsMuted = !ML.IsMuted;
                        if (ML.IsMuted)
                        {
                            ((PackIcon)btn_mude.Content).Kind = PackIconKind.AudioOff;
                        }
                        else
                        {
                            ((PackIcon)btn_mude.Content).Kind = PackIconKind.Audio;
                        }
                        break;
                    default:
                        break;
                }

            }
        }

        private void btn_mude_Click(object sender, RoutedEventArgs e)
        {
            ML.IsMuted = !ML.IsMuted;
            if (ML.IsMuted)
            {
                ((PackIcon)btn_mude.Content).Kind = PackIconKind.AudioOff;
            }
            else
            {
                ((PackIcon)btn_mude.Content).Kind = PackIconKind.Audio;
            }

        }

        private void volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ML.Volume <= 0)
            {
                ((PackIcon)btn_mude.Content).Kind = PackIconKind.AudioOff;
            }
            else
            {
                ((PackIcon)btn_mude.Content).Kind = PackIconKind.Audio;
            }
        }

        private void _listTemas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_listTemas.SelectedIndex >= 0)
            {
                var tt = (XLibrary.Tema)_listTemas.Items[_listTemas.SelectedIndex];
                LoadTema(tt);
            }
        }

        private void btn_close_tema_Click(object sender, RoutedEventArgs e)
        {
            Dia_Tema.IsOpen = false;
        }

        private void TituloPlay_Click(object sender, RoutedEventArgs e)
        {
            Dia_Tema.IsOpen = true;
        }

        private void btn_List_Click(object sender, RoutedEventArgs e)
        {
            _draw.IsRightDrawerOpen = !_draw.IsRightDrawerOpen;
        }

        private void _listaPro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void _listaPro_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (_listaPro.SelectedIndex >= 0)
            {
                var item = (XLibrary.PlayItem)_listaPro.SelectedItem;
                var item2 = (ListBoxItem)_listaPro.SelectedItem;

                if (item != rep.PlayAtual())
                {
                    ML.Source = rep.Reproduzir(item);
                    StartTime();
                }
            }
        }
    }
}

using iTunesLib;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace iTunesAdjuster
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.IsEnabled = false;
            this.InitializeComponent();
            this.IsEnabled = true;
        }

        private void playlistGenerateButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    this.Dispatch(() =>
                    {
                        this.IsEnabled = false;
                    });

                    var itunes = new iTunesApp();

                    var trackGroups =
                        itunes.LibraryPlaylist.GetCommentedTracks()
                        .GroupBy(x => x.Comment.Split('@').First())
                        .ToArray();

                    trackGroups.AsParallel().ForAll(trackGroup =>
                    {
                        var playlist = itunes.CreatePlaylist(trackGroup.Key) as IITUserPlaylist;

                        playlist.SongRepeat = ITPlaylistRepeatMode.ITPlaylistRepeatModeAll;

                        var orderdTracks = trackGroup.OrderBy("Comment", "DiscNumber", "TrackNumber");

                        playlist.AddRangeTracks(orderdTracks.ToArray());
                    });

                    this.Dispatch(() =>
                    {
                        MessageBox.Show(this, "completed");
                    });
                }
                finally
                {
                    this.Dispatch(() =>
                    {
                        this.IsEnabled = true;
                    });
                }
            });
        }

        private void playlistTrackSortButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;

                new PlaylistTrackSortWindow().ShowDialog();
            }
            finally
            {
                this.IsEnabled = true;
            }
        }
    }
}

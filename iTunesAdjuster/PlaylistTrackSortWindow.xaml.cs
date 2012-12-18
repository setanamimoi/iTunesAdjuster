using iTunesLib;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace iTunesAdjuster
{
    /// <summary>
    /// PlaylistTrackSortWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class PlaylistTrackSortWindow : Window
    {
        public PlaylistTrackSortWindow()
        {
            this.IsEnabled = false;

            this.InitializeComponent();

            Task.Factory.StartNew(() =>
            {
                try
                {
                    this.Dispatch(() =>
                    {
                        this.playlistSortListView.ItemsSource =
                            new iTunesApp().LibrarySource.GetUserPlaylists().Select(x =>
                            new UserPlaylistViewModel()
                            {
                                Name = x.Name,
                                IsChecked = false,
                                Model = x,
                            });
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

        private void sortButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    this.Dispatch(() =>
                    {
                        this.IsEnabled = false;
                    });

                    var iTunes = new iTunesApp();

                    var playlists =
                        this.playlistSortListView.Items
                        .Cast<UserPlaylistViewModel>()
                        .Where(x => x.IsChecked == true);

                    playlists.AsParallel().ForAll(playlist =>
                    {
                        var created = iTunes.CreatePlaylist(playlist.Name) as IITUserPlaylist;

                        var source = playlist.Model;

                        created.Shuffle = source.Shuffle;
                        created.SongRepeat = source.SongRepeat;

                        created.AddRangeTracks(
                            source.Tracks
                            .Cast<IITTrack>()
                            .OrderBy("Comment", "DiscNumber", "TrackNumber")
                            .ToArray());

                        source.Delete();

                        playlist.Model = created;
                        playlist.IsChecked = false;
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
    }

    public class UserPlaylistViewModel : INotifyPropertyChanged
    {
        #region IsChecked プロパティ
        public virtual bool IsChecked
        {
            get
            {
                return this._IsChecked;
            }
            set
            {
                this.OnPropertyChanged("IsChecked");
                this._IsChecked = value;
            }
        }
        private bool _IsChecked = false;
        #endregion

        #region Name プロパティ
        public virtual string Name { get; set; }
        #endregion

        #region Model プロパティ
        public virtual IITUserPlaylist Model { get; set; }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

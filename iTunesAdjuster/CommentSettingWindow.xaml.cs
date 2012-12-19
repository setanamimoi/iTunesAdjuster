using iTunesLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace iTunesAdjuster
{
    /// <summary>
    /// CommentSettingWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CommentSettingWindow : Window
    {
        public CommentSettingWindow()
        {
            InitializeComponent();

            var itunes = new iTunesApp();

            var o = itunes.LibraryPlaylist.Tracks.Cast<IITTrack>().GroupBy(x => x.Album).ToArray();

            List<ViewModel> ret = new List<ViewModel>();

            foreach (var a in o)
            {
                var minAritistsLength = a.ToArray().Select(x => x.Artist.Length).Min();

                string artist = a.ToArray().Where(x => x.Artist.Length == minAritistsLength).First().Artist;

                var model = new ViewModel();
                model.IsInitailizing = true;
                model.AlbumName = a.Key;

                if (a.ToArray().Where(x => string.IsNullOrWhiteSpace(x.Comment) == true).Any() == false)
                {
                    if (a.ToArray().Select(x => x.Comment.Split('@').First()).Distinct().Count() == 1)
                    {
                        model.ArtistName = a.ToArray().Select(x => x.Comment.Split('@').First()).Distinct().First();
                    }

                    if (string.IsNullOrWhiteSpace(model.ArtistName) == false)
                    {
                        if (a.ToArray().Select(x => x.Comment.Split('@').ElementAtOrDefault(1)).Distinct().Count() == 1)
                        {
                            model.ReleaseYear = a.ToArray().Select(x => x.Comment.Split('@').ElementAtOrDefault(1)).Distinct().First();
                        }
                    }

                    if (string.IsNullOrWhiteSpace(model.ReleaseYear) == false)
                    {
                        if (a.ToArray().Select(x => x.Comment.Split('@').ElementAtOrDefault(2)).Distinct().Count() == 1)
                        {
                            model.ReleaseOrder = a.ToArray().Select(x => x.Comment.Split('@').ElementAtOrDefault(2)).Distinct().First();
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(model.ArtistName) == true)
                {
                    model.ArtistName = artist;
                }
                model.IsInitailizing = false;

                ret.Add(model);
            }

            this.dataGrid.ItemsSource = ret.ToArray();
        }

        public class ViewModel : INotifyPropertyChanged
        {
            #region IsInitailizing プロパティ
            public virtual bool IsInitailizing { get; set; }
            #endregion

            #region IsChecked プロパティ
            public virtual bool IsChecked
            {
                get
                {
                    return this._isChecked;
                }
                set
                {
                    if (this.IsChecked == value)
                    {
                        return;
                    }

                    this._isChecked = value;

                    this.OnPropertyChanged("IsChecked");
                }
            }
            private bool _isChecked;
            #endregion

            #region AlbumName プロパティ
            public virtual string AlbumName { get; set; }
            #endregion

            #region ArtistName プロパティ
            public virtual string ArtistName
            {
                get
                {
                    return this._artistName;
                }
                set
                {
                    if (this.ArtistName == value)
                    {
                        return;
                    }

                    if (this.IsInitailizing == false)
                    {
                        this.IsChecked = true;
                    }

                    this._artistName = value;

                    this.OnPropertyChanged("ArtistName");
                }
            }

            private string _artistName;
            #endregion

            #region ReleaseYear プロパティ
            public virtual string ReleaseYear
            {
                get
                {
                    return this._releaseYear;
                }
                set
                {
                    if (this.ReleaseYear == value)
                    {
                        return;
                    }

                    if (this.IsInitailizing == false)
                    {
                        this.IsChecked = true;
                    }

                    this._releaseYear = value;

                    this.OnPropertyChanged("ReleaseYear");
                }
            }
            private string _releaseYear;
            #endregion

            #region ReleaseOrder プロパティ
            public virtual string ReleaseOrder
            {
                get
                {
                    return this._releaseOrder;
                }
                set
                {
                    if (this.ReleaseOrder == value)
                    {
                        return;
                    }

                    if (this.IsInitailizing == false)
                    {
                        this.IsChecked = true;
                    }

                    this._releaseOrder = value;

                    this.OnPropertyChanged("ReleaseOrder");
                }
            }
            private string _releaseOrder;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var items =
                this.dataGrid.Items.Cast<ViewModel>()
                .Where(x => x.IsChecked == true).ToArray();

            var tracks= new iTunesAppClass().LibraryPlaylist.Tracks.Cast<IITTrack>();

            foreach (var item in items)
            {
                var targets = tracks.Where(x => x.Album == item.AlbumName).ToArray();


                var comment = string.Join("@", new string[]{
                    Convert.ToString(item.ArtistName),
                    Convert.ToString(item.ReleaseYear),
                    Convert.ToString(item.ReleaseOrder)
                }.Where(x => string.IsNullOrWhiteSpace(x) == false).ToArray());

                foreach (var t in targets)
                {
                    t.Comment = comment;
                }

                var stop = "";

                
            }

            MessageBox.Show("completed");
        }
    }
}

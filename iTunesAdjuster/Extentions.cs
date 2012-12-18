using iTunesAdjuster.ArgumentValidators;
using iTunesLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace iTunesAdjuster
{
    /// <summary>
    /// アプリケーションを拡張メソッドを定義したクラス
    /// </summary>
    public static class Extentions
    {
        /// <summary>
        /// 指定したトラック情報をプレイリストに追加します。
        /// </summary>
        /// <param name="self">拡張元のインスタンス</param>
        /// <param name="tracks">プレイリストに追加するトラック情報</param>
        [Require]
        public static void AddRangeTracks(this IITUserPlaylist self, params IITTrack[] tracks)
        {
            ArgumentValidator.Validate(self, tracks);

            foreach (IITTrack track in tracks)
            {
                self.AddTrack(track);
            }
        }

        /// <summary>
        /// プロパティの値を返します。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="obj">プロパティ値が返されるオブジェクト</param>
        /// <returns>obj パラメーターのプロパティ値</returns>
        private static object GetValue(this PropertyInfo self, object obj)
        {
            return self.GetValue(obj, null);
        }

        /// <summary>
        /// シーケンスの要素に従って昇順に並び替えます。
        /// </summary>
        /// <param name="self">拡張元のインスタンス</param>
        /// <param name="sequenceElements">シーケンス要素</param>
        /// <returns>並び替えた配列</returns>
        [Require]
        public static IOrderedEnumerable<IITTrack> OrderBy(this IEnumerable<IITTrack> self, params string[] sequenceElements)
        {
            ArgumentValidator.Validate(self, sequenceElements);

            var declareType = typeof(IITTrack);

            var properties = sequenceElements.Select(x => declareType.GetProperty(x));

            IOrderedEnumerable<IITTrack> ret = null;

            foreach (var property in properties)
            {
                if (ret == null)
                {
                    ret = self.OrderBy(x => property.GetValue(x));
                    continue;
                }

                ret = ret.ThenBy(x => property.GetValue(x));
            }

            return ret;
        }

        /// <summary>
        /// コメントが設定されたトラックを取得します。
        /// </summary>
        /// <param name="self">拡張元インスタンス</param>
        /// <returns>コメントが設定されたトラックの配列。</returns>
        [Require]
        public static IITTrack[] GetCommentedTracks(this IITLibraryPlaylist self)
        {
            ArgumentValidator.Validate(self);

            var ret = new List<IITTrack>();

            //モックオブジェクトのGetEnumerator が実行されるように foreach を使用して要素を取得する
            foreach (IITTrack track in self.Tracks)
            {
                ret.Add(track);
            }

            return ret
                .Where(x => x.Comment != null)
                .Where(x => string.IsNullOrEmpty(x.Comment.Trim()) == false)
                .ToArray();
        }

        /// <summary>
        /// ユーザ定義プレイリストを取得します。
        /// </summary>
        /// <param name="self">拡張元インスタンス</param>
        /// <returns>ユーザ定義プレイリスト</returns>
        [Require]
        public static IITUserPlaylist[] GetUserPlaylists(this IITSource self)
        {
            ArgumentValidator.Validate(self);

            var ret = new List<IITUserPlaylist>();
            foreach (IITPlaylist playlist in self.Playlists)
            {
                var add = playlist as IITUserPlaylist;
                if (add == null)
                {
                    continue;
                }
                if (add.SpecialKind != ITUserPlaylistSpecialKind.ITUserPlaylistSpecialKindNone)
                {
                    continue;
                }

                ret.Add(add);
            }
            return ret.ToArray();
        }

        /// <summary>
        /// 指定したアクションを同期的に実行します。
        /// </summary>
        /// <param name="self">拡張元オブジェクト</param>
        /// <param name="action">実行するアクション</param>
        [Require]
        public static void Dispatch(this Window self, Action action)
        {
            self.Dispatcher.Invoke(action);
        }
    }
}

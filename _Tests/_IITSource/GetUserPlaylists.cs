using iTunesAdjuster;
using iTunesLib;
using Moq;
using NUnit.Framework;
using System;

namespace _Tests._IITSource
{
    [TestFixture]
    public class GetUserPlaylists
    {
        [Test]
        public void IITUserPlaylistにキャストできる要素のみ抽出する()
        {
            var playlistMock1 = new Mock<IITUserPlaylist>();
            playlistMock1.Setup(x => x.SpecialKind).Returns(ITUserPlaylistSpecialKind.ITUserPlaylistSpecialKindNone);
            var playlistMock2 = new Mock<IITUserPlaylist>();
            playlistMock2.Setup(x => x.SpecialKind).Returns(ITUserPlaylistSpecialKind.ITUserPlaylistSpecialKindNone);
            var playlistMock3 = new Mock<IITPlaylist>();
            var playlistMock4 = new Mock<IITUserPlaylist>();
            playlistMock4.Setup(x => x.SpecialKind).Returns(ITUserPlaylistSpecialKind.ITUserPlaylistSpecialKindMovies);

            var playlistsMock = new Mock<IITPlaylistCollection>();

            playlistsMock
                .Setup(x => x.GetEnumerator())
                .Returns(new IITPlaylist[]{
                    playlistMock1.Object,
                    playlistMock2.Object,
                    playlistMock3.Object,
                    playlistMock4.Object,
                }.GetEnumerator());

            var targetMock = new Mock<IITSource>();
            targetMock.Setup(x => x.Playlists).Returns(playlistsMock.Object);

            CollectionAssert.AreEqual(
                new IITUserPlaylist[]{
                    playlistMock1.Object,
                    playlistMock2.Object,
                },
                targetMock.Object.GetUserPlaylists());

            playlistMock1.VerifyAll();
            playlistMock2.VerifyAll();
            playlistMock3.VerifyAll();
            playlistMock4.VerifyAll();
            targetMock.VerifyAll();
        }

        [Test]
        public void 引数がnullの場合例外をスローする()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Extentions.GetUserPlaylists(null);

                Assert.IsTrue(false);
            });
        }
    }
}
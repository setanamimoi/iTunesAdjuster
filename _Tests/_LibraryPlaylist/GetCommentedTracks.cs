using iTunesLib;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using iTunesAdjuster;
using System;

namespace _Tests._LibraryPlaylist
{
    [TestFixture]
    public class GetCommentedTracks
    {
        [Test]
        public void コメントが設定されているトラックのみ抽出する()
        {
            var trackMock1 = new Mock<IITTrack>();
            trackMock1.Setup(x => x.Comment).Returns("ABC");

            var trackMock2 = new Mock<IITTrack>();
            trackMock2.Setup(x => x.Comment).Returns("ABC@1");

            var trackMock3 = new Mock<IITTrack>();
            trackMock3.Setup(x => x.Comment).Returns("");

            string nullString = null;
            var trackMock4 = new Mock<IITTrack>();
            trackMock4.Setup(x => x.Comment).Returns(nullString);

            var trackMock5 = new Mock<IITTrack>();
            trackMock5.Setup(x => x.Comment).Returns(" ");

            var libraryPlaylistMock = new Mock<IITLibraryPlaylist>();

            var targets = new IITTrack[]
            { 
                trackMock1.Object,
                trackMock2.Object,
                trackMock3.Object,
                trackMock4.Object,
                trackMock5.Object,
            };

            var trackCollectionMock = new Mock<IITTrackCollection>();
            trackCollectionMock.Setup(x => x.GetEnumerator()).Returns(targets.GetEnumerator());

            libraryPlaylistMock.Setup(x => x.Tracks).Returns(trackCollectionMock.Object);

            var actuals = new List<IITTrack>();

            var expected = new IITTrack[]
            { 
                trackMock1.Object,
                trackMock2.Object,
            };

            CollectionAssert.AreEqual(
                expected,
                libraryPlaylistMock.Object.GetCommentedTracks());

            trackMock1.VerifyAll();
            trackMock2.VerifyAll();
            trackMock3.VerifyAll();
            trackMock4.VerifyAll();
            trackMock5.VerifyAll();
            trackCollectionMock.VerifyAll();
            libraryPlaylistMock.VerifyAll();
        }

        [Test]
        public void 引数がnullの場合例外をスローする()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Extentions.GetCommentedTracks(null);

                Assert.IsTrue(false);
            });
        }
    }
}

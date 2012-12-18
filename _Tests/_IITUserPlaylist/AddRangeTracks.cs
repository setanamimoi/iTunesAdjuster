using iTunesLib;
using Moq;
using NUnit.Framework;
using iTunesAdjuster;
using System;

namespace _Tests._IITUserPlaylist
{
    [TestFixture]
    public class AddRangeTracks
    {
        [Test]
        public void 指定した配列すべてが追加される()
        {
            var trackMock1 = new Mock<IITTrack>();
            var trackMock2 = new Mock<IITTrack>();
            var trackMock3 = new Mock<IITTrack>();

            var actuals = new IITTrack[] { 
                trackMock1.Object,
                trackMock2.Object,
                trackMock3.Object,
            };

            var targetMock = new Mock<IITUserPlaylist>();

            foreach (var actual in actuals)
            {
                object refObject = (object)actual;
                targetMock.Setup(m => m.AddTrack(ref refObject));
            }

            var target = targetMock.Object;
            target.AddRangeTracks(actuals);

            trackMock1.VerifyAll();
            trackMock2.VerifyAll();
            trackMock3.VerifyAll();

            targetMock.VerifyAll();
        }

        [Test]
        public void 引数がnullの場合例外をスローする()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Extentions.AddRangeTracks(null);

                Assert.IsTrue(false);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                Mock<IITUserPlaylist> mock = new Mock<IITUserPlaylist>();
                mock.Object.AddRangeTracks(null);

                Assert.IsTrue(false);
            });
        }
    }
}

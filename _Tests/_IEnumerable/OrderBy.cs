using iTunesAdjuster;
using iTunesLib;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace _Tests._IEnumerable
{
    [TestFixture]
    public class OrderBy
    {
        [Test]
        public void 並び替えができる()
        {
            var trackMock1 = new Mock<IITTrack>();
            trackMock1.Setup(x => x.Comment).Returns("b");
            trackMock1.Setup(x => x.DiscNumber).Returns(1);
            trackMock1.Setup(x => x.TrackNumber).Returns(1);

            var trackMock2 = new Mock<IITTrack>();
            trackMock2.Setup(x => x.Comment).Returns("b");
            trackMock2.Setup(x => x.DiscNumber).Returns(1);
            trackMock2.Setup(x => x.TrackNumber).Returns(2);

            var trackMock3 = new Mock<IITTrack>();
            trackMock3.Setup(x => x.Comment).Returns("b");
            trackMock3.Setup(x => x.DiscNumber).Returns(2);
            trackMock3.Setup(x => x.TrackNumber).Returns(1);

            var trackMock4 = new Mock<IITTrack>();
            trackMock4.Setup(x => x.Comment).Returns("b");
            trackMock4.Setup(x => x.DiscNumber).Returns(3);
            trackMock4.Setup(x => x.TrackNumber).Returns(1);

            var trackMock5 = new Mock<IITTrack>();
            trackMock5.Setup(x => x.Comment).Returns("c");
            trackMock5.Setup(x => x.DiscNumber).Returns(2);
            trackMock5.Setup(x => x.TrackNumber).Returns(1);

            var trackMock6 = new Mock<IITTrack>();
            trackMock6.Setup(x => x.Comment).Returns("d");
            trackMock6.Setup(x => x.DiscNumber).Returns(2);
            trackMock6.Setup(x => x.TrackNumber).Returns(1);

            var targets = new IITTrack[]
            {
                trackMock6.Object,
                trackMock5.Object,
                trackMock4.Object,
                trackMock3.Object,
                trackMock2.Object,
                trackMock1.Object,
            };

            var actuals = targets.OrderBy("Comment", "DiscNumber", "TrackNumber");

            var expected = targets.OrderBy(x => x.Comment).ThenBy(x => x.DiscNumber).ThenBy(x => x.TrackNumber);
            CollectionAssert.AreEqual(actuals.ToArray(), expected.ToArray());

            trackMock1.VerifyAll();
            trackMock2.VerifyAll();
            trackMock3.VerifyAll();
            trackMock4.VerifyAll();
            trackMock5.VerifyAll();
            trackMock6.VerifyAll();
        }

        [Test]
        public void 引数がnullの場合例外をスローする()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                Extentions.OrderBy(null);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                new IITTrack[] { }.OrderBy(null);

                Assert.IsTrue(false);
            });
        }
    }
}

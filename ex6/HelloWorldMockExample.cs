using System.IO;
using Moq;
using NUnit.Framework;

namespace ex6
{
    [TestFixture]
    public class HelloWorldMockExample
    {
        [Test]
        public void Buffered_Stream_Closes_Output_On_Close()
        {
            var mock = new Mock<Stream>();
            Stream stream = mock.Object;
            mock.Setup(s => s.CanRead).Returns(true);
            BufferedStream bs = new BufferedStream(stream);
            bs.Close();
            mock.Verify(s => s.Close());
        }

        public void Buffered_Stream_Rethrows_Exceptions_From_Underlying_Stream()
        {
            var mock = new Mock<Stream>();
            Stream stream = mock.Object;
            mock.SetupGet(d => d.CanRead).Returns(true);
            mock.Setup(d => d.Close()).Throws(new IOException());

            BufferedStream bs = new BufferedStream(stream);
            Assert.Throws<IOException>(() => bs.Close());
        }

        [Test]
        public void Buffered_Stream_buffers_and_forwards_writes_and_flushes_before_close()
        {
            var mock = new Mock<Stream>();
            Stream stream = mock.Object;
            mock.SetupGet(d => d.CanRead).Returns(true);
            mock.SetupGet(d => d.CanWrite).Returns(true);

            BufferedStream bs = new BufferedStream(stream);
            bs.WriteByte((byte) 'a');
            bs.Flush();
            bs.Close();

            mock.Verify(d => d.Write(It.Is<byte[]>(array => array.Length > 0 && array[0] == 'a'), 0, 1));
            mock.Verify(d => d.Flush());
            mock.Verify(d => d.Close());
        }
    }
}
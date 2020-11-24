using Moq;
using NUnit.Framework;

namespace roulette
{
    public interface IDocumentRepository
    {
        bool IsReady { get; }
        string FetchDocument(string url);
    }

    [TestFixture]
    public class HelloWorldStubExampleTest
    {

        [Test]
        public void Should_understand_how_to_stub_a_property()
        {
            var stub = new Mock<IDocumentRepository>();
            IDocumentRepository documentRepository = stub.Object;

            stub.Setup(documentRepository => documentRepository.IsReady).Returns(true);

            Assert.IsTrue(documentRepository.IsReady);
        }

        [Test]
        public void Should_understand_how_to_stub_a_method()
        {
            var stub = new Mock<IDocumentRepository>();
            IDocumentRepository documentRepository = stub.Object;

            stub.Setup(documentRepository => documentRepository.FetchDocument("https://www.google.com")).Returns("<html><body></body></html>");

            Assert.AreEqual("<html><body></body></html>", documentRepository.FetchDocument("https://www.google.com"));
            Assert.AreEqual(null, documentRepository.FetchDocument("http://www.google.com"));
        }

        [Test]
        public void Should_understand_how_to_stub_a_method_with_unspecified_arguments()
        {
            var stub = new Mock<IDocumentRepository>();
            IDocumentRepository documentRepository = stub.Object;

            stub.Setup(documentRepository => documentRepository.FetchDocument(It.IsAny<string>())).Returns("<html><body>Hello World!</body></html>");

            Assert.AreEqual("<html><body>Hello World!</body></html>", documentRepository.FetchDocument("https://www.google.com"));
            Assert.AreEqual("<html><body>Hello World!</body></html>", documentRepository.FetchDocument("http://www.google.com"));
        }
    }
}

using NUnit.Framework;

namespace roulette
{
    [TestFixture]
    public class SingleNumberWinningStrategyTest
    {
        [Test]
        public void Wins_only_on_a_particular_single_number()
        {
            var strategy = new SingleNumberWinningStrategy(2);
            Assert.IsFalse(strategy.WinsOn(0));
            Assert.IsFalse(strategy.WinsOn(1));
            Assert.IsTrue(strategy.WinsOn(2));
            for (var i = 3; i < 37; i++)
                Assert.IsFalse(strategy.WinsOn(i));
        }
    }
}

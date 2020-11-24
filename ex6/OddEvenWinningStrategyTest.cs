using NUnit.Framework;

namespace roulette
{
    [TestFixture]
    public class OddEvenWinningStrategyTest
    {
        [Test]
        public void wins_on_even_if_odd_was_false()
        {
            OddEvenWinningStrategy strategy = new OddEvenWinningStrategy(false);
            for (int i = 1; i < 36; i += 2)
            {
                Assert.IsTrue(strategy.WinsOn(i + 1));
                Assert.IsFalse(strategy.WinsOn(i));
            }
        }

        [Test]
        public void wins_on_odd_if_odd_was_true()
        {
            OddEvenWinningStrategy strategy = new OddEvenWinningStrategy(true);
            for (int i = 1; i < 36; i += 2)
            {
                Assert.IsTrue(strategy.WinsOn(i));
                Assert.IsFalse(strategy.WinsOn(i + 1));
            }
        }

        [Test]
        public void loses_on_zero_regardless_of_odd()
        {
            Assert.IsFalse(new OddEvenWinningStrategy(true).WinsOn(0));
            Assert.IsFalse(new OddEvenWinningStrategy(false).WinsOn(0));
        }
    }
}
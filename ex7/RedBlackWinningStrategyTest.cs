using NUnit.Framework;

namespace roulette
{
    [TestFixture]
    public class RedBlackWinningStrategyTest
    {
        private int[] redNumbers = new int[] {1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36};
        private int[] blackNumbers = new int[] {2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35};

        [Test]
        public void Red_wins_on_red_numbers()
        {
            foreach (int i in redNumbers)
            {
                Assert.IsTrue(new RedBlackWinningStrategy(true).WinsOn(i));
                Assert.IsFalse(new RedBlackWinningStrategy(false).WinsOn(i));
            }
        }

        [Test]
        public void Both_red_and_black_lose_on_zero()
        {
            Assert.IsFalse(new RedBlackWinningStrategy(true).WinsOn(0));
            Assert.IsFalse(new RedBlackWinningStrategy(false).WinsOn(0));
        }

        [Test]
        public void black_wins_on_black_numbers()
        {
            foreach (int i in blackNumbers)
            {
                Assert.IsFalse(new RedBlackWinningStrategy(true).WinsOn(i), "" + i);
                Assert.IsTrue(new RedBlackWinningStrategy(false).WinsOn(i), "" + i);
            }
        }
    }
}
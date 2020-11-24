using NUnit.Framework;

namespace roulette
{
    [TestFixture]
    public class FieldTest
    {
        [Test]
        public void forNumber_returns_field_with_that_number_and_single_number_winning_strategy()
        {
            Field f = Field.ForNumber(1);
            Assert.AreEqual("1", f.FieldName);
            Assert.AreEqual(new SingleNumberWinningStrategy(1), f.WinningStrategy);
        }

        [Test]
        public void ODD_has_an_odd_even_winning_strategy()
        {
            Assert.AreEqual(new OddEvenWinningStrategy(true), Field.ODD.WinningStrategy);
        }

        [Test]
        public void EVEN_has_an_odd_even_winning_strategy()
        {
            Assert.AreEqual(new OddEvenWinningStrategy(false), Field.EVEN.WinningStrategy);
        }

        [Test]
        public void RED_has_a_red_black_winning_strategy()
        {
            Assert.AreEqual(new RedBlackWinningStrategy(true), Field.RED.WinningStrategy);
        }

        [Test]
        public void BLACK_has_a_red_black_winning_strategy()
        {
            Assert.AreEqual(new RedBlackWinningStrategy(false), Field.BLACK.WinningStrategy);
        }

        [Test]
        public void ONE_EIGHTEEN_has_a_half_winning_strategy()
        {
            Assert.AreEqual(new HalfWinningStrategy(1), Field.ONE_EIGHTEEN.WinningStrategy);
        }

        [Test]
        public void NINETEEN_TIRTYSIX_has_a_half_winning_strategy()
        {
            Assert.AreEqual(new HalfWinningStrategy(2), Field.NINETEEN_THIRTYSIX.WinningStrategy);
        }

        [Test]
        public void ONE_TWELVE_has_a_third_winning_strategy()
        {
            Assert.AreEqual(new ThirdWinningStrategy(1), Field.ONE_TWELVE.WinningStrategy);
        }

        [Test]
        public void THIRTEEN_TWENTYFOUR_has_a_third_winning_strategy()
        {
            Assert.AreEqual(new ThirdWinningStrategy(2), Field.THIRTEEN_TWENTYFOUR.WinningStrategy);
        }

        [Test]
        public void TWENTYFIVE_THIRTYSIX_has_a_third_winning_strategy()
        {
            Assert.AreEqual(new ThirdWinningStrategy(3), Field.TWENTYFIVE_THIRTYSIX.WinningStrategy);
        }
    }
}
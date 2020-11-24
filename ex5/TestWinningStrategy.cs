namespace roulette
{
    public class TestWinningStrategy : WinningStrategy
    {
        readonly int expectedPosition;
        readonly bool expectedOutcome;

        public TestWinningStrategy(int expectedPosition, bool expectedOutcome)
        {
            this.expectedOutcome = expectedOutcome;
            this.expectedPosition = expectedPosition;
        }

        public bool WinsOn(int wheelPosition)
        {
            return wheelPosition == expectedPosition && expectedOutcome;
        }
    }
}

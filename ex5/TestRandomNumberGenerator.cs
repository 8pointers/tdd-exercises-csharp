namespace roulette
{
    public class TestRandomNumberGenerator : RandomNumberGenerator
    {
        public int NextOutcome { get; set; }

        public int Generate(int from, int to)
        {
            return NextOutcome;
        }
    }
}
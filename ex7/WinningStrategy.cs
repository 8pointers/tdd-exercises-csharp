namespace roulette
{
    public interface WinningStrategy
    {
        bool WinsOn(int wheelPosition);
    }
}
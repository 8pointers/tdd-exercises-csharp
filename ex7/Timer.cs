namespace roulette
{
    public delegate void TimerCallBack();

    public interface Timer
    {
        void CallBack(long howMuchLaterInMillis, TimerCallBack what);

        long TimeInMillis { get; }
    }
}
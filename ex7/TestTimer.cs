namespace roulette
{
    public class TestTimer : Timer
    {
        private long currentMillis;
        private long executeRunnableMillis;
        private TimerCallBack delegateToExecute;

        public void CallBack(long howMuchLaterInMillis, TimerCallBack what)
        {
            executeRunnableMillis = howMuchLaterInMillis + currentMillis;
            delegateToExecute = what;
        }

        public long TimeInMillis
        {
            get { return currentMillis; }
        }

        public void MoveTime(long millis)
        {
            currentMillis += millis;
            if (delegateToExecute != null && executeRunnableMillis <= currentMillis)
            {
                delegateToExecute();
                delegateToExecute = null;
                executeRunnableMillis = 0;
            }
        }
    }
}
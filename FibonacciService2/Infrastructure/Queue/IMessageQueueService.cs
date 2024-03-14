namespace FibonacciService2.Infrastructure.Queue
{
    public interface IMessageQueueService
    {
        void SendToFibQueue(long result);
    }
}

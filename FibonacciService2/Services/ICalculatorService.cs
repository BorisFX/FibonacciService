namespace FibonacciService2.Services
{
    public interface ICalculatorService
    {
        long CalculateFibonacci(long n);
        bool IsInFibonacciSequence(int number);
    }
}

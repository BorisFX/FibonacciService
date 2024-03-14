namespace FibonacciService2.APIContracts
{
    public record CalculateFibonacciStepRequest
    {
        public long FibonacciNumber { get;  set; }
    }
}

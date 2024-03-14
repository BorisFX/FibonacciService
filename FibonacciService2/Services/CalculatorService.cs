using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FibonacciService2.Services
{
    public class CalculatorService : ICalculatorService
    {
        public long CalculateFibonacci(long n)
        {
            double a = n * (1 + Math.Sqrt(5)) / 2.0;
            return (long)Math.Round(a);
        }

        // Функция для определения, принадлежит ли число к последовательности Фибоначчи
        public bool IsInFibonacciSequence(int number)
        {          
            int a = 0;
            int b = 1;
            while (b < number)
            {
                int temp = b;
                b += a;
                a = temp;
            }
            return b == number;
        }
    }
}

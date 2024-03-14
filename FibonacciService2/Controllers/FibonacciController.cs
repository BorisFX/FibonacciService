using FibonacciService2.APIContracts;
using FibonacciService2.Infrastructure.Queue;
using FibonacciService2.Services;
using Microsoft.AspNetCore.Mvc;

namespace FibonacciService1.Controllers
{
    [Route("api/fib")]
    [ApiController]
    public class FibonacciController(ICalculatorService calculatorService, IMessageQueueService messageQueueService) : ControllerBase
    {
        private readonly ICalculatorService _calculatorService = calculatorService;
        private readonly IMessageQueueService _messageQueueService = messageQueueService;

        /// <summary>
        /// Метод используется для запуска расчета последовательности Фибоначчи.
        /// Принимает начальное число последовательности и отправляет его в очередь для дальнейших вычислений.
        /// Если начальное число не является членом последовательности Фибоначчи или равно 0 или 1, возвращается ошибка.
        /// </summary>
        /// <param name="startCalculatingRequest">Модель запроса с начальным числом для расчета последовательности Фибоначчи.</param>
        [HttpPost("start")]
        public IActionResult StartCalculating([FromBody] StartCalculatingRequest startCalculatingRequest)
        {
            if (startCalculatingRequest.StartNumber == 0 || startCalculatingRequest.StartNumber == 1)
            {
                return BadRequest("Число не может быть равно 0 или 1.");
            }

            if (!_calculatorService.IsInFibonacciSequence(startCalculatingRequest.StartNumber))
            {
                return BadRequest("Число не принадлежит последовательности Фибоначчи.");
            }

            _messageQueueService.SendToFibQueue(startCalculatingRequest.StartNumber);

            return Ok();
        }

        [HttpPost("next-step")]
        public IActionResult CalculateFibonacciStep([FromBody] CalculateFibonacciStepRequest  calculateFibonacciStepRequest)
        {
            var result = _calculatorService.CalculateFibonacci(calculateFibonacciStepRequest.FibonacciNumber);
            _messageQueueService.SendToFibQueue(result);

            return Ok(result);
        }
    }
}

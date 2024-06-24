// Controllers/CalculatorController.cs

using System;
using Microsoft.AspNetCore.Mvc;

namespace LucasNumberCalculator.Controllers
{
    public class CalculatorController : Controller
    {
        [HttpPost("/calculate")]
        public IActionResult Calculate([FromBody] CalculationRequest request)
        {
            if (request == null || request.n < 0)
            {
                return BadRequest();
            }

            // 計算時間を測定
            var startTime = DateTime.UtcNow;
            long lucasNumber = CalculateLucasNumber(request.n);
            var endTime = DateTime.UtcNow;
            var processTime = (endTime - startTime).TotalMilliseconds;

            return Ok(new
            {
                result = lucasNumber,
                process_time = processTime
            });
        }

        private long CalculateLucasNumber(int n)
        {
            if (n == 0)
                return 2;
            if (n == 1)
                return 1;

            // 意図的に遅い再帰関数を使用
            return CalculateLucasNumber(n - 1) + CalculateLucasNumber(n - 2);
        }

        public class CalculationRequest
        {
            public int n { get; set; }
        }
    }
}

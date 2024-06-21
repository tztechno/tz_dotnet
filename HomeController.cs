using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MyMvcApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CalculateLucas([FromBody] LucasRequest request)
        {
            int n = request.n;

            // Lucas数を計算する処理
            long result = CalculateLucasNumber(n);

            // 計算時間を計測する
            Stopwatch stopwatch = Stopwatch.StartNew();

            // 何らかの計算処理をシミュレート
            System.Threading.Thread.Sleep(1000); // 1秒間の待機を追加

            stopwatch.Stop();
            double processTime = stopwatch.Elapsed.TotalMilliseconds;

            return Json(new { result, process_time = processTime });
        }

        private long CalculateLucasNumber(int n)
        {
            if (n == 0) return 2;
            if (n == 1) return 1;
            return CalculateLucasNumber(n - 1) + CalculateLucasNumber(n - 2);
        }

        public class LucasRequest
        {
            public int n { get; set; }
        }
    }
}



---

エラーの原因として、`.NET Core MVC` アプリケーションで正しいルーティングが設定されていない可能性があります。`.NET Core` では、ルーティングの設定が適切でないと、リクエストが正しいエンドポイントにルーティングされないため、`404 Not Found` エラーが発生します。

以下に、正しい設定方法を示します。まずは、ルーティングの設定を確認してください。

### 1. ルーティングの設定

`.NET Core MVC` アプリケーションでは、`Startup.cs` ファイル内でルーティングを設定します。以下の手順で設定を確認・修正してください。

#### Startup.cs の修正

`MyMvcApp/Startup.cs` ファイルで、`UseEndpoints` メソッドを使用してルーティングを設定します。

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MyMvcApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
```

### 2. コントローラーとアクションメソッドの設定

`HomeController` に `CalculateLucas` アクションメソッドが正しく設定されていることを確認します。

#### HomeController.cs の修正

`MyMvcApp/Controllers/HomeController.cs` ファイル内で、`CalculateLucas` アクションメソッドを以下のように定義します。

```csharp
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
```

### 3. JavaScriptの修正

`public/index.html` 内のJavaScript部分を、正しいエンドポイントに向けて修正します。

```html
<!DOCTYPE html>
<html>

<head>
    <title>Lucas Number Calculator</title>
</head>

<body>
    <p>AJAX Lucas Number</p>
    <input type="number" id="inputN" placeholder="Enter a number">
    <button onclick="sendRequest()">Calculate</button>
    <div id="result"></div>
    <div id="time"></div>

    <script>
        function sendRequest() {
            const n = document.getElementById('inputN').value;
            fetch('/Home/CalculateLucas', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ n })
            })
                .then(response => response.json())
                .then(data => {
                    document.getElementById('result').innerText = `Lucas Number L${n} = ${data.result}`;
                    document.getElementById('time').innerText = `Time: ${(data.process_time / 1000).toFixed(9)} sec`;
                })
                .catch(error => {
                    console.error('Error:', error);
                });
        }
    </script>
</body>

</html>
```

### 4. アプリケーションのビルドと実行

ターミナルで以下のコマンドを実行し、アプリケーションをビルドして実行します。

```bash
dotnet build
dotnet run
```

### 5. 動作確認

ブラウザで `http://localhost:5000` にアクセスし、数値を入力して計算ボタンをクリックします。計算結果と計算時間が表示されれば、通信が正常に行われています。

以上の手順で、`.NET Core MVC` アプリケーションでHTMLをホストし、Ajax通信でバックエンドの計算結果を取得する方法を確認してください。

---

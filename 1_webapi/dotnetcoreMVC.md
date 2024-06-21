###
# .NET Core MVC
###

---

.NET Core MVCを使用してHTMLをホストする手順を説明します。

### 1. .NET Core SDKのインストール

まず最初に、.NET Core SDKをインストールしてください。公式のダウンロードページ（https://dotnet.microsoft.com/download）から最新バージョンをダウンロードしてインストールします。

### 2. 新しい .NET Core MVC プロジェクトの作成

次に、ターミナル（コマンドプロンプト）を開き、以下のコマンドを実行して新しい .NET Core MVC プロジェクトを作成します。

```bash
dotnet new mvc -o MyMvcApp
cd MyMvcApp
```

### 3. HTMLファイルの追加

`MyMvcApp/wwwroot` ディレクトリに静的なHTMLファイルを追加します。例えば、`MyMvcApp/wwwroot/index.html` という名前で以下の内容のHTMLファイルを作成します。

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
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

### 4. コントローラーとアクションメソッドの追加

次に、コントローラーとアクションメソッドを追加して、Ajaxリクエストに応答する処理を記述します。

`MyMvcApp/Controllers/HomeController.cs` ファイルを作成し、以下のようにアクションメソッドを追加します。

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MyMvcApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return File("~/index.html", "text/html");
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

### 5. ルーティングの設定

`MyMvcApp/Startup.cs` ファイルでルーティングを設定します。`Startup.cs` の `Configure` メソッド内で `UseStaticFiles()` を有効にし、`UseEndpoints` メソッドでルーティングを設定します。

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MyMvcApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app)
        {
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

### 6. アプリケーションの実行

ターミナル（コマンドプロンプト）で以下のコマンドを実行してアプリケーションをビルドし、実行します。

```bash
dotnet build
dotnet run
```

### 7. 動作確認

ブラウザで `http://localhost:5000` にアクセスすると、`public/index.html` の内容が表示されます。数値を入力して計算ボタンをクリックすると、Ajaxリクエストが送信され、結果と計算時間が表示されるはずです。

以上で、.NET Core MVCを使用して静的なHTMLファイルをホストし、Ajax通信を用いてバックエンドでの計算を行い、フロントエンドに結果を返す設定が完了しました。

---

###
# MVC
###

---

再帰関数を使用して、意図的に遅い処理となるように `CalculateLucasNumber` メソッドを実装します。Lucas 数列は再帰的な定義を持つため、再帰を利用して計算することができます。以下にその実装例を示します。

```csharp
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
```

この実装では、`CalculateLucasNumber` メソッドが再帰的に自身を呼び出しており、指数時間の計算量を持つため、大きな数に対しては計算に時間がかかるようになっています。これにより、意図的に処理を遅くしています。

動作確認の際には、適切な `index.html` ファイルを作成し、そのフォームからのリクエストを送信して、正常に動作することを確認してください。

---

この場合、ASP.NET Core MVC アプリケーションで、フロントエンドからの AJAX リクエストを受け取り、その値を処理して結果を返す方法を説明します。具体的には、以下の手順に従います。

### 手順概要

1. **ASP.NET Core MVC プロジェクトの作成**
2. **Controller の作成**
3. **ルーティングの設定**
4. **Lucas 数の計算と時間計測の実装**
5. **フロントエンドとの連携**

### 手順詳細

#### 1. ASP.NET Core MVC プロジェクトの作成

まず、ASP.NET Core MVC プロジェクトを作成します。既にプロジェクトがある場合は、次の手順に進んでください。

```bash
dotnet new mvc -o LucasNumberCalculator
cd LucasNumberCalculator
```

#### 2. Controller の作成

Lucas 数の計算と時間計測を行うためのコントローラを作成します。

```csharp
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

            long a = 2, b = 1;
            long result = 0;

            for (int i = 2; i <= n; i++)
            {
                result = a + b;
                a = b;
                b = result;
            }

            return result;
        }

        public class CalculationRequest
        {
            public int n { get; set; }
        }
    }
}
```

#### 3. ルーティングの設定

`Startup.cs` ファイルで、ルーティングを設定して `CalculatorController` を有効にします。

```csharp
// Startup.cs

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LucasNumberCalculator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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

            app.UseAuthorization();

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

#### 4. フロントエンドとの連携

`index.html` ファイルにある `sendRequest()` 関数で、`/calculate` への POST リクエストを送信し、結果を表示します。

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
            fetch('/calculate', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ n })
            })
                .then(response => response.json())
                .then(data => {
                    document.getElementById('result').innerText = `Lucas Number L${n} = ${data.result}`;
                    document.getElementById('time').innerText = `Time: ${(data.process_time / 1000).toFixed(6)} sec`;
                })
                .catch(error => {
                    console.error('Error:', error);
                });
        }
    </script>
</body>

</html>
```

### 動作確認

1. アプリケーションをビルドして実行します。

   ```bash
   dotnet build
   dotnet run
   ```

2. ブラウザで `http://localhost:5000` もしくは `http://localhost:5001` にアクセスします（ポート番号は実行時に表示されるポート番号に置き換えてください）。

3. `index.html` に数値を入力して "Calculate" ボタンをクリックします。計算された Lucas 数とその計算時間が表示されるはずです。

これで、ASP.NET Core MVC アプリケーションでフロントエンドとの連携を行う Lucas 数計算のサンプルが完成しました。

---

C# .NET Core MVCを使用して、ルートフォルダ `myapp` に `index.html` をホストする手順を以下に示します。

### 手順概要

1. **プロジェクトの作成**
2. **静的ファイルの追加**
3. **ルーティングの設定**
4. **ビルドと実行**

### 手順詳細

#### 1. プロジェクトの作成

まず、C# .NET Core MVC プロジェクトを作成します。

1. **コマンドラインでプロジェクトを作成する場合**:
   ```
   dotnet new mvc -o myapp
   cd myapp
   ```

2. **Visual Studio または Visual Studio Code を使用する場合**:
   - Visual Studio: 新しいプロジェクトを作成し、MVC プロジェクトを選択します。
   - Visual Studio Code: `dotnet new mvc -o myapp` コマンドを使用してプロジェクトを作成し、Visual Studio Code でフォルダを開きます。

#### 2. 静的ファイルの追加

`wwwroot` フォルダに `index.html` を配置します。

1. `wwwroot` フォルダが存在しない場合は、プロジェクトのルートに新しいフォルダ `wwwroot` を作成します。
2. `wwwroot` フォルダ内に `index.html` ファイルを配置します。例えば、`wwwroot/index.html`。

#### 3. ルーティングの設定

MVC のルーティングを設定して、`index.html` をルートでホストできるようにします。

1. `Startup.cs` ファイルを開きます。
2. `Configure` メソッド内に以下のようにルーティングを設定します:

   ```csharp
   public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
   {
       // 静的ファイルの使用を宣言
       app.UseStaticFiles();

       // デフォルトのルーティングを設定
       app.UseRouting();

       // ルートパスへのリクエストをindex.htmlにルーティング
       app.UseEndpoints(endpoints =>
       {
           endpoints.MapControllerRoute(
               name: "default",
               pattern: "{controller}/{action=Index}/{id?}");
           endpoints.MapFallbackToFile("/index.html");
       });
   }
   ```

   - `UseStaticFiles()` は静的ファイルの提供を有効にします。
   - `MapFallbackToFile("/index.html")` は、ルートパス (`/`) へのリクエストが来た場合に `index.html` を返すようにします。

#### 4. ビルドと実行

プロジェクトをビルドし、実行します。

1. コマンドラインから:
   ```
   dotnet build
   dotnet run
   ```

2. Visual Studio または Visual Studio Code からは、IDE のビルドと実行機能を使用してください。

これで、C# .NET Core MVC アプリケーションが `myapp` フォルダ内の `index.html` をホストする準備が整いました。

---

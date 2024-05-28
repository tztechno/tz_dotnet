# tz_dotnet
### 

---

ASP.NET Core の Razor Pages では、.cshtml と .cshtml.cs ファイルは密接に連携しています。この仕組みは次のようになっています。

### .cshtml ファイル

このファイルには、HTML マークアップと Razor 構文が含まれています。
@model ディレクティブで、関連する .cshtml.cs ファイルのページモデルクラスを指定します。


### .cshtml.cs ファイル

このファイルには、ページの振る舞いを制御するロジックが C# で書かれています。
ページモデルクラスは Microsoft.AspNetCore.Mvc.RazorPages.PageModel から継承しています。
このクラスには、ページ固有のプロパティ、メソッド、イベントハンドラーなどが定義されています。


### データの流れ

ページの初期処理は、.cshtml.cs ファイルの OnGet メソッドで行われます。
ここでモデルデータを設定したり、データベースからデータを取得したりできます。
モデルデータは .cshtml ファイルで参照・表示できます。
フォームの送信時は、.cshtml.cs ファイルの OnPost メソッドが呼び出されます。


### 具体例

Pages/Hello.cshtml
```
@page
@model HelloModel

<h1>@Model.Message</h1>
```

Pages/Hello.cshtml.cs
```
public class HelloModel : PageModel
{
    public string Message { get; set; } = "Hello, World!";
    
    public void OnGet()
    {
        // ページ初期処理
    }
}
```

この例では、HelloModel クラスの Message プロパティを .cshtml ファイルで参照し、画面に表示しています。



.cshtml と .cshtml.cs は、Razor Pages における View とページモデルの役割を果たしています。ビューとロジックを分離することで、メンテナンス性と拡張性が高まります。この仕組みを上手く活用することで、ASP.NET Core Web アプリケーションを柔軟に構築できます。


---

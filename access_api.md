### 
# access_api
### 

外部のAPIにアクセスする場合は、プロジェクト内に新しいクラスを作成し、そこで外部APIへのHTTPリクエストを行うロジックを実装する必要があります。
通常、次のようなステップが必要になります。

### HTTPクライアントライブラリのインストール
外部APIへのリクエストを送信するために、System.Net.Http.HttpClientクラスを利用します。NuGetから必要なパッケージをインストールします。
```
dotnet add package Microsoft.Extensions.Http
```

### HTTPクライアントの登録
Startup.csファイルのConfigureServicesメソッド内で、HTTPクライアントをDIコンテナに登録します。
```
services.AddHttpClient();
```

### 外部APIアクセスロジックの実装
新しいクラスを作成し、コンストラクタ注入でIHttpClientFactoryを受け取り、HttpClientインスタンスを生成してAPIコールを行うメソッドを実装します。
```
public class ExternalApiService 
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ExternalApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> CallExternalApiAsync(string apiUrl)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
```

### コントローラーからサービスを呼び出す
作成したサービスクラスをコントローラーでインスタンス化し、外部APIへのリクエストを送信するメソッドを呼び出します。

```
[ApiController]
[Route("[controller]")]
public class ExternalApiController : ControllerBase
{
     private readonly ExternalApiService _externalApiService;

     public ExternalApiController(ExternalApiService externalApiService)
     {
         _externalApiService = externalApiService;
     }

     [HttpGet]
     public async Task<string> Get(string apiUrl)
     {
         return await _externalApiService.CallExternalApiAsync(apiUrl);
     }
}
```

上記のように実装することで、コントローラーのアクションメソッドからサービスクラスのメソッドを呼び出し、外部APIへのHTTPリクエストを送信できるようになります。外部APIからのレスポンスは、そのままアプリケーションのレスポンスとして返されます。
外部APIの認証が必要な場合は、さらにその処理を実装する必要があります。APIのドキュメントを参照して、適切な方法で認証ヘッダーやパラメータを付加してください。

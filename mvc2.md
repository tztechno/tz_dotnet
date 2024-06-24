###
# MVC
###

---

---

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

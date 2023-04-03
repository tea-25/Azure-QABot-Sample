# Azure-QABot-Sample

## Q/Aのデータベースを環境変数で切り替えることができる。
環境変数に何も設定していないと、Question Answering が利用される。
### 開発環境で以下のパッケージをダウンロードする。
```
PS > dotnet add package Microsoft.Bot.Builder.Integration.AspNet.Core
PS > dotnet add package Azure.AI.Language.QuestionAnswering 
PS > dotnet add package Microsoft.Bot.Builder.AI.QnA;
```

### これを設定しておく(必須かどうかはわかってない)
```
PS > az webapp config appsettings set --resource-group <group-name> --name <app-name> --settings SCM_DO_BUILD_DURING_DEPLOYMENT=true
```

### デプロイ
```
PS > az webapp deploy --resource-group <group-name> --name <app-name> --src-path <zip-package-path>
```
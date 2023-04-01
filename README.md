# Azure-QABot-Sample[


## 開発環境で以下のパッケージをダウンロードする。
'''
dotnet add package Microsoft.Bot.Builder.Integration.AspNet.Core
'''
'''
dotnet add package Azure.AI.Language.QuestionAnswering 
'''
'''
dotnet add package Microsoft.Bot.Builder.AI.QnA;
'''

##これを設定しておく(必須かどうかはわかってない)
az webapp config appsettings set --resource-group <group-name> --name <app-name> --settings SCM_DO_BUILD_DURING_DEPLOYMENT=true

##デプロイ
az webapp deploy --resource-group <group-name> --name <app-name> --src-path <zip-package-path>

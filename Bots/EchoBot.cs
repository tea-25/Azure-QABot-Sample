// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with EchoBot .NET Template version v4.17.1

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
// using add
using Microsoft.Bot.Builder.AI.QnA;
using Azure.AI.Language.QuestionAnswering;
using System;
using System.Linq;

namespace EchoBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        // define
        private const int QNAMAKER_ENVIRONMENT_CODE = 0;
        private const int QA_ENVIRONMENT_CODE = 1;
        private const int ENVIRONMENT_CODE = QA_ENVIRONMENT_CODE;

        public QnAMaker EchoBotQnA { get; private set; }
        public EchoBot(QnAMakerEndpoint endpoint)
        {
            // connects to QnA Maker endpoint for each turn
            EchoBotQnA = new QnAMaker(endpoint);
        }


        // message add working
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyText = $"Echo: {turnContext.Activity.Text}";
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);

            // environment value check
            int EnvironmentCode = Int32.Parse(Environment.GetEnvironmentVariable("ENVIRONMENT_CODE"));

            // use database check
            if(QA_ENVIRONMENT_CODE == EnvironmentCode)
            {
                // use question answering
                // Config to Azure Cognitive Service for Language
                var endpoint = Environment.GetEnvironmentVariable("QA_ENDPOINT");
                var credential = Environment.GetEnvironmentVariable("QA_AUTH_KEY");
                var projectName = Environment.GetEnvironmentVariable("QA_PROJECT_NAME");
                var deploymentName = Environment.GetEnvironmentVariable("DEPLOYMENT_NAME");

                var client = new QuestionAnsweringClient(endpoint, credential);
                var project = new QuestionAnsweringProject(projectName, deploymentName);

                Response<AnswersResult> response = await client.GetAnswersAsync(turnContext.Activity.Text, project);
                if(response.Value.Answer.First().Answer.Any())
                {
                    // var replyText = response.Value.Answers.First().Answer;
                    // bot に喋ってもらう
                    // await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                    await turnContext.SendActivityAsync(MessageFactory.Text("回答は:" + replyText), cancellationToken);
                }
                else
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text("回答なし), cancellationToken);
                }
            }
            else
            {
                // QnA Maker Access
                await AccessQnAMaker(turnContext, cancellationToken);
            }
            
        }

        // welcome working
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }

        // access qna maker
        private async Task AccessQnAMaker(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var results = await EchoBotQnA.GetAnswersAsync(turnContext);
            if(results.Any())
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("回答は" + results.First().Answer), cancellationToken);
            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("回答なし"), cancellationToken);
            }
        }
    }
}

﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with EchoBot .NET Template version v4.17.1

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
// using add
using Microsoft.Bot.Builder.AI.QnA;
using System;

namespace EchoBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient().AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.MaxDepth = HttpHelper.BotMessageSerializerSettings.MaxDepth;
            });

            // Create the Bot Framework Authentication to be used with the Bot Adapter.
            services.AddSingleton<BotFrameworkAuthentication, ConfigurationBotFrameworkAuthentication>();

            // Create the Bot Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, Bots.EchoBot>();

            // QnA Maker Endpoint Add 
            services.AddSingleton(new QnAMakerEndpoint
            {
                KnowledgeBaseId = Environment.GetEnvironmentVariable("QnA_KNOWLEDGE_BASE_ID"),
                EndopointKey = Environment.GetEnvironmentVariable("QnA_AUTH_KEY"),
                Host = Environment.GetEnvironmentVariable("QnA_ENDPOINT_HOSTNAME")
            });

            // QuestionAnswering Endpoint Add
            services.AddSingleton(new QuestionAnsweringEndpoint
            {
                endpoint = Environment.GetEnvironmentVariable("QA_ENDPOINT"),
                credential = Environment.GetEnvironmentVariable("QA_AUTH_KEY"),
                projectname = Environment.GetEnvironmentVariable("QA_PROJECT_NAME"),
                deploymentName = Environment.GetEnvironmentVariable("DEPLOYMENT_NAME")
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // app.UseHttpsRedirection();
        }
    }
}

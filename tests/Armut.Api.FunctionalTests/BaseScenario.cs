using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Armut.Api.Core;
using Armut.Tests.Common.Fixtures;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Armut.Api.FunctionalTests
{
    public abstract class BaseScenario
    {
        protected BaseScenario(TestServerFixture testServerFixture)
        {
            TestServer = testServerFixture.Server;
            HttpClient = testServerFixture.CreateClient();
            ArmutContext = TestServer.Services.GetRequiredService<ArmutContext>();
            ServiceProvider = TestServer.Services;

            WaitUntilAwsResourcesReady().GetAwaiter().GetResult();
        }

        protected TestServer TestServer { get; set; }

        protected HttpClient HttpClient { get; set; }

        protected ArmutContext ArmutContext { get; }

        protected IServiceProvider ServiceProvider { get; }

        protected async Task WaitUntilAwsResourcesReady()
        {
            var amazonSimpleNotificationService = ServiceProvider.GetRequiredService<IAmazonSimpleNotificationService>();
            var amazonLambdaService = ServiceProvider.GetRequiredService<IAmazonLambda>();

            var ready = false;
            var attempt = 0;
            do
            {
                await Task.Delay(500);
                try
                {
                    ListTopicsResponse listTopicsResponse = await amazonSimpleNotificationService.ListTopicsAsync(new ListTopicsRequest());
                    ListFunctionsResponse listFunctionsResponse = await amazonLambdaService.ListFunctionsAsync(new ListFunctionsRequest());

                    if (listTopicsResponse.HttpStatusCode != HttpStatusCode.OK || listFunctionsResponse.HttpStatusCode != HttpStatusCode.OK)
                    {
                        attempt += 1;
                        continue;
                    }

                    List<Topic> topics = listTopicsResponse.Topics;
                    List<FunctionConfiguration> functionConfigurations = listFunctionsResponse.Functions;

                    if (topics.Any(topic => topic.TopicArn.EndsWith("dispatch")) && functionConfigurations.Any())
                    {
                        ready = true;
                        break;
                    }

                    attempt += 1;
                }
                catch
                {
                    attempt += 1;
                }

            } while (!ready || attempt <= 10);

            if (!ready)
            {
                throw new InvalidOperationException("The sns topic named dispatch was not found.");
            }
        }
    }
}

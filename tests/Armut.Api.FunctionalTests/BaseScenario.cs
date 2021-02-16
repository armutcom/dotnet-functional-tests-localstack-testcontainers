using System;
using System.Net.Http;
using Armut.Api.Core;
using Armut.Tests.Common.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
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
        }

        protected TestServer TestServer { get; set; }

        protected HttpClient HttpClient { get; set; }

        protected ArmutContext ArmutContext { get; }

        protected IServiceProvider ServiceProvider { get; }
    }
}

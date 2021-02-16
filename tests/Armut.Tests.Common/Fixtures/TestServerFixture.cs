using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Armut.Api;
using Armut.Api.Core;
using Armut.Tests.Common.Seeders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Armut.Tests.Common.Fixtures
{
    public class TestServerFixture : WebApplicationFactory<Startup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
            //Environment.SetEnvironmentVariable("ASPNETCORE_URLS", "https://localhost:5001;http://localhost:5000");

            var hostBuilder = base.CreateHostBuilder()
                .UseEnvironment("Testing")
                .ConfigureAppConfiguration(builder =>
                {
                    var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                    builder.AddJsonFile(configPath);
                })
                .ConfigureServices((context, collection)  =>
                {
                    ServiceProvider serviceProvider = collection.BuildServiceProvider();

                    var hostEnvironment = serviceProvider.GetRequiredService<IHostEnvironment>();
                    bool isTest = hostEnvironment.IsEnvironment("Testing");

                    if (!isTest)
                    {
                        throw new Exception("Incorrect config loaded.");
                    }

                    using IServiceScope scope = serviceProvider.CreateScope();
                    IServiceProvider scopeServiceProvider = scope.ServiceProvider;
                    var armutContext = scopeServiceProvider.GetRequiredService<ArmutContext>();
                    var amazonS3Client = scopeServiceProvider.GetRequiredService<IAmazonS3>();

                    armutContext.Database.EnsureCreated();
                    Seeder.Seed(armutContext);

                    CreateS3BucketAsync(amazonS3Client, Constants.ProfilePictureBucket)
                        .GetAwaiter()
                        .GetResult();
                });

            return hostBuilder;
        }

        private async Task CreateS3BucketAsync(IAmazonS3 amazonS3Client, string bucketName)
        {
            bool bucketExists = await amazonS3Client.DoesS3BucketExistAsync(bucketName);

            if (!bucketExists)
            {
                await amazonS3Client.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = bucketName,
                });

                await amazonS3Client.PutACLAsync(new PutACLRequest()
                {
                    CannedACL = S3CannedACL.PublicRead,
                    BucketName = bucketName
                });
            }
        }
    }
}

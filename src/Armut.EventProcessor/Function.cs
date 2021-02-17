using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Armut.Api.Core;
using Armut.Api.Core.Contracts;
using Armut.Api.Core.Installers;
using Armut.Api.Core.Models;
using Armut.Api.Core.Models.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Armut.EventProcessor
{
    public class Function
    {
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();

            HostEnvironment = new LambdaHostEnv()
            {
                ApplicationName = "Armut.EventProcessor", 
                ContentRootFileProvider = new NullFileProvider(),
                EnvironmentName = "Production"
            };

            var collection = new ServiceCollection();

            ConfigureServices(collection);

            ServiceProvider = collection.BuildServiceProvider();
        }

        private IConfiguration Configuration { get; }

        private IServiceProvider ServiceProvider { get; }

        private IHostEnvironment HostEnvironment { get; }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SNS event object and can be used 
        /// to respond to SNS messages.
        /// </summary>
        /// <param name="event"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(SNSEvent @event, ILambdaContext context)
        {
            try
            {
                foreach(SNSEvent.SNSRecord record in @event.Records)
                {
                    await ProcessRecordAsync(record, context);

                    string message = record.Sns.Message;
                    var eventService = ServiceProvider.GetRequiredService<IEventService>();

                    if (TryDeserialize(message, context, out UserCreatedEvent userCreatedEvent))
                    {
                        context.Logger.LogLine($"Saving UserCreatedEvent");
                        await eventService.SaveUserCreatedEvent(userCreatedEvent);
                    }
                    else if (TryDeserialize(message, context, out JobCreatedEvent jobCreatedEvent))
                    {
                        context.Logger.LogLine($"Saving JobCreatedEvent");
                        await eventService.SaveJobCreatedEvent(jobCreatedEvent);
                    }
                    else
                    {
                        context.Logger.LogLine($"Invalid event ${message}");
                    }
                }
            }
            catch (Exception e)
            {
                context.Logger.LogLine(e.Message);
                throw;
            }
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddAutoMapper(typeof(MappingProfile))
                .InstallServices(Configuration, HostEnvironment);
        }

        private static async Task ProcessRecordAsync(SNSEvent.SNSRecord record, ILambdaContext context)
        {
            context.Logger.LogLine($"Processed record {record.Sns.Message}");

            // TODO: Do interesting work based on the new message
            await Task.CompletedTask;
        }

        private static bool TryDeserialize<TEvent>(string message, ILambdaContext context, out TEvent @event)
        {
            try
            {
                @event = JsonSerializer.Deserialize<TEvent>(message);
            }
            catch  (Exception e)
            {
                context.Logger.LogLine(e.Message);
                @event = default(TEvent);
                return false;
            }

            return true;
        }
    }
}

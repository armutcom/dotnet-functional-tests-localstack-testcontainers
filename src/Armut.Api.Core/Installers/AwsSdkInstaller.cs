using System;
using System.Collections.Generic;
using System.Text;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Armut.Api.Core.Contracts;
using LocalStack.Client.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Armut.Api.Core.Installers
{
    public class AwsSdkInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, IHostEnvironment webHostEnvironment)
        {
            services.AddDefaultAWSOptions(GetAwsOptions(configuration));
            services.AddLocalStack(configuration);
            services.AddAwsService<IAmazonS3>();
            services.AddSingleton<ITransferUtility>(provider =>
            {
                var s3Client = provider.GetService<IAmazonS3>();
                return new TransferUtility(s3Client);
            });
        }

        private AWSOptions GetAwsOptions(IConfiguration configuration)
        {
            string awsAccessKey = configuration.GetSection("AWSAccessKey").Value;
            string awsSecretKey = configuration.GetSection("AWSSecretKey").Value;

            var awsOptions = new AWSOptions
            {
                Region = RegionEndpoint.EUCentral1,
                Credentials = new BasicAWSCredentials(awsAccessKey, awsSecretKey)
            };

            return awsOptions;
        }
    }
}

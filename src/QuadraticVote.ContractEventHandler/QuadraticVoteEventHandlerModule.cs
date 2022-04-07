using System;
using System.Net.Security;
using System.Security.Authentication;
using AElf.AElfNode.EventHandler.BackgroundJob.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuadraticVote.EntityFrameworkCore;
using RabbitMQ.Client;
using Volo.Abp;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Modularity;
using Volo.Abp.RabbitMQ;

namespace QuadraticVote.ContractEventHandler
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(QuadraticVoteEntityFrameworkModule),
        typeof(QuadraticVoteEventHandlerCoreModule),
        typeof(AbpEventBusRabbitMqModule)
    )]
    public class QuadraticVoteEventHandlerModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            context.Services.AddHostedService<QuadraticVoteHostedService>();

            Configure<AbpRabbitMqOptions>(options =>
            {
                var messageQueueConfig = configuration.GetSection("MessageQueue");
                var hostName = messageQueueConfig.GetSection("HostName").Value;

                options.Connections.Default.HostName = hostName;
                options.Connections.Default.Port = int.Parse(messageQueueConfig.GetSection("Port").Value);
                options.Connections.Default.UserName = messageQueueConfig.GetSection("UserName").Value;
                options.Connections.Default.Password = messageQueueConfig.GetSection("Password").Value;
                options.Connections.Default.Ssl = new SslOption
                {
                    Enabled = true,
                    ServerName = hostName,
                    Version = SslProtocols.Tls12,
                    AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNameMismatch |
                                             SslPolicyErrors.RemoteCertificateChainErrors
                };
                options.Connections.Default.VirtualHost = "/";
                options.Connections.Default.Uri = new Uri(messageQueueConfig.GetSection("Uri").Value);
            });

            Configure<AElfProcessorOption>(options =>
            {
                configuration.GetSection("AElfEventProcessors").Bind(options);
            });
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
        }
    }
}
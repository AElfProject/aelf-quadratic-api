using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob.ETO;
using AElf.AElfNode.EventHandler.BackgroundJob.Processors;
using AElf.AElfNode.EventHandler.BackgroundJob.Provider;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace AElf.AElfNode.EventHandler.BackgroundJob
{
    public interface ILogEventProcessor
    {
        Task ProcessTransactionAsync(int chainId, TransactionResultEto transactionResultEto);
    }

    public class LogEventProcessor : ILogEventProcessor, ITransientDependency
    {
        private readonly IAElfEventProcessorProvider _aelfEventProcessorProvider;
        private readonly ILogger<LogEventProcessor> _logger;
        private readonly IAbpLazyServiceProvider _lazyServiceProvider;
        private readonly List<IEventContextProcessor> _eventContextProcessors;
        private IObjectMapper ObjectMapper => _lazyServiceProvider.LazyGetService<IObjectMapper>();

        public LogEventProcessor(IAElfEventProcessorProvider aelfEventProcessorProvider,
            ILogger<LogEventProcessor> logger,
            IEnumerable<IEventContextProcessor> eventContextProcessors, IAbpLazyServiceProvider lazyServiceProvider)
        {
            _aelfEventProcessorProvider = aelfEventProcessorProvider;
            _logger = logger;
            _lazyServiceProvider = lazyServiceProvider;
            _eventContextProcessors = eventContextProcessors.ToList();
        }

        public async Task ProcessTransactionAsync(int chainId, TransactionResultEto txEto)
        {
            var eventContext = ObjectMapper.Map<TransactionResultEto, EventContext>(txEto);
            eventContext.ChainId = chainId;
            _eventContextProcessors.ForEach(p => p.ProcessEventContext(eventContext, txEto));
            await ProcessLogEvents(txEto.Logs, eventContext);
        }

        private async Task ProcessLogEvents(IEnumerable<LogEventEto> logs, EventContext eventContext)
        {
            foreach (var eventLog in logs)
            {
                _logger.LogInformation(
                    $"Received event log {eventLog.Name} of contract {eventLog.Address} on chain {eventContext.ChainId}");

                var processor = _aelfEventProcessorProvider.GetProcessor(eventLog, eventContext);
                if (processor == null)
                {
                    _logger.LogInformation($"Lack of Processor for event: {eventLog.Name}");
                    continue;
                }
                
                _logger.LogInformation(
                    $"Pushing aforementioned event log to processor : {processor.GetProcessorName()}.");
                eventContext.EventName = eventLog.Name;
                eventContext.EventAddress = eventLog.Address;
                await processor.HandleEventAsync(eventLog, eventContext);
            }
        }
    }
}
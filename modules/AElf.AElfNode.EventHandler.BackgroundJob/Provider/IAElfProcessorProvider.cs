using System.Collections.Generic;
using System.Linq;
using AElf.AElfNode.EventHandler.BackgroundJob.ETO;
using AElf.AElfNode.EventHandler.BackgroundJob.Options;
using AElf.AElfNode.EventHandler.BackgroundJob.Processors;
using AElf.AElfNode.EventHandler.Core;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace AElf.AElfNode.EventHandler.BackgroundJob.Provider
{
    public interface IAElfEventProcessorProvider
    {
        IAElfEventProcessor GetProcessor(LogEventEto logEventEto, EventContext eventContext);

        IList<IAElfEventProcessor> GetAllProcessors();
    }

    public class AElfEventProcessorProvider : IAElfEventProcessorProvider, ISingletonDependency
    {
        private readonly Dictionary<string, IAElfEventProcessor> _processorsDic;

        public AElfEventProcessorProvider(IOptionsSnapshot<AElfProcessorOption> optionsSnapshot,
            IEnumerable<IAElfEventProcessor> processors)
        {
            if (optionsSnapshot.Value == null)
                return;
            _processorsDic = new Dictionary<string, IAElfEventProcessor>();
            foreach (var processorOption in optionsSnapshot.Value.ProcessorKeyList)
            {
                var key = GetProcessorKey(processorOption);
                if (_processorsDic.TryGetValue(key, out var processor))
                {
                    continue;
                }

                processor = processors.SingleOrDefault(x => x.GetProcessorName() == processorOption.ProcessorName);
                if (processor == null)
                {
                    continue;
                }

                _processorsDic.TryAdd(key, processor);
            }
        }

        public IAElfEventProcessor GetProcessor(LogEventEto logEventEto, EventContext eventContext)
        {
            var processorKey = new AElfProcessorKey
            {
                ContractAddress = logEventEto.Address,
                ChainId = eventContext.ChainId,
                EventName = logEventEto.Name
            };

            return _processorsDic.TryGetValue(GetProcessorKey(processorKey), out var processor)
                ? processor
                : null;
        }

        public IList<IAElfEventProcessor> GetAllProcessors()
        {
            return _processorsDic.Values.ToList();
        }

        private static string GetProcessorKey(AElfProcessorKey processorKey)
        {
            return processorKey.ContractAddress + processorKey.ChainId + processorKey.EventName;
        }
    }
}
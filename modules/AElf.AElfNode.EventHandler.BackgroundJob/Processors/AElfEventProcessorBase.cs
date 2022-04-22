using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob.ETO;
using AElf.AElfNode.EventHandler.BackgroundJob.Helpers;
using AElf.CSharp.Core;
using Volo.Abp.DependencyInjection;

namespace AElf.AElfNode.EventHandler.BackgroundJob.Processors
{
    [ExposeServices(typeof(IAElfEventProcessor))]
    public abstract class AElfEventProcessorBase<T> : IAElfEventProcessor, ISingletonDependency
        where T : IEvent<T>, new()
    {
        private readonly string _processorName;
        protected AElfEventProcessorBase()
        {
            _processorName = this.GetType().Name;
        }

        public virtual async Task HandleEventAsync(LogEventEto logEventEto, EventContext txInfoDto)
        {
            var eventObj = AElfEventDeserializationHelper.DeserializeAElfEvent<T>(logEventEto);
            await HandleEventAsync(eventObj, txInfoDto);
        }

        public virtual string GetProcessorName()
        {
            return _processorName;
        }

        protected virtual Task HandleEventAsync(T eventDetailsEto, EventContext txInfoDto)
        {
            return Task.CompletedTask;
        }
    }
}
using System.Data;
using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob;
using AElf.AElfNode.EventHandler.TestBase.Providers;
using AElf.CSharp.Core;
using Volo.Abp.Uow;

namespace AElf.AElfNode.EventHandler.TestBase
{
    public interface IEventHandlerTestProcessor<T>  where T : IEvent<T>, new()
    {
        Task HandleEventAsync(T eventDetailsEto, EventContext txInfoDto);
    }

    public class EventHandlerTestProcessor<T> : IEventHandlerTestProcessor<T>
        where T : IEvent<T>, new()
    {
        private readonly IProcessorsActionProvider _processorsActionProvider;

        public EventHandlerTestProcessor(IProcessorsActionProvider processorsActionProvider)
        {
            _processorsActionProvider = processorsActionProvider;
        }

        [UnitOfWork(true, IsolationLevel.ReadCommitted)]
        public virtual async Task HandleEventAsync(T eventDetailsEto, EventContext txInfoDto)
        {
            var processorAction = _processorsActionProvider.GetProcessorAction<T>(typeof(T));
            await processorAction.Invoke(eventDetailsEto, txInfoDto);
        }
    }
}
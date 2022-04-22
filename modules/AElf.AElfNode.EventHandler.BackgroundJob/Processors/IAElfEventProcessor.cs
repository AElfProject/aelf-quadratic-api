using System.Threading.Tasks;
using AElf.AElfNode.EventHandler.BackgroundJob.ETO;

namespace AElf.AElfNode.EventHandler.BackgroundJob.Processors
{
    public interface IAElfEventProcessor
    {
        Task HandleEventAsync(LogEventEto eventDetailsEto, EventContext txInfoDto);
        string GetProcessorName();
    }
}
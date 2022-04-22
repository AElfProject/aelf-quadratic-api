using AElf.AElfNode.EventHandler.BackgroundJob.ETO;

namespace AElf.AElfNode.EventHandler.BackgroundJob.Processors
{
    public interface IEventContextProcessor
    {
        EventContext ProcessEventContext(EventContext context, TransactionResultEto txListEto);
    }
}
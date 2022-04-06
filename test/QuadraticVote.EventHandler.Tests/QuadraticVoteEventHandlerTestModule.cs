using AElf.AElfNode.EventHandler.TestBase;
using QuadraticVote.ContractEventHandler;
using Volo.Abp.Modularity;

namespace QuadraticVote
{
    [DependsOn(typeof(QuadraticVoteDomainTestModule),
        typeof(QuadraticVoteEventHandlerCoreModule),
        typeof(AElfEventHandlerTestBaseModule)
    )]
    public class QuadraticVoteEventHandlerTestModule : AbpModule
    {
    }
}
using QuadraticVote.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace QuadraticVote
{
    [DependsOn(
        typeof(QuadraticVoteEntityFrameworkCoreTestModule)
        )]
    public class QuadraticVoteDomainTestModule : AbpModule
    {

    }
}
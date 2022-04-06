using System;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace QuadraticVote.Domain
{
    [DependsOn(
        typeof(AbpDddDomainModule)
    )]
    public class QuadraticVoteDomainModule: AbpModule
    {
    }
}
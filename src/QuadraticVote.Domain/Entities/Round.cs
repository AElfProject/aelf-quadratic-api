using Volo.Abp.Domain.Entities;

namespace QuadraticVote.Domain.Entities
{
    public class Round : Entity<long>
    {
        public long RoundNumber { get; set; }
        public long TotalSupport { get; set; }
    }
}
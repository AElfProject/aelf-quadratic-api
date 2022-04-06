using Volo.Abp.Domain.Entities;

namespace QuadraticVote.Domain.Entities
{
    public class UserProjectInfo : Entity<long>
    {
        public long RoundNumber { get; set; }
        public string User { get; set; }
        public string ProjectId { get; set; }
        public long Vote { get; set; }
        public long Cost { get; set; }
    }
}
using Volo.Abp.Domain.Entities;

namespace QuadraticVote.Domain.Entities
{
    public class Project: Entity<long>
    {
        public Project()
        {
            
        }

        public Project(string projectId, long roundNumber)
        {
            ProjectId = projectId;
            RoundNumber = roundNumber;
            var length = projectId.Length;
            ProjectNumber = long.Parse(projectId.Substring(length - 10));
            Bid = int.Parse(projectId.Substring(0, 5));
        }
        public string ProjectId { get; set; }
        public long RoundNumber { get; set; }
        public long ProjectNumber { get; set; }
        public int Bid { get; set; }
        public long SupportArea { get; set; }
        public long Grant { get; set; }
        public long Vote { get; set; }
        public bool IsBanned { get; set; }
    }
}
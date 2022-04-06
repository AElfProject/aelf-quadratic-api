namespace QuadraticVote.Application.Dtos.QuadraticVote
{
    public class StatisticInfo
    {
        //public List<ProjectInfo> ProjectList { get; set; } // with small amount of data
        public long Round { get; set; }
        public long TotalVotes { get; set; }
        public decimal TotalSupportValue { get; set; }
        public decimal TotalVoteValue { get; set; }
    }
}
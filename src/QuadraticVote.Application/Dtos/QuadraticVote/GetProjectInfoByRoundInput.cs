namespace QuadraticVote.Application.Dtos.QuadraticVote
{
    public class GetProjectInfoByRoundInput
    {
        public long Round { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public bool IsWithBanned { get; set; }
    }
}
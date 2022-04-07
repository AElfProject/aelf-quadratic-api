using System.Collections.Generic;

namespace QuadraticVote.Application.Dtos.QuadraticVote
{
    public class PageProjectInfos
    {
        public List<ProjectInfoDto> ProjectList { get; set; }
        public int TotalCount { get; set; }
        public long Round { get; set; }
    } 
    
    public class ProjectInfoDto
    {
        public string ProjectId { get; set; }
        public long Votes { get; set; }
        public decimal VoteValue { get; set; }
        public decimal SupportValue { get; set; }
    }
}
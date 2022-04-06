using System.Collections.Generic;

namespace QuadraticVote.Application.Dtos.QuadraticVote
{
    public class UserProjects
    {
        public long Round { get; set; }
        public List<UserProjectDto> UserProjectList { get; set; }
    }

    public class UserProjectDto
    {
        public string ProjectId { get; set; }
        public long Vote { get; set; }
        public decimal Cost { get; set; }
    }
}
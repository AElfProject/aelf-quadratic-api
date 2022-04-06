using System.ComponentModel.DataAnnotations;

namespace QuadraticVote.Application.Dtos.QuadraticVote
{
    public class GetUserInfoByRoundInput
    {
        [Required]
        public string User { get; set; }
        public long Round { get; set; }
    }
}
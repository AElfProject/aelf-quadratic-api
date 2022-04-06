using System.Threading.Tasks;
using QuadraticVote.Application.Dtos.QuadraticVote;

namespace QuadraticVote.Application.Services
{
    public interface IQuadraticVoteAppService
    {
        Task<StatisticInfo> GetStatisticInfoByRoundAsync(GetStatisticInfoByRoundInput input);
        Task<PageProjectInfos> GetProjectInfoByRoundAsync(GetProjectInfoByRoundInput input);
        Task<UserProjects> GetUserInfoByRoundAsync(GetUserInfoByRoundInput input);
    }
}
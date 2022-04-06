using System.Collections.Generic;
using System.Threading.Tasks;
using QuadraticVote.Application.Dtos.QuadraticVote;
using Volo.Abp.Application.Services;

namespace QuadraticVote.Application.Services.Impl
{
    public class MockQuadraticVoteService //: ApplicationService, IQuadraticVoteAppService
    {
        public async Task<StatisticInfo> GetStatisticInfoByRoundAsync(GetStatisticInfoByRoundInput input)
        {
            return new StatisticInfo
            {
                Round = input.Round,
                TotalVotes = 100,
                TotalSupportValue = 100000,
                TotalVoteValue = 11234
            };
        }

        public async Task<PageProjectInfos> GetProjectInfoByRoundAsync(GetProjectInfoByRoundInput input)
        {
            var projectInfo = new ProjectInfoDto
            {
                ProjectId = "123333001",
                Votes = 50,
                SupportValue = 2000,
                VoteValue = 1234
            };
            return new PageProjectInfos
            {
                TotalCount = 1,
                ProjectList = new List<ProjectInfoDto>
                {
                    projectInfo
                }
            };
        }

        public async Task<UserProjects> GetUserInfoByRoundAsync(GetUserInfoByRoundInput input)
        {
            return new UserProjects
            {
                Round = 1001,
                UserProjectList = new List<UserProjectDto>
                {
                    new UserProjectDto
                    {
                        ProjectId = "123",
                        Vote = 43534534,
                        Cost = 13213.123m
                    },
                    new UserProjectDto
                    {
                        ProjectId = "456",
                        Vote = 7789,
                        Cost = 13213.123m
                    }
                }
            };
        }
    }
}
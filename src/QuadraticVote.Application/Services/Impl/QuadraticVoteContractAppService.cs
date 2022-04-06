using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AElf.Contracts.QuadraticFunding;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using QuadraticVote.Application.Dtos.QuadraticVote;
using QuadraticVote.Application.Extensions;
using QuadraticVote.Application.Options;
using QuadraticVote.Application.Provider;
using Volo.Abp.Application.Services;

namespace QuadraticVote.Application.Services.Impl
{
    public class QuadraticVoteContractAppService //: ApplicationService, IQuadraticVoteAppService
    {
        private readonly IAElfClientProvider _aelfClientProvider;
        private readonly string _quadraticAddress;

        public QuadraticVoteContractAppService(IAElfClientProvider aelfClientProvider,
            IOptionsSnapshot<ApiOption> apiOption)
        {
            _aelfClientProvider = aelfClientProvider;
            _quadraticAddress = apiOption.Value.QuadraticVoteContractAddress;
        }

        public async Task<StatisticInfo> GetStatisticInfoByRoundAsync(GetStatisticInfoByRoundInput input)
        {
            var round = await GetRoundIdAsync(input.Round);
            var rankingList = await GetProjectsAsync(round);
            var supportUnit = rankingList.Unit;
            var totalVotes = rankingList.Votes.Sum();
            var totalProjectGrants = GetAmountWithDecimal(rankingList.Grants.Sum());
            var roundInfo = await GetRoundInfoAsync(round);
            var totalSupport = roundInfo.Support;
            var totalSupportValue = GetAmountWithDecimal(supportUnit, totalSupport);
            //var projects = GetProjectsInfo(rankingList);
            return new StatisticInfo
            {
                //ProjectList = projects,
                Round = round,
                TotalVotes = totalVotes,
                TotalVoteValue = totalProjectGrants,
                TotalSupportValue = totalSupportValue
            };
        }

        public async Task<PageProjectInfos> GetProjectInfoByRoundAsync(GetProjectInfoByRoundInput input)
        {
            var roundId = await GetRoundIdAsync(input.Round);
            var projectTotalCount = await GetProjectCountByRoundIdAsync(roundId);
            var rankingList = await GetProjectsAsync(roundId, input.Page, input.Size);
            var projectList = GetProjectsInfo(rankingList);
            return new PageProjectInfos
            {
                TotalCount = projectTotalCount,
                ProjectList = projectList
            };
        }

        public Task<UserProjects> GetUserInfoByRoundAsync(GetUserInfoByRoundInput input)
        {
            throw new System.NotImplementedException();
        }

        private async Task<long> GetRoundIdAsync(long round)
        {
            if (round > 0)
            {
                return round;
            }

            var client = _aelfClientProvider.GetAElfClient();
            return (await client.QueryAsync<Int64Value>(_quadraticAddress, "GetCurrentRound", new Empty())).Value;
        }

        private async Task<RankingList> GetProjectsAsync(long roundId, int page = 0, int size = 0)
        {
            var client = _aelfClientProvider.GetAElfClient();
            if (size == 0)
            {
                return await client.QueryAsync<RankingList>(_quadraticAddress, "GetRankingList", new Int64Value
                {
                    Value = roundId
                });
            }

            return await client.QueryAsync<RankingList>(_quadraticAddress, "GetPagedRankingList",
                new GetPagedRankingListInput
                {
                    Round = roundId,
                    Page = page,
                    Size = size
                });
        }

        private async Task<RoundInfo> GetRoundInfoAsync(long roundId)
        {
            var client = _aelfClientProvider.GetAElfClient();
            return await client.QueryAsync<RoundInfo>(_quadraticAddress, "GetRoundInfo", new Int64Value
            {
                Value = roundId
            });
        }

        private decimal GetAmountWithDecimal(params long[] amounts)
        {
            return amounts.Aggregate((ret, amount) =>
            {
                ret *= amount / QuadraticVoteConstants.VoteSymbolDecimal;
                return ret;
            });
        }

        private List<ProjectInfoDto> GetProjectsInfo(RankingList rankingList)
        {
            var supportUnit = rankingList.Unit;
            var grants = rankingList.Grants;
            var votes = rankingList.Votes;
            var supports = rankingList.Support;
            return rankingList.Projects.Select((x, index) =>
                new ProjectInfoDto
                {
                    ProjectId = x,
                    SupportValue = GetAmountWithDecimal(supports[index], supportUnit),
                    VoteValue = GetAmountWithDecimal(grants[index]),
                    Votes = votes[index]
                }
            ).ToList();
        }

        private async Task<int> GetProjectCountByRoundIdAsync(long roundId)
        {
            var client = _aelfClientProvider.GetAElfClient();
            var allProjects = await client.QueryAsync<ProjectList>(_quadraticAddress, "GetAllProjects", new Int64Value
            {
                Value = roundId
            });
            return allProjects.Value.Count;
        }
    }
}
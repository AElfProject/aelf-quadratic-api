using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using QuadraticVote.Application.Dtos.QuadraticVote;
using QuadraticVote.Domain.Entities;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace QuadraticVote.Application.Services.Impl
{
    public class QuadraticVoteAppService : ApplicationService, IQuadraticVoteAppService
    {
        private readonly IRepository<Project> _projectRoundInfosRepository;
        private readonly IRepository<UserProjectInfo> _userProjectInfosRepository;
        private readonly IRepository<Round> _roundInfosRepository;

        public QuadraticVoteAppService(IRepository<Project> projectRoundInfosRepository,
            IRepository<UserProjectInfo> userProjectInfosRepository, IRepository<Round> roundInfosRepository)
        {
            _projectRoundInfosRepository = projectRoundInfosRepository;
            _userProjectInfosRepository = userProjectInfosRepository;
            _roundInfosRepository = roundInfosRepository;
        }

        public async Task<StatisticInfo> GetStatisticInfoByRoundAsync(GetStatisticInfoByRoundInput input)
        {
            var round = await GetRoundAsync(input.Round);
            var roundInfo = await _roundInfosRepository.FindAsync(r => r.RoundNumber == round);
            if (roundInfo == null)
            {
                return new StatisticInfo();
            }

            var roundProjectList =
                await _projectRoundInfosRepository.GetListAsync(x => x.RoundNumber == round && !x.IsBanned);
            return new StatisticInfo
            {
                Round = round,
                TotalVotes = roundProjectList.Sum(p => p.Vote),
                TotalSupportValue = roundInfo.TotalSupport,
                TotalVoteValue = roundProjectList.Sum(p => p.Grant)
            };
        }

        public async Task<PageProjectInfos> GetProjectInfoByRoundAsync(GetProjectInfoByRoundInput input)
        {
            var round = await GetRoundAsync(input.Round);
            var roundInfo = await _roundInfosRepository.FindAsync(r => r.RoundNumber == round);
            if (roundInfo == null)
            {
                return new PageProjectInfos();
            }

            var totalSupport = roundInfo.TotalSupport;
            List<Project> roundProjectList;
            int totalCount;
            long totalSupportArea;
            if (input.Page >= 0 && input.Size > 0)
            {
                totalCount = await _projectRoundInfosRepository.CountAsync(ProjectQuery(round, input.IsWithBanned));
                var queryable = await _projectRoundInfosRepository.GetQueryableAsync();
                roundProjectList =
                    queryable.Where(ProjectQuery(round, input.IsWithBanned)).Skip(input.Page * input.Size)
                        .Take(input.Size).ToList();
                queryable = await _projectRoundInfosRepository.GetQueryableAsync();
                totalSupportArea = queryable.Where(p => p.RoundNumber == round && !p.IsBanned).Sum(p => p.SupportArea);
            }
            else
            {
                roundProjectList =
                    await _projectRoundInfosRepository.GetListAsync(ProjectQuery(round, input.IsWithBanned));
                totalCount = roundProjectList.Count;
                totalSupportArea = roundProjectList.Where(p => !p.IsBanned).Sum(p => p.SupportArea);
            }

            var supportUnit = totalSupportArea == 0 ? 0 : (decimal)totalSupport / totalSupportArea;
            return new PageProjectInfos
            {
                Round = round,
                TotalCount = totalCount,
                ProjectList = roundProjectList.Select(p => new ProjectInfoDto
                {
                    ProjectId = p.ProjectId,
                    Votes = p.Vote,
                    VoteValue = CalculateWithDecimal(p.Grant),
                    SupportValue = p.IsBanned ? 0m : CalculateWithDecimal(supportUnit * p.SupportArea)
                }).ToList()
            };
        }

        public async Task<UserProjects> GetUserInfoByRoundAsync(GetUserInfoByRoundInput input)
        {
            var round = await GetRoundAsync(input.Round);
            var userProjectList =
                await _userProjectInfosRepository.GetListAsync(u => u.RoundNumber == round && u.User == input.User);
            return new UserProjects
            {
                Round = round,
                UserProjectList = userProjectList.Select(u => new UserProjectDto
                {
                    ProjectId = u.ProjectId,
                    Vote = u.Vote,
                    Cost = CalculateWithDecimal(u.Cost)
                }).ToList()
            };
        }

        private async Task<long> GetRoundAsync(long round)
        {
            if (round > 0)
            {
                return round;
            }

            if (await _roundInfosRepository.AnyAsync())
            {
                return await _roundInfosRepository.MaxAsync(x => x.RoundNumber);
            }

            return 0;
        }

        private Expression<Func<Project, bool>> ProjectQuery(long round, bool isWithBanned)
        {
            return p => p.RoundNumber == round && (isWithBanned || !p.IsBanned);
        }

        private decimal CalculateWithDecimal(long number)
        {
            return CalculateWithDecimal((decimal)number);
        }

        private decimal CalculateWithDecimal(decimal number)
        {
            return Decimal.Round(number / QuadraticVoteConstants.VoteSymbolDecimalScale, 8);
        }
    }
}
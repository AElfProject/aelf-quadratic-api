using System.Threading.Tasks;
using QuadraticVote.Domain.Entities;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace QuadraticVote
{
    public class QuadraticVoteTestDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Project> _projectRoundInfosRepository;
        private readonly IRepository<UserProjectInfo> _userProjectInfosRepository;
        private readonly IRepository<Round> _roundInfosRepository;

        public QuadraticVoteTestDataSeedContributor(IRepository<Project> projectRoundInfosRepository,
            IRepository<UserProjectInfo> userProjectInfosRepository, IRepository<Round> roundInfosRepository)
        {
            _projectRoundInfosRepository = projectRoundInfosRepository;
            _userProjectInfosRepository = userProjectInfosRepository;
            _roundInfosRepository = roundInfosRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await AddRoundsAsync();
            await AddProjectsAsync();
            await AddUserVotesAsync();
        }

        private async Task AddRoundsAsync()
        {
            var rounds = QuadraticVoteTestConstants.Rounds;
            var roundsSupport = QuadraticVoteTestConstants.RoundTotalSupport;
            for (int i = 0; i < rounds.Length; i++)
            {
                await _roundInfosRepository.InsertAsync(new Round
                {
                    RoundNumber = rounds[i],
                    TotalSupport = roundsSupport[i]
                });
            }
        }

        private async Task AddProjectsAsync()
        {
            var rounds = QuadraticVoteTestConstants.Rounds;
            var supportAreas = QuadraticVoteTestConstants.RoundSupportArea;
            var grants = QuadraticVoteTestConstants.RoundGrants;
            var isBaneds = QuadraticVoteTestConstants.RoundBanned;
            var projects = QuadraticVoteTestConstants.Projects;
            var votes = QuadraticVoteTestConstants.ProjectsVotes;

            for (int i = 0; i < rounds.Length; i++)
            {
                for (int j = 0; j < projects.Length; j++)
                {
                    var round = rounds[i];
                    var project = projects[j];
                    var supportArea = supportAreas[i, j];
                    var grant = grants[i, j];
                    var isBaned = isBaneds[i, j];
                    var vote = votes[i, j];
                    await _projectRoundInfosRepository.InsertAsync(new Project(project, round)
                    {
                        SupportArea = supportArea,
                        Grant = grant,
                        IsBanned = isBaned,
                        Vote = vote
                    });
                }
            }
        }

        private async Task AddUserVotesAsync()
        {
            var rounds = QuadraticVoteTestConstants.Rounds;
            var projects = QuadraticVoteTestConstants.Projects;
            var users = QuadraticVoteTestConstants.Users;
            var votes = QuadraticVoteTestConstants.UserVotes;
            var cost = QuadraticVoteTestConstants.UserCosts;
            for (int i = 0; i < rounds.Length; i++)
            {
                for (int j = 0; j < projects.Length; j++)
                {
                    for (int k = 0; k < users.Length; k++)
                    {
                        var round = rounds[i];
                        var project = projects[j];
                        var user = users[k];
                        var userVote = votes[i, j, k];
                        var userCost = cost[i, j, k];
                        await _userProjectInfosRepository.InsertAsync(new UserProjectInfo
                        {
                            RoundNumber = round,
                            ProjectId = project,
                            User = user,
                            Vote = userVote,
                            Cost = userCost
                        });
                    }
                }
            }
        }
    }
}
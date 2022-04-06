using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using QuadraticVote.Application.Dtos.QuadraticVote;
using QuadraticVote.Application.Services;
using Shouldly;
using Xunit;

namespace QuadraticVote.Application
{
    public class QuadraticVoteAppServiceTests : QuadraticVoteApplicationTestBase
    {
        private readonly IQuadraticVoteAppService _quadraticVoteAppService;

        public QuadraticVoteAppServiceTests()
        {
            _quadraticVoteAppService = GetRequiredService<IQuadraticVoteAppService>();
        }

        [Fact]
        public async Task GetStatisticInfoByRound_Should_Get_Right_Data()
        {
            var round = QuadraticVoteTestConstants.RoundOne;
            var statisticInfo = await _quadraticVoteAppService.GetStatisticInfoByRoundAsync(
                new GetStatisticInfoByRoundInput
                {
                    Round = round
                });
            statisticInfo.Round.ShouldBe(round);
            
            // Project three is banned in round one
            var roundTotalVote = QuadraticVoteTestConstants.ProjectOneRoundOneVotes +
                                 QuadraticVoteTestConstants.ProjectTwoRoundOneVotes;
            statisticInfo.TotalVotes.ShouldBe(roundTotalVote);
            var roundTotalGrant = QuadraticVoteTestConstants.ProjectOneRoundOneGrant +
                                  QuadraticVoteTestConstants.ProjectTwoRoundOneGrant;
            statisticInfo.TotalVoteValue.ShouldBe(roundTotalGrant);
            statisticInfo.TotalSupportValue.ShouldBe(QuadraticVoteTestConstants.RoundOneTotalSupport);
        }
        
        [Fact]
        public async Task GetStatisticInfoByRound_Without_Round_Should_Get_Latest_Round_Data()
        {
            var statisticInfo = await _quadraticVoteAppService.GetStatisticInfoByRoundAsync(
                new GetStatisticInfoByRoundInput());
            var targetRound = QuadraticVoteTestConstants.Rounds.Max();
            statisticInfo.Round.ShouldBe(targetRound);
            var roundTotalVote = QuadraticVoteTestConstants.ProjectOneRoundTwoVotes +
                                 QuadraticVoteTestConstants.ProjectTwoRoundTwoVotes + 
                                 QuadraticVoteTestConstants.ProjectThreeRoundTwoVotes;
            statisticInfo.TotalVotes.ShouldBe(roundTotalVote);
            var roundTotalGrant = QuadraticVoteTestConstants.ProjectOneRoundTwoGrant +
                                  QuadraticVoteTestConstants.ProjectTwoRoundTwoGrant + 
                                  QuadraticVoteTestConstants.ProjectThreeRoundTwoGrant;
            statisticInfo.TotalVoteValue.ShouldBe(roundTotalGrant);
            statisticInfo.TotalSupportValue.ShouldBe(QuadraticVoteTestConstants.RoundTwoTotalSupport);
        }

        [Fact]
        public async Task GetProjectInfoByRound_Without_Page_Should_Get_All_Pool()
        {
            var round = QuadraticVoteTestConstants.RoundOne;
            var projects = await _quadraticVoteAppService.GetProjectInfoByRoundAsync(new GetProjectInfoByRoundInput
            {
                Round = round
            });
            projects.TotalCount.ShouldBe(2);
            var projectsInRoundOne = QuadraticVoteTestConstants.Projects;
            var firstProject = projectsInRoundOne[0];
            var projectOne = projects.ProjectList.Single(x => x.ProjectId == firstProject);
            projectOne.Votes.ShouldBe(QuadraticVoteTestConstants.ProjectOneRoundOneVotes);
            var totalSupport = QuadraticVoteTestConstants.RoundOneTotalSupport;
            var voteValue = totalSupport * (decimal)QuadraticVoteTestConstants.ProjectOneRoundOneSupportArea /
                            (QuadraticVoteTestConstants.ProjectOneRoundOneSupportArea +
                             QuadraticVoteTestConstants.ProjectTwoRoundOneSupportArea);
            projectOne.SupportValue.ShouldBe(GetDataWithDecimal(voteValue));
            projectOne.VoteValue.ShouldBe(GetDataWithDecimal(QuadraticVoteTestConstants.ProjectOneRoundOneGrant));
        }

        [Fact]
        public async Task GetProjectInfoByRound_With_IsBanned_Should_Get_Right_Data()
        {
            var round = QuadraticVoteTestConstants.RoundOne;
            var projects = await _quadraticVoteAppService.GetProjectInfoByRoundAsync(new GetProjectInfoByRoundInput
            {
                Round = round,
                IsWithBanned = true
            });
            projects.TotalCount.ShouldBe(3);
            var projectsInRoundOne = QuadraticVoteTestConstants.Projects;
            var firstProject = projectsInRoundOne[0];
            var projectOne = projects.ProjectList.Single(x => x.ProjectId == firstProject);
            var totalSupport = QuadraticVoteTestConstants.RoundOneTotalSupport;
            var voteValue = totalSupport * (decimal)QuadraticVoteTestConstants.ProjectOneRoundOneSupportArea /
                            (QuadraticVoteTestConstants.ProjectOneRoundOneSupportArea +
                             QuadraticVoteTestConstants.ProjectTwoRoundOneSupportArea);
            projectOne.SupportValue.ShouldBe(GetDataWithDecimal(voteValue));
            
            var theThirdProject = projectsInRoundOne[2];
            var projectThree = projects.ProjectList.Single(x => x.ProjectId == theThirdProject);
            projectThree.SupportValue.ShouldBe(0m);
        }
        
        [Fact]
        public async Task GetProjectInfoByRound_With_Page_Should_Get_Right_Data()
        {
            var round = QuadraticVoteTestConstants.RoundOne;
            var projects = await _quadraticVoteAppService.GetProjectInfoByRoundAsync(new GetProjectInfoByRoundInput
            {
                Round = round,
                Page = 1,
                Size = 1
            });
            projects.TotalCount.ShouldBe(2);
            projects.ProjectList.Count.ShouldBe(1);
            var projectsInRoundOne = QuadraticVoteTestConstants.Projects;
            var theSecondProject = projectsInRoundOne[1];
            var projectTwo = projects.ProjectList.SingleOrDefault(x => x.ProjectId == theSecondProject);
            projectTwo.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetProjectInfoByRound_With_Page_And_Banned_Should_Get_Right_Data()
        {
            var round = QuadraticVoteTestConstants.RoundOne;
            var projects = await _quadraticVoteAppService.GetProjectInfoByRoundAsync(new GetProjectInfoByRoundInput
            {
                Round = round,
                IsWithBanned = true,
                Page = 1,
                Size = 2
            });
            projects.TotalCount.ShouldBe(3);
            projects.ProjectList.Count.ShouldBe(1);
            var projectsInRoundOne = QuadraticVoteTestConstants.Projects;
            var theThirdProject = projectsInRoundOne[2];
            var projectThree = projects.ProjectList.Single(x => x.ProjectId == theThirdProject);
            projectThree.SupportValue.ShouldBe(0m);
        }

        [Fact]
        public async Task GetProjectInfoByRound_Without_Round_Should_Get_Latest_Round_Data()
        {
            var projects = await _quadraticVoteAppService.GetProjectInfoByRoundAsync(new GetProjectInfoByRoundInput());
            projects.TotalCount.ShouldBe(3);
        }

        [Fact]
        public async Task GetUserInfoByRound_Should_Get_Right_Data()
        {
            var user = QuadraticVoteTestConstants.Ranni.ToBase58();
            var round = QuadraticVoteTestConstants.RoundOne;
            var userInfos = await _quadraticVoteAppService.GetUserInfoByRoundAsync(new GetUserInfoByRoundInput
            {
                User = user,
                Round = round
            });
            userInfos.Round.ShouldBe(round);
            userInfos.UserProjectList.Count.ShouldBe(3);
            var userProjectOneInfo =
                userInfos.UserProjectList.Single(x => x.ProjectId == QuadraticVoteTestConstants.ProjectOneId);
            userProjectOneInfo.Vote.ShouldBe(QuadraticVoteTestConstants.RanniRoundOneToProjectOneVote);
            userProjectOneInfo.Cost.ShouldBe(GetDataWithDecimal(QuadraticVoteTestConstants.RanniRoundOneToProjectOneCost));
        }
        
        [Fact]
        public async Task GetUserInfoByRound_Without_Round_Should_Get_Right_Data()
        {
            var user = QuadraticVoteTestConstants.Sun.ToBase58();
            var userInfos = await _quadraticVoteAppService.GetUserInfoByRoundAsync(new GetUserInfoByRoundInput
            {
                User = user,
            });
            userInfos.Round.ShouldBe(QuadraticVoteTestConstants.RoundTwo);
            userInfos.UserProjectList.Count.ShouldBe(3);
            var userProjectThreeInfo =
                userInfos.UserProjectList.Single(x => x.ProjectId == QuadraticVoteTestConstants.ProjectThreeId);
            userProjectThreeInfo.Vote.ShouldBe(QuadraticVoteTestConstants.SunRoundTwoToProjectThreeVote);
            userProjectThreeInfo.Cost.ShouldBe(GetDataWithDecimal(QuadraticVoteTestConstants.SunRoundTwoToProjectThreeCost));
        }

        private decimal GetDataWithDecimal(decimal data)
        {
            var denominator = (decimal)BigInteger.Pow(10, QuadraticVoteTestConstants.VoteTokenDecimal);
            return decimal.Round(data / denominator, 8);
        }
    }
}
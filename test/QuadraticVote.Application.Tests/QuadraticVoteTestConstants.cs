using AElf.ContractTestKit;
using AElf.Types;

namespace QuadraticVote
{
    public static class QuadraticVoteTestConstants
    {
        public const int VoteTokenDecimal = 8;
        public const int RoundOne = 100;
        public const long RoundOneTotalSupport = 200_00000000L;
        public const int RoundTwo = 101;
        public const long RoundTwoTotalSupport = 400_00000000L;
        public const string ProjectOneId = "001100234567890";
        public const string ProjectTwoId = "002100234567890";
        public const string ProjectThreeId = "000010129123410";
        public static Address Ranni { get; } = SampleAccount.Accounts[1].Address;
        public static Address Sun { get; } = SampleAccount.Accounts[2].Address;

        public static string[] Projects = new[]
        {
            ProjectOneId,
            ProjectTwoId,
            ProjectThreeId,
        };

        public static int[] Rounds = new[]
        {
            RoundOne,
            RoundTwo
        };

        public static long[] RoundTotalSupport = new[]
        {
            RoundOneTotalSupport,
            RoundTwoTotalSupport
        };


        public const long ProjectOneRoundOneSupportArea = 10;
        public const long ProjectTwoRoundOneSupportArea = 30;
        public const long ProjectThreeRoundOneSupportArea = 10;
        public const long ProjectOneRoundTwoSupportArea = 10;
        public const long ProjectTwoRoundTwoSupportArea = 30;
        public const long ProjectThreeRoundTwoSupportArea = 10;

        public static long[,] RoundSupportArea = new[,]
        {
            { ProjectOneRoundOneSupportArea, ProjectTwoRoundOneSupportArea, ProjectThreeRoundOneSupportArea },
            { ProjectOneRoundTwoSupportArea, ProjectTwoRoundTwoSupportArea, ProjectThreeRoundTwoSupportArea }
        };

        public const long ProjectOneRoundOneGrant = 100;
        public const long ProjectTwoRoundOneGrant = 300;
        public const long ProjectThreeRoundOneGrant = 100;

        public const long ProjectOneRoundTwoGrant = 100;
        public const long ProjectTwoRoundTwoGrant = 660;
        public const long ProjectThreeRoundTwoGrant = 11234;

        public static long[,] RoundGrants = new[,]
        {
            { ProjectOneRoundOneGrant, ProjectTwoRoundOneGrant, ProjectThreeRoundOneGrant },
            { ProjectOneRoundTwoGrant, ProjectTwoRoundTwoGrant, ProjectThreeRoundTwoGrant }
        };

        public const bool ProjectOneRoundOneIsBanned = false;
        public const bool ProjectTwoRoundOneIsBanned = false;
        public const bool ProjectThreeRoundOneIsBanned = true;

        public const bool ProjectOneRoundTwoIsBanned = false;
        public const bool ProjectTwoRoundTwoIsBanned = false;
        public const bool ProjectThreeRoundTwoIsBanned = false;

        public static bool[,] RoundBanned = new[,]
        {
            { ProjectOneRoundOneIsBanned, ProjectTwoRoundOneIsBanned, ProjectThreeRoundOneIsBanned },
            { ProjectOneRoundTwoIsBanned, ProjectTwoRoundTwoIsBanned, ProjectThreeRoundTwoIsBanned }
        };

        public const long RanniRoundOneToProjectOneVote = 132123;
        public const long RanniRoundOneToProjectTwoVote = 32145;
        public const long RanniRoundOneToProjectThreeVote = 1323;
        public const long RanniRoundTwoToProjectOneVote = 132123;
        public const long RanniRoundTwoToProjectTwoVote = 32145;
        public const long RanniRoundTwoToProjectThreeVote = 1323;

        public const long SunRoundOneToProjectOneVote = 645123;
        public const long SunRoundOneToProjectTwoVote = 542343;
        public const long SunRoundOneToProjectThreeVote = 234;
        public const long SunRoundTwoToProjectOneVote = 1000000;
        public const long SunRoundTwoToProjectTwoVote = 30000000;
        public const long SunRoundTwoToProjectThreeVote = 13211222;

        public static long ProjectOneRoundOneVotes = RanniRoundOneToProjectOneVote + SunRoundOneToProjectOneVote;
        public static long ProjectOneRoundTwoVotes = RanniRoundTwoToProjectOneVote + SunRoundTwoToProjectOneVote;

        public static long ProjectTwoRoundOneVotes = RanniRoundOneToProjectTwoVote + SunRoundOneToProjectTwoVote;
        public static long ProjectTwoRoundTwoVotes = RanniRoundTwoToProjectTwoVote + SunRoundTwoToProjectTwoVote;

        public static long ProjectThreeRoundOneVotes = RanniRoundOneToProjectThreeVote + SunRoundOneToProjectThreeVote;
        public static long ProjectThreeRoundTwoVotes = RanniRoundTwoToProjectThreeVote + SunRoundTwoToProjectThreeVote;

        public static long[,] ProjectsVotes = new long[,]
        {
            { ProjectOneRoundOneVotes, ProjectTwoRoundOneVotes, ProjectThreeRoundOneVotes },
            { ProjectOneRoundTwoVotes, ProjectTwoRoundTwoVotes, ProjectThreeRoundTwoVotes }
        };

        public const long RanniRoundOneToProjectOneCost = 132123_000000;
        public const long RanniRoundOneToProjectTwoCost = 32145_000000;
        public const long RanniRoundOneToProjectThreeCost = 1323_000000;
        public const long RanniRoundTwoToProjectOneCost = 132123_000000;
        public const long RanniRoundTwoToProjectTwoCost = 32145_000000;
        public const long RanniRoundTwoToProjectThreeCost = 1323_000000;

        public const long SunRoundOneToProjectOneCost = 645123_000000;
        public const long SunRoundOneToProjectTwoCost = 542343_000000;
        public const long SunRoundOneToProjectThreeCost = 234_000000;
        public const long SunRoundTwoToProjectOneCost = 1000000_000000;
        public const long SunRoundTwoToProjectTwoCost = 30000000_000000;
        public const long SunRoundTwoToProjectThreeCost = 13211222_000000;

        public static string[] Users = new[]
        {
            Ranni.ToBase58(),
            Sun.ToBase58()
        };

        public static long[,,] UserVotes = new long[,,]
        {
            {
                { RanniRoundOneToProjectOneVote, SunRoundOneToProjectOneVote },
                { RanniRoundOneToProjectTwoVote, SunRoundOneToProjectTwoVote },
                { RanniRoundOneToProjectThreeVote, SunRoundOneToProjectThreeVote }
            },
            {
                { RanniRoundTwoToProjectOneVote, SunRoundTwoToProjectOneVote },
                { RanniRoundTwoToProjectTwoVote, SunRoundTwoToProjectTwoVote },
                { RanniRoundTwoToProjectThreeVote, SunRoundTwoToProjectThreeVote }
            }
        };

        public static long[,,] UserCosts = new long[,,]
        {
            {
                { RanniRoundOneToProjectOneCost, SunRoundOneToProjectOneCost },
                { RanniRoundOneToProjectTwoCost, SunRoundOneToProjectTwoCost },
                { RanniRoundOneToProjectThreeCost, SunRoundOneToProjectThreeCost }
            },
            {
                { RanniRoundTwoToProjectOneCost, SunRoundTwoToProjectOneCost },
                { RanniRoundTwoToProjectTwoCost, SunRoundTwoToProjectTwoCost },
                { RanniRoundTwoToProjectThreeCost, SunRoundTwoToProjectThreeCost }
            }
        };
    }
}
using AElf.ContractTestKit;
using AElf.Types;

namespace QuadraticVote
{
    public static class QuadraticVoteConstants
    {
        public const string ProjectOneId = "001100234567890";
        public const int ProjectOneBid = 110;
        public const long ProjectOneNum = 234567890;
        public static Address Radahn { get; } = SampleAccount.Accounts[0].Address;
    }
}
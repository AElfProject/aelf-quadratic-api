using System.Numerics;

namespace QuadraticVote.Application
{
    public class QuadraticVoteConstants
    {
        public const string ExamplePrivateKey = "09da44778f8db2e602fb484334f37df19e221c84c4582ce5b7770ccfbc3ddbef";
        public const string ExampleAddress = "2bWwpsN9WSc4iKJPHYL4EZX3nfxVY7XLadecnNMar1GdSb4hJz";
        public const int VoteSymbolDecimal = 8;
        public static long VoteSymbolDecimalScale { get; } = (long)BigInteger.Pow(10, VoteSymbolDecimal);
    }
}
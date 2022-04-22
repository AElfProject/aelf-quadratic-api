using System.Linq;
using AElf.AElfNode.EventHandler.BackgroundJob.ETO;
using AElf.AElfNode.EventHandler.BackgroundJob.Extensions;
using AElf.CSharp.Core;
using AElf.Types;
using Google.Protobuf;

namespace AElf.AElfNode.EventHandler.BackgroundJob.Helpers
{
    public static class AElfEventDeserializationHelper
    {
        public static T DeserializeAElfEvent<T>(LogEventEto logEventEto) where T : IEvent<T>, new()
        {
            var logEvent = new LogEvent
            {
                Indexed = {logEventEto.Indexed.Select(ByteString.FromBase64)},
                NonIndexed = ByteString.FromBase64(logEventEto.NonIndexed)
            };
            var message = new T();
            message.MergeFrom(logEvent);
            return message;
        }
    }
}
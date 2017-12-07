using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Borg.Infra.Messaging
{
    public class DefaultBroadcaster : BroadcasterBase
    {
        public DefaultBroadcaster(ILoggerFactory loggerFactory, IEnumerable<IMessagePublisher> publishers) : base(
            loggerFactory, publishers)
        {
        }
    }
}
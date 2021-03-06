using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Zindagi.SeedWork;

namespace Zindagi.Infra.Behaviors
{
    [DebuggerStepThrough]
    public class LoggingPreProcessor<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<LoggingPreProcessor<TRequest>> _logger;

        public LoggingPreProcessor(ILogger<LoggingPreProcessor<TRequest>> logger, ICurrentUser currentUser)
        {
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = await _currentUser.GetOpenIdUser();

            if (userId.IsSuccess)
            {
                _logger.LogInformation("Request: {Name} {@UserId} {@UserName} {@Request}",
                                       requestName, userId.Value.GetPersistenceKey(), userId.Value.EmailAddress, request);
            }
            else
                _logger.LogInformation("Request: {Name} {@Request}", requestName, request);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JSar.Membership.Domain.Events;
using MediatR;
using Serilog;

namespace JSar.Membership.Messages.Logging
{
    public class MediatrLoggingDecorator : IMediator
    {
        private readonly IMediator _inner;
        private readonly ILogger _logger;

        public MediatrLoggingDecorator(IMediator inner, ILogger logger)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger.Verbose("{0:l} class constructed", nameof(MediatrLoggingDecorator));
        }

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
        {
            try
            {
                string messageId = notification.GetType().GetProperty("EventId").GetValue(notification).ToString();
                _logger.Debug("Publishing EVENT: {0:l}, MID: {1:l}, Type: {2:l}", notification.GetType().Name, messageId, notification.GetType().FullName);
            }
            catch
            {
            }

            await _inner.Publish(notification, cancellationToken);
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.Verbose("Calling Mediator.Send<{0:l}>", request.GetType().Name);

            return 
                await _inner.Send<TResponse>(request, cancellationToken);
        }

        public async Task Send(IRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.Verbose("Calling Mediator.Send<{0:l}>", request.GetType().Name);

            await _inner.Send(request, cancellationToken);
        }
    }
}

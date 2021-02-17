using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Armut.Api.Core.Contracts;
using Armut.Api.Core.Entities;
using Armut.Api.Core.Models.Events;

namespace Armut.Api.Core.Services
{
    public class EventService : IEventService
    {
        private readonly ArmutContext _armutContext;
        private readonly IAmazonSimpleNotificationService _amazonSimpleNotificationService;

        public EventService(ArmutContext armutContext, IAmazonSimpleNotificationService amazonSimpleNotificationService)
        {
            _armutContext = armutContext;
            _amazonSimpleNotificationService = amazonSimpleNotificationService;
        }

        public async Task SaveUserCreatedEvent(UserCreatedEvent userCreatedEvent, CancellationToken token = default)
        {
            var eventEntity = new EventEntity()
            {
                Sender = userCreatedEvent.Sender,
                EventType = nameof(UserCreatedEvent),
                EventRelationId = userCreatedEvent.Payload.Id,
                Message = JsonSerializer.Serialize(userCreatedEvent.Payload),
                CreateDate = DateTime.Now
            };

            await _armutContext.AddAsync(eventEntity, token);
            await _armutContext.SaveChangesAsync(token);
        }

        public async Task SaveJobCreatedEvent(JobCreatedEvent jobCreatedEvent, CancellationToken token = default)
        {
            var eventEntity = new EventEntity()
            {
                Sender = jobCreatedEvent.Sender,
                EventType = nameof(JobCreatedEvent),
                EventRelationId = jobCreatedEvent.Payload.Id,
                Message = JsonSerializer.Serialize(jobCreatedEvent.Payload),
                CreateDate = DateTime.Now
            };

            await _armutContext.AddAsync(eventEntity, token);
            await _armutContext.SaveChangesAsync(token);
        }

        public Task<PublishResponse> PublishEvent<TEvent>(TEvent @event, string topicArn, CancellationToken token = default) where TEvent: IEvent
        {
            var request = new PublishRequest
            {
                Message = JsonSerializer.Serialize(@event),
                TopicArn = topicArn,
            };

            return _amazonSimpleNotificationService.PublishAsync(request, token);
        }
    }
}

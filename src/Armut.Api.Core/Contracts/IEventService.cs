using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService.Model;
using Armut.Api.Core.Models.Events;

namespace Armut.Api.Core.Contracts
{
    public interface IEventService : IService
    {
        Task SaveUserCreatedEvent(UserCreatedEvent userCreatedEvent, CancellationToken token = default);
        Task SaveJobCreatedEvent(JobCreatedEvent jobCreatedEvent, CancellationToken token = default);
        Task<PublishResponse> PublishEvent<TEvent>(TEvent @event, string topicArn, CancellationToken token = default) where TEvent: IEvent;
    }
}
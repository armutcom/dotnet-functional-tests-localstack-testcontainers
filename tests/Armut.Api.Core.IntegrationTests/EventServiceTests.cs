using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Armut.Api.Core.Entities;
using Armut.Api.Core.IntegrationTests.CollectionDefinitions;
using Armut.Api.Core.Models;
using Armut.Api.Core.Models.Events;
using Armut.Tests.Common.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Armut.Api.Core.IntegrationTests
{
    [Collection(nameof(IntegrationTestCollection))]
    public class EventServiceTests : BaseTest
    {
        public EventServiceTests(IntegrationTestFixture integrationTestFixture) 
            : base(integrationTestFixture)
        {
        }


        [Fact]
        public async Task SaveUserCreatedEvent_Should_Create_An_Event_Record_In_Database()
        {
            var userCreatedEvent = new UserCreatedEvent()
            {
                Sender = nameof(EventServiceTests),
                Payload = new UserModel()
                {
                    Id = 23,
                    FirstName = "Deniz",
                    LastName = "İrgin",
                    Email = "den@armut.com",
                    ProfilePictureUrl = Guid.NewGuid().ToString()
                },
                CreateDate = DateTime.Now
            };

            await EventService.SaveUserCreatedEvent(userCreatedEvent);

            EventEntity eventEntity = await ArmutContext.EventEntities.SingleOrDefaultAsync(entity =>
                entity.EventType == nameof(UserCreatedEvent) && entity.EventRelationId == userCreatedEvent.Payload.Id);

            Assert.NotNull(eventEntity);

            UserModel userModel = JsonSerializer.Deserialize<UserModel>(eventEntity.Message);

            Assert.NotNull(userModel);
        }
    }
}

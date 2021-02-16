using System.Collections.Generic;
using System.Net.Mail;
using Armut.Api.Core;
using Armut.Api.Core.Entities;
using AutoFixture;

namespace Armut.Tests.Common.Seeders
{
    public class UserSeeder : ISeeder
    {
        public void Seed(ArmutContext context)
        {
            var fixture = new Fixture();
            fixture
                .Customize<UserEntity>(c => c
                .With(x => x.Email, fixture.Create<MailAddress>().Address));

            fixture
                .Customize<UserEntity>(c => c
                    .With(x => x.Id, default(int)));
                

            IEnumerable<UserEntity> userEntities = fixture.CreateMany<UserEntity>(20);

            context.AddRange(userEntities);
            context.SaveChanges();
        }
    }
}

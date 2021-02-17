using System;
using System.Threading.Tasks;
using Armut.Api.Core.Entities;
using Armut.Api.Core.IntegrationTests.CollectionDefinitions;
using Armut.Api.Core.Models;
using Armut.Tests.Common.Fixtures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Armut.Api.Core.IntegrationTests
{
    [Collection(nameof(IntegrationTestCollection))]
    public class UserServiceTests : BaseTest
    {
        public UserServiceTests(IntegrationTestFixture integrationTestFixture) 
            : base(integrationTestFixture)
        {
        }

        [Fact]
        public async Task AddUser_Should_Throw_An_Exception_If_Given_AddUserModel_Is_Not_Valid()
        {
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => await UserService.AddUser(new AddUserModel()));
        }

        [Fact]
        public async Task AddUser_Should_Create_A_Record_In_Database()
        {
            var addUserModel = new AddUserModel()
            {
                FirstName = "Deniz",
                LastName = "İrgin",
                Email = "den@armut.com",
                ProfilePictureUrl = "sadasd"
            };

            UserModel userModel = await UserService.AddUser(addUserModel);

            UserEntity userEntity = await ArmutContext.Users.SingleOrDefaultAsync(entity => entity.Id == userModel.Id);

            Assert.NotNull(userEntity);
            Assert.Equal(userModel.Id, userEntity.Id);
            Assert.Equal(userModel.FirstName, userEntity.FirstName);
            Assert.Equal(userModel.LastName, userEntity.LastName);
            Assert.Equal(userModel.Email, userEntity.Email);
            Assert.Equal(userModel.ProfilePictureUrl, userEntity.ProfilePictureUrl); 
        }

        [Fact]
        public async Task GetUserById_Should_Read_A_Record_From_Database()
        {
            var userEntity = new UserEntity()
            {
                FirstName = "Fatma",
                LastName = "Tanrısevdi",
                Email = "fat@armut.com",
                CreateDate = DateTime.Now,
                ProfilePictureUrl = "sadas"
            };

            await ArmutContext.AddAsync(userEntity);
            await ArmutContext.SaveChangesAsync();

            UserModel userById = await UserService.GetUserById(userEntity.Id);

            Assert.NotNull(userById);
            Assert.Equal(userEntity.Id, userById.Id);
            Assert.Equal(userEntity.FirstName, userById.FirstName);
            Assert.Equal(userEntity.LastName, userById.LastName);
            Assert.Equal(userEntity.Email, userById.Email);
            Assert.Equal(userEntity.ProfilePictureUrl, userById.ProfilePictureUrl); 
        }
    }
}

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Armut.Api.Core.Entities;
using Armut.Api.Core.Models;
using Armut.Api.FunctionalTests.CollectionDefinitions;
using Armut.Api.FunctionalTests.Extensions;
using Armut.Api.FunctionalTests.Routes;
using Armut.Tests.Common.Fixtures;
using Armut.Tests.Common.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Armut.Api.FunctionalTests
{
    [Collection(nameof(ApiTestCollection))]
    public class UserScenario : BaseScenario
    {
        public UserScenario(TestServerFixture testServerFixture) 
            : base(testServerFixture)
        {
        }

        [Fact]
        public async Task AddUser_Should_Return_201_And_UserModel()
        {
            var addUserModel = new AddUserViewModel()
            {
                FirstName = "Deniz",
                LastName = "Özgen",
                Email = "denolk@armut.com",
                ProfilePictureBase64 = ImageHelper.ImageSample1
            };

            HttpResponseMessage httpResponseMessage = await HttpClient.PostAsync(UserRoots.Root, addUserModel);

            Assert.Equal(HttpStatusCode.Created, httpResponseMessage.StatusCode);

            var userModel = await httpResponseMessage.Content.GetAsync<UserModel>();

            Assert.NotNull(userModel);
            Assert.Equal(addUserModel.FirstName, userModel.FirstName);
            Assert.Equal(addUserModel.LastName, userModel.LastName);
            Assert.Equal(addUserModel.Email, userModel.Email);
        }

        [Fact]
        public async Task AddUser_Should_Create_An_UserCreatedEvent_In_Database()
        {
            var addUserModel = new AddUserViewModel()
            {
                FirstName = "Ezgi",
                LastName = "Peker",
                Email = "ezg@armut.com",
                ProfilePictureBase64 = ImageHelper.ImageSample1
            };

            HttpResponseMessage httpResponseMessage = await HttpClient.PostAsync(UserRoots.Root, addUserModel);

            Assert.Equal(HttpStatusCode.Created, httpResponseMessage.StatusCode);

            var userModel = await httpResponseMessage.Content.GetAsync<UserModel>();
            Assert.NotNull(userModel);

            await Task.Delay(15000);

            EventEntity eventEntity = await ArmutContext.EventEntities.SingleOrDefaultAsync(entity => entity.EventRelationId == userModel.Id);
            Assert.NotNull(eventEntity);
        }

        [Fact]
        public async Task AddUser_Should_Save_A_Profile_Picture_To_A_Bucket()
        {
            var addUserModel = new AddUserViewModel()
            {
                FirstName = "Azmi",
                LastName = "Mengü",
                Email = "azn@armut.com",
                ProfilePictureBase64 = ImageHelper.ImageSample1
            };

            HttpResponseMessage httpResponseMessage = await HttpClient.PostAsync(UserRoots.Root, addUserModel);

            Assert.Equal(HttpStatusCode.Created, httpResponseMessage.StatusCode);

            var userModel = await httpResponseMessage.Content.GetAsync<UserModel>();
            string pictureUrl = $"http://localhost:4566/profile-pictures/{userModel.ProfilePictureUrl}";

            var amazonS3 = ServiceProvider.GetRequiredService<IAmazonS3>();

            GetObjectResponse getObjectResponse = await amazonS3.GetObjectAsync(Constants.ProfilePictureBucket, userModel.ProfilePictureUrl);

            Assert.Equal(HttpStatusCode.OK, getObjectResponse.HttpStatusCode);
            Assert.Equal(userModel.ProfilePictureUrl, getObjectResponse.Key);
        }

        [Fact]
        public async Task AddUser_Should_Return_400_If_AddUserModel_Is_Invalid()
        {
            HttpResponseMessage httpResponseMessage = await HttpClient.PostAsync(UserRoots.Root, new AddUserViewModel());

            string json = await httpResponseMessage.Content.ReadAsStringAsync();

            Assert.NotNull(json);
            Assert.NotEmpty(json);
            Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
        }

        [Fact]
        public async Task GetUser_Should_Return_200_And_UserModel()
        {
            UserEntity userEntity = await ArmutContext.Users.FirstAsync();

            HttpResponseMessage httpResponseMessage = await HttpClient.GetAsync(UserRoots.GetUser(userEntity.Id));

            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);

            var userModel = await httpResponseMessage.Content.GetAsync<UserModel>();

            Assert.NotNull(userModel);
            Assert.Equal(userEntity.Id, userModel.Id);
            Assert.Equal(userEntity.FirstName, userModel.FirstName);
            Assert.Equal(userEntity.LastName, userModel.LastName);
            Assert.Equal(userEntity.Email, userModel.Email);
            Assert.Equal(userEntity.ProfilePictureUrl, userModel.ProfilePictureUrl);
        }
    }
}

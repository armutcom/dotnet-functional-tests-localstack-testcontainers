using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SimpleNotificationService.Model;
using Microsoft.AspNetCore.Mvc;
using Armut.Api.Core.Contracts;
using Armut.Api.Core.Models;
using Armut.Api.Core.Models.Events;
using Armut.Api.Options;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Armut.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEventService _eventService;
        private readonly ArmutEventOptions _armutEventOptions;
        private readonly IAmazonS3 _amazonS3;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IEventService eventService, IOptions<ArmutEventOptions> eventOptions, IAmazonS3 amazonS3, IMapper mapper)
        {
            _userService = userService;
            _eventService = eventService;
            _armutEventOptions = new ArmutEventOptions() {SnsTopic = "arn:aws:sns:eu-central-1:000000000000:dispatch"};
            _amazonS3 = amazonS3;
            _mapper = mapper;
        }


        [HttpPost]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUser(AddUserViewModel addUserViewModel, CancellationToken cancellationToken)
        {
            byte[] bytes = Convert.FromBase64String(addUserViewModel.ProfilePictureBase64);

            var imgNameWithoutExtension = Guid.NewGuid().ToString();
            var imgName = $"{imgNameWithoutExtension}.jpeg";

            await using (var ms = new MemoryStream(bytes))
            {
                var request = new PutObjectRequest()
                {
                    BucketName = Constants.ProfilePictureBucket,
                    Key = imgName,
                    InputStream = ms
                };

                PutObjectResponse amazonS3Response = await _amazonS3.PutObjectAsync(request, cancellationToken);

                if (amazonS3Response.HttpStatusCode != HttpStatusCode.OK)
                {
                    return BadRequest();
                }
            }


            var addUserModel = _mapper.Map<AddUserModel>(addUserViewModel);
            addUserModel.ProfilePictureUrl = imgName;

            UserModel userModel = await _userService.AddUser(addUserModel, cancellationToken);

            PublishResponse publishResponse = await _eventService.PublishEvent(new UserCreatedEvent()
            {
                Payload = userModel, 
                CreateDate = DateTime.Now,
                Sender = $"Armut.Api-{nameof(UserController)}.{nameof(AddUser)}"
            }, _armutEventOptions.SnsTopic, cancellationToken);

            return CreatedAtAction("GetUser", new {user_id = userModel.Id}, userModel);
        }

        [HttpGet]
        [Route("{user_id}")]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUser([FromRoute(Name = "user_id")]int userId, CancellationToken cancellationToken)
        {
            UserModel userModel = await _userService.GetUserById(userId, cancellationToken);

            return Ok(userModel);
        }
    }
}

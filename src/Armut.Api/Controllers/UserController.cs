using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Armut.Api.Core.Contracts;
using Armut.Api.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Armut.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddUser(AddUserModel addUserModel)
        {
            UserModel userModel = await _userService.AddUser(addUserModel);

            return CreatedAtAction("GetUser", userModel);
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

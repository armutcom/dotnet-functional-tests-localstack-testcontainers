using System.Threading;
using System.Threading.Tasks;
using Armut.Api.Core.Contracts;
using Armut.Api.Core.Models;

namespace Armut.Api.Core.Services
{
    public class UserService : IUserService
    {
        private readonly ArmutContext _armutContext;

        public UserService(ArmutContext armutContext)
        {
            _armutContext = armutContext;
        }

        public Task<UserModel> AddUser(AddUserModel addUserModel, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserModel> GetUserById(int userId, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}

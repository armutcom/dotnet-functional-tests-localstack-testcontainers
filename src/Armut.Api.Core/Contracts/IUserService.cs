using System.Threading;
using System.Threading.Tasks;
using Armut.Api.Core.Models;

namespace Armut.Api.Core.Contracts
{
    public interface IUserService : IService
    {
        Task<UserModel> AddUser(AddUserModel addUserModel, CancellationToken token = default);

        Task<UserModel> GetUserById(int userId, CancellationToken cancellationToken = default);
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Armut.Api.Core.Components;
using Armut.Api.Core.Contracts;
using Armut.Api.Core.Entities;
using Armut.Api.Core.Exceptions;
using Armut.Api.Core.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Armut.Api.Core.Services
{
    public class UserService : IUserService
    {
        private readonly ArmutContext _armutContext;
        private readonly IModelValidator _modelValidator;
        private readonly IMapper _mapper;

        public UserService(ArmutContext armutContext, IModelValidator modelValidator, IMapper mapper)
        {
            _armutContext = armutContext;
            _modelValidator = modelValidator;
            _mapper = mapper;
        }

        public async Task<UserModel> AddUser(AddUserModel addUserModel, CancellationToken token = default)
        {
            await _modelValidator.ValidateAndThrowAsync(addUserModel, token);

            bool userExists = await _armutContext.Users.AnyAsync(entity => entity.Email == addUserModel.Email, token);

            if (userExists)
            {
                throw new UserExistsException($"{addUserModel.Email}");
            }

            var userEntity = _mapper.Map<UserEntity>(addUserModel);
            userEntity.CreateDate = DateTime.Now;

            await _armutContext.AddAsync(userEntity, token);
            await _armutContext.SaveChangesAsync(token);

            var userModel = _mapper.Map<UserModel>(userEntity);

            return userModel;
        }

        public Task<UserModel> GetUserById(int userId, CancellationToken cancellationToken = default)
        {
            Contract.Requires<ParameterRequiredException>(userId > 0, nameof(userId));

            return _armutContext.Users
                .FirstOrDefaultAsync(entity => entity.Id == userId, cancellationToken)
                .ContinueWith(task => _mapper.Map<UserModel>(task.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}

using System.Threading;
using System.Threading.Tasks;

namespace Armut.Api.Core.Contracts
{
    public interface IModelValidator
    {
        Task ValidateAndThrowAsync<TModel>(TModel model, CancellationToken token = default);
    }
}

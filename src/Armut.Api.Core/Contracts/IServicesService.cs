using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Armut.Api.Core.Models;

namespace Armut.Api.Core.Contracts
{
    public interface IServicesService : IService
    {
        Task<IEnumerable<ServiceModel>> GetServices(CancellationToken token = default);
    }
}
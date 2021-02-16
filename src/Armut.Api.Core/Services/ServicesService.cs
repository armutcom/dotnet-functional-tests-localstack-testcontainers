using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Armut.Api.Core.Contracts;
using Armut.Api.Core.Models;

namespace Armut.Api.Core.Services
{
    public class ServicesService : IServicesService
    {
        private readonly ArmutContext _armutContext;

        public ServicesService(ArmutContext armutContext)
        {
            _armutContext = armutContext;
        }

        public Task<IEnumerable<ServiceModel>> GetServices(CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using Armut.Api.Core.Contracts;
using Armut.Api.Core.Models;

namespace Armut.Api.Core.Services
{
    public class JobService : IJobService
    {
        private readonly ArmutContext _armutContext;

        public JobService(ArmutContext armutContext)
        {
            _armutContext = armutContext;
        }

        public Task<JobModel> AddJob(AddJobModel addJobModel, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<JobModel> GetJobById(int jobId, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
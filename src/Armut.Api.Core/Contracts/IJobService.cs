using System.Threading;
using System.Threading.Tasks;
using Armut.Api.Core.Models;

namespace Armut.Api.Core.Contracts
{
    public interface IJobService : IService
    {
        Task<JobModel> AddJob(AddJobModel addJobModel, CancellationToken token = default);

        Task<JobModel> GetJobById(int jobId, CancellationToken cancellationToken);
    }
}
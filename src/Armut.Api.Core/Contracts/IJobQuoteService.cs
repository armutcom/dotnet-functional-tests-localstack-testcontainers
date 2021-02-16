using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Armut.Api.Core.Models;

namespace Armut.Api.Core.Contracts
{
    public interface IJobQuoteService : IService
    {
        Task<JobQuoteModel> AddJobQuote(AddJobQuoteModel addJobQuoteModel, CancellationToken token = default);
        Task<IEnumerable<JobQuoteModel>> GetJobQuotes(int jobId, CancellationToken token = default);
    }
}
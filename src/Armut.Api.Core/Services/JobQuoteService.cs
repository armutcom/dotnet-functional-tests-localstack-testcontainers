using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Armut.Api.Core.Contracts;
using Armut.Api.Core.Models;

namespace Armut.Api.Core.Services
{
    public class JobQuoteService : IJobQuoteService
    {
        private readonly ArmutContext _armutContext;

        public JobQuoteService(ArmutContext armutContext)
        {
            _armutContext = armutContext;
        }

        public Task<JobQuoteModel> AddJobQuote(AddJobQuoteModel addJobQuoteModel, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<JobQuoteModel>> GetJobQuotes(int jobId, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
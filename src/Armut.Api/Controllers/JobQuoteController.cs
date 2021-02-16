using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Armut.Api.Core.Contracts;
using Armut.Api.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Armut.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobQuoteController : ControllerBase
    {
        private readonly IJobQuoteService _jobQuoteService;
        private readonly IMapper _mapper;

        public JobQuoteController(IJobQuoteService jobQuoteService, IMapper mapper)
        {
            _jobQuoteService = jobQuoteService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("{job_id}")]
        [ProducesResponseType(typeof(JobQuoteModel), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddJobQuote(
            [FromRoute(Name = "job_id")] int jobId,
            AddJobQuoteViewModel addJobQuoteViewModel, 
            CancellationToken token)
        {
            if (jobId <= 0)
            {
                return BadRequest($"{nameof(jobId)} is required");
            }

            var addJobQuoteModel = _mapper.Map<AddJobQuoteModel>(addJobQuoteViewModel);
            addJobQuoteModel.JobId = jobId;

            JobQuoteModel jobQuoteModel = await _jobQuoteService.AddJobQuote(addJobQuoteModel, token);

            return CreatedAtAction("GetJobQuotes", jobQuoteModel);
        }

        [HttpGet]
        [Route("{job_id}")]
        [ProducesResponseType(typeof(IEnumerable<JobQuoteModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetJobQuotes([FromRoute(Name = "job_id")]int jobId, CancellationToken cancellationToken)
        {
            IEnumerable<JobQuoteModel> jobQuoteModels = await _jobQuoteService.GetJobQuotes(jobId, cancellationToken);

            return Ok(jobQuoteModels);
        }
    }
}

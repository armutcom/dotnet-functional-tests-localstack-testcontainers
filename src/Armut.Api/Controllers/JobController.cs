using System.Threading;
using System.Threading.Tasks;
using Armut.Api.Core.Contracts;
using Armut.Api.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Armut.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }


        [HttpPost]
        [ProducesResponseType(typeof(JobModel), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddJob(AddJobModel addJobModel, CancellationToken token)
        {
            JobModel jobModel = await _jobService.AddJob(addJobModel, token);

            return CreatedAtAction("GetJob", jobModel);
        }

        [HttpGet]
        [Route("{job_id}")]
        [ProducesResponseType(typeof(JobModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetJob([FromRoute(Name = "job_id")]int jobId, CancellationToken cancellationToken)
        {
            JobModel userModel = await _jobService.GetJobById(jobId, cancellationToken);

            return Ok(userModel);
        }
    }
}

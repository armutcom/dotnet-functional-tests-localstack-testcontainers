using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Armut.Api.Core.Contracts;
using Armut.Api.Core.Models;
using Microsoft.AspNetCore.Http;

namespace Armut.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;

        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ServiceModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetServices(CancellationToken token)
        {
            IEnumerable<ServiceModel> serviceModels = await _servicesService.GetServices(token);

            return Ok(serviceModels);
        }
    }
}

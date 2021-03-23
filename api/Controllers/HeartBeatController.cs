namespace api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using api.Services.HeartBeat;
    using api.App.Models.Request;
    using System.Collections.Generic;

    [Route("api/[controller]")]
    [ApiController]
    public class HeartBeatController : ControllerBase
    {

        private readonly IHeartBeatService _service;

        public HeartBeatController(IHeartBeatService service)
        {
            _service = service;
        }

        [HttpPost("reportAlive")]
        public Task ReportAlive(ReportAliveRequest request)
        {
            return _service.ReportAliveAsync(request.GroupId, request.Id);
        }

        [HttpPost("getAlive")]
        public Task<IEnumerable<string>> GetAlive(GetAliveWithinGroupRequest request)
        {
            return _service.GetAliveMembersAsync(request.GroupId);
        }
    }
}
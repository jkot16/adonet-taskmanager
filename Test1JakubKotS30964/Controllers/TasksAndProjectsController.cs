using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Test1JakubKotS30964.Contracts.Responses;
using Test1JakubKotS30964.Services;

namespace Test1JakubKotS30964.Controllers
{
    [ApiController]
    [Route("api")]
    public class TasksAndProjectsController : ControllerBase
    {
        private readonly IWorkService _svc;
        public TasksAndProjectsController(IWorkService svc) => _svc = svc;

        // GET api/tasks/{id}
        [HttpGet("tasks/{id}")]
        public async Task<IActionResult> GetMemberTasks(int id, CancellationToken cancellationToken)
        {
            if (!await _svc.TeamMemberExists(id, cancellationToken))
                return NotFound(new ErrorResponse { Error = $"Team member with id {id} doesnt exits" });

            var dto = await _svc.GetTeamMemberTasks(id, cancellationToken);
            return Ok(dto);
        }

        // DELETE api/tasks/{id}
        [HttpDelete("tasks/{id}")]
        public async Task<IActionResult> DeleteProject(int id, CancellationToken ct)
        {
            if (!await _svc.ProjectExists(id, ct))
                return NotFound(new ErrorResponse { Error = $"Project with id {id} doesnt exists" });

            await _svc.DeleteProject(id, ct);
            return NoContent();
        }
    }
}
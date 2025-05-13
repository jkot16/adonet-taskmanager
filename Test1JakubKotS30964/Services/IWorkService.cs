using System.Threading;
using System.Threading.Tasks;
using Test1JakubKotS30964.Contracts.Responses;

namespace Test1JakubKotS30964.Services
{
    public interface IWorkService
    {
        // /api/tasks/{id}
        Task<bool> TeamMemberExists (int memberId, CancellationToken ct);
        Task<GetTeamMemberResponse> GetTeamMemberTasks  (int memberId, CancellationToken ct);

        // /api/tasks/{id}
        Task<bool> ProjectExists (int projectId, CancellationToken ct);
        Task  DeleteProject (int projectId, CancellationToken ct);
    }
}
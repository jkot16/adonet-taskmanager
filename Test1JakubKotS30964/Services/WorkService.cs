using System.Data;
using Microsoft.Data.SqlClient;
using Test1JakubKotS30964.Contracts.Responses;

namespace Test1JakubKotS30964.Services
{
    public class WorkService : IWorkService
    {
        private readonly string _cs;
        public WorkService(IConfiguration cfg) => _cs = cfg.GetConnectionString("DefaultConnection");

        public async Task<bool> TeamMemberExists(int memberId, CancellationToken ct)
        {
            const string sql = "SELECT 1 FROM TeamMember WHERE IdTeamMember = @id";
            await using var conn = new SqlConnection(_cs);
            await conn.OpenAsync(ct);
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = memberId });
            return await cmd.ExecuteScalarAsync(ct) != null;
        }

        public async Task<GetTeamMemberResponse> GetTeamMemberTasks(int memberId, CancellationToken ct)
        {
            await using var conn = new SqlConnection(_cs);
            await conn.OpenAsync(ct);

            var dto = new GetTeamMemberResponse { MemberId = memberId };
            
            const string sqlMember = @"
            SELECT FirstName, LastName, Email
            FROM TeamMember
            WHERE IdTeamMember = @id";
            await using (var cmd1 = new SqlCommand(sqlMember, conn))
            {
                cmd1.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = memberId });
                await using var reader = await cmd1.ExecuteReaderAsync(ct);
                if (!await reader.ReadAsync(ct))
                    throw new KeyNotFoundException($"Team member with id {memberId} not found");
                dto.FirstName = (string)reader["FirstName"];
                dto.LastName  = (string)reader["LastName"];
                dto.Email = (string)reader["Email"];
                await reader.CloseAsync();
            }
            
            const string sqlAssigned = @"
            SELECT 
            t.Name, t.Description, t.Deadline,
            p.Name   AS ProjectName,
            tt.Name  AS TaskTypeName
            FROM [Task] t
            JOIN Project   p  ON t.IdProject  = p.IdProject
            JOIN TaskType  tt ON t.IdTaskType = tt.IdTaskType
            WHERE t.IdAssignedTo = @id
            ORDER BY t.Deadline DESC";
            await using (var cmd2 = new SqlCommand(sqlAssigned, conn))
            {
                cmd2.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = memberId });
                await using var rdr2 = await cmd2.ExecuteReaderAsync(ct);
                while (await rdr2.ReadAsync(ct))
                {
                    dto.AssignedTasks.Add(new GetTaskResponse
                    {
                        Name         = (string)rdr2["Name"],
                        Description  = (string)rdr2["Description"],
                        Deadline     = (DateTime)rdr2["Deadline"],
                        ProjectName  = (string)rdr2["ProjectName"],
                        TaskTypeName = (string)rdr2["TaskTypeName"],
                    });
                }
                await rdr2.CloseAsync();
            }
            
            const string sqlCreated = @"
            SELECT 
            t.Name, t.Description, t.Deadline,
            p.Name   AS ProjectName,
            tt.Name  AS TaskTypeName
            FROM [Task] t
            JOIN Project   p  ON t.IdProject  = p.IdProject
            JOIN TaskType  tt ON t.IdTaskType = tt.IdTaskType
            WHERE t.IdCreator = @id
            ORDER BY t.Deadline DESC";
            await using (var cmd3 = new SqlCommand(sqlCreated, conn))
            {
                cmd3.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = memberId });
                await using var rdr3 = await cmd3.ExecuteReaderAsync(ct);
                while (await rdr3.ReadAsync(ct))
                {
                    dto.CreatedTasks.Add(new GetTaskResponse
                    {
                        Name         = (string)rdr3["Name"],
                        Description  = (string)rdr3["Description"],
                        Deadline     = (DateTime)rdr3["Deadline"],
                        ProjectName  = (string)rdr3["ProjectName"],
                        TaskTypeName = (string)rdr3["TaskTypeName"],
                    });
                }
                await rdr3.CloseAsync();
            }

            return dto;
        }

        public async Task<bool> ProjectExists(int projectId, CancellationToken ct)
        {
            const string sql = "SELECT 1 FROM Project WHERE IdProject = @id";
            await using var conn = new SqlConnection(_cs);
            await conn.OpenAsync(ct);
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = projectId });
            return await cmd.ExecuteScalarAsync(ct) != null;
        }

        public async Task DeleteProject(int projectId, CancellationToken ct)
        {
            await using var conn = new SqlConnection(_cs);
            await conn.OpenAsync(ct);
            using var tx = conn.BeginTransaction();
            try
            {
                
                await using (var cmd1 = new SqlCommand("DELETE FROM [Task] WHERE IdProject = @id", conn, tx))
                {
                    cmd1.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = projectId });
                    await cmd1.ExecuteNonQueryAsync(ct);
                }
           
                await using (var cmd2 = new SqlCommand("DELETE FROM Project WHERE IdProject = @id", conn, tx))
                {
                    cmd2.Parameters.Add(new SqlParameter("@id", SqlDbType.Int) { Value = projectId });
                    var rows = await cmd2.ExecuteNonQueryAsync(ct);
                    if (rows == 0)
                        throw new KeyNotFoundException($"Project with id {projectId} not found");
                }
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }
    }
}

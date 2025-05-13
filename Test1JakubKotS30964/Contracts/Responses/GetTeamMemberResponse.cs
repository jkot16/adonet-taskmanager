using System.Collections.Generic;

namespace Test1JakubKotS30964.Contracts.Responses
{
    public class GetTeamMemberResponse
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<GetTaskResponse>  AssignedTasks { get; set; } = new();
        public List<GetTaskResponse>  CreatedTasks  { get; set; } = new();
    }
}
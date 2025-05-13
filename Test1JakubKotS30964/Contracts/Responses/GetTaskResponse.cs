using System;

namespace Test1JakubKotS30964.Contracts.Responses
{
    public class GetTaskResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string ProjectName { get; set; }
        public string TaskTypeName { get; set; }
    }
}
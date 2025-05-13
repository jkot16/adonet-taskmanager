using System;
using System.ComponentModel.DataAnnotations;

namespace Test1JakubKotS30964.Contracts.Requests
{
    public class CreateTaskRequest
    {
        [Required] 
        public string Name { get; set; }
        [Required] 
        public string Description { get; set; }
        
        [Required] 
        public DateTime Deadline { get; set; }

        [Required]
        public int IdTeam { get; set; }

        [Required]
        public int IdTaskType { get; set; }

        [Required]
        public int IdAssignedTo { get; set; }

        [Required]
        public int IdCreator  { get; set; }
    }
}
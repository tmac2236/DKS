using System;

namespace DKS_API.DTOs
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
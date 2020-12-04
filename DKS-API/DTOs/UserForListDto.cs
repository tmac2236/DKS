using System;

namespace DFPS.API.DTOs
{
    public class UserForListDto
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
using System;
using System.Collections.Generic;
using DFPS.API.Models;

namespace DFPS.API.DTOs
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Email { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
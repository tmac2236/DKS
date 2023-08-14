using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DKS.API.Models.DKS
{
    public class DevTeamByLoginDto
    {
        public string WorkNo { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string DevTeamNo { get; set; }
        public string Team { get; set; }

    }
}
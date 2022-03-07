using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Countries;
using API.Entities.Countries;

namespace API.DTOs.Members
{
    public class MemberDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public MemberProfileDto MemberProfile { get; set; }
    }
}
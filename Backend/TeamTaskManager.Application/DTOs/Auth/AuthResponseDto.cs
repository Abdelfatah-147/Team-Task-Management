using System;
using System.Collections.Generic;
using System.Text;

namespace TeamTaskManager.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}

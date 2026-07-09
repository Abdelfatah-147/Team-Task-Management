using System;
using System.Collections.Generic;
using System.Text;
using TeamTaskManager.Domain.Entities;

namespace TeamTaskManager.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(AppUser user, IList<string> roles);
    }
}

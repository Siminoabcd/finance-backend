using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using beckend.Models;

namespace beckend.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
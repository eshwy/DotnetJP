using JobPortal2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortal2.Repository
{
    public interface IJWTManagerRepository
    {
        public string Authenticate(UserLogin users);

    }
}

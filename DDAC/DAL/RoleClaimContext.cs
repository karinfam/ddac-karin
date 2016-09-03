using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DDAC.Models;

namespace DDAC.DAL
{
    public class RoleClaimContext : DbContext
    {
        public RoleClaimContext() : base("RoleClaimContext") { }

    }
}
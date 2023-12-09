using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackiSwift.Models;

namespace TrackiSwift.DataAccess.DbInitializer
{
    public interface IDbInitializer
    {
        void Initialize();
    }
}

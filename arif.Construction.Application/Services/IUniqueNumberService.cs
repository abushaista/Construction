using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Application.Services
{
    public interface IUniqueNumberService
    {
        Task<string> GetUniqueNumber();
    }
}

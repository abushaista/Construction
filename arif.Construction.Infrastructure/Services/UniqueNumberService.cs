using arif.Construction.Application.Interfaces;
using arif.Construction.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Infrastructure.Services
{
    public class UniqueNumberService : IUniqueNumberService
    {
        private readonly IConstructionRepository _repo;
        public UniqueNumberService(IConstructionRepository repository)
        {
            _repo = repository;
        }
        public async Task<string> GetUniqueNumber()
        {
            var count = await _repo.GetCount();
            var unique = string.Concat("000000", count.ToString());
            return unique.PadLeft(6);
        }
    }
}

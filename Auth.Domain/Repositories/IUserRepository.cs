using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByEmail(string email);
    Task Add(User user);
}

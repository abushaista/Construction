using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Common;

public interface IPasswordHash
{
    string Generate(string value);
    bool Verify(string value, string hashedValue);
}

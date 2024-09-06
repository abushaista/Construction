using Auth.Application.Abstractions.Messaging;
using Auth.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Queries;

public sealed record LoginQuery(string Email, string Password) : IQuery<AuthenticationResult>;
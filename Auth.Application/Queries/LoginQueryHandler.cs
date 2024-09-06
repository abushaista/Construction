using Auth.Application.Abstractions.Messaging;
using Auth.Application.Common;
using Auth.Domain.Repositories;
using Auth.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Queries;

public class LoginQueryHandler : IQueryHandler<LoginQuery, AuthenticationResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IPasswordHash _hash;
    public LoginQueryHandler(IUserRepository userRepository, IJwtTokenGenerator tokenGenerator, IPasswordHash hash)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
        _hash = hash;
    }
    public async Task<Result<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmail(request.Email);
        if (user == null || !_hash.Verify(request.Password, user.Password))
        {
            return Result.Failure<AuthenticationResult>(new Error("401", "Invalid Credential"));
        }
        var token = _tokenGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }
}
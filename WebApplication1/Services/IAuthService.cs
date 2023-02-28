using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Services
{
    public  interface IAuthService
    {
        //Task<AuthModel> GetTokenAsync(TokenRequestModel model);
        Task<JwtSecurityToken> CreateJwtToken(User user);
    }
}

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobPortal2.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace JobPortal2.Repository
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
		private readonly IConfiguration iconfiguration;
		private readonly JobPortalContext _context;
		public JWTManagerRepository(IConfiguration iconfiguration,JobPortalContext context)
		{
			this.iconfiguration = iconfiguration;
			_context = context;
		}
		public string Authenticate(UserLogin users)
		{
			var data = _context.UserGetisterAndLoginTable.FirstOrDefault(x => x.UserName == users.Name && x.PassWord == users.Password);
			
			if (data is null)
            {
                return null;
            }

            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
			  {
			 new Claim(ClaimTypes.Name, data.UserName),
			 new Claim(ClaimTypes.NameIdentifier, data.User_Id.ToString()),
			
			 
			  }),
				Expires = DateTime.UtcNow.AddMinutes(60),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return new string(new JwtSecurityTokenHandler().WriteToken(token));

		}
	}
}

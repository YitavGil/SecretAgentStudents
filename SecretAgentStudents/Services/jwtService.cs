using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecretAgentStudents.Services
{
    public class jwtService
    {
        private readonly IConfiguration _config;


        // יוצר קונסטרקטור     

        public jwtService(IConfiguration config)
        { 
            _config = config;
        }

        // פונקציה לייצור טוקן
        public string GenerateToken(string codeName)
        {
            // מייצר סיסמה סודית לפי appsettings.json
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            // יוצר נתוני התחברות
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // שומר את נתוני "הסוכן" לזיהוי
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, codeName)
            };

            // יצירת הטוקן
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15), // Token expires in 15 minutes
                signingCredentials: credentials
                );

            // מחזיר את הטוקן
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

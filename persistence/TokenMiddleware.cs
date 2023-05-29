using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

public class TokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;
    public TokenMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _config = config;
    }
    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            attachUserToContext(context, token);
        await _next(context);
    }
    private void attachUserToContext(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var ID = jwtToken.Claims.First(x => x.Type == "ID").Value;
            var Name = jwtToken.Claims.First(x => x.Type == "Name").Value;
            var Username = jwtToken.Claims.First(x => x.Type == "Username").Value;

            // attach user to context on successful jwt validation
            context.Items["ID"] = ID;
            context.Items["Name"] = Name;
            context.Items["Username"] = Username;
        }
        catch (SecurityTokenExpiredException) // token has expired
        {

            context.Items["Token_Expired"] = true;
            // user is not attached to context so request won't have access to secure routes
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Project.RSA;
using System.Net;
namespace ProjectBE.Controllers;

[Route("general")]
public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly IRsaService _rsaService ;

    public AuthController(ILogger<AuthController> logger,IRsaService rsaService)
    {
        _logger = logger;
        _rsaService = rsaService;
    }
    [HttpGet("RSA-Public-Key")]
    public IActionResult GetPublickeyRSA()
    {
        try {
            string PublicKey = _rsaService.getPublicKey();
            //string Encrypted = _rsaService.Encrypt("test");
            //string Decrypted = _rsaService.Decrypt("bqI5Osz8zKKjA8vowNVeiEOrgyqnk/XGyM9VXE5LIZO1h+ieQVQvcuQoryalWEYAHf1XuLAT9y9reZP9a0WXa2kGn58OqbFcACPob2rSftQHJmM6RqfoHfhx7G0Fc9Kmgmhgzb3atHqhgd73gfNsa9WMIQ5HdNDOB/Mq/llOoE4=");
            return StatusCode((int)HttpStatusCode.OK, PublicKey); 
        }
        catch {
            return StatusCode((int)HttpStatusCode.BadRequest, ""); 
        }
    }

}

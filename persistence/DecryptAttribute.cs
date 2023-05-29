using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Newtonsoft.Json;
using Project.RSA;
public class EncryptedData
{
    public string? Encrypted {get;set;}
}
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DecryptedAttribute :  Attribute, IAuthorizationFilter
{
    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        try {
            // Đọc raw request
            var reader = new StreamReader(context.HttpContext.Request.Body,encoding: Encoding.UTF8, detectEncodingFromByteOrderMarks: false);
            string rawContent = await reader.ReadToEndAsync();
            var result = JsonConvert.DeserializeObject<EncryptedData>(rawContent); // Parse json để lấy Encrypted
            if (result != null && !string.IsNullOrEmpty(result.Encrypted))  // Có Encrypted
            {
                IRsaService RsaService = new RsaService();
                string decrypted_jsondata = RsaService.Decrypt(result.Encrypted); // Decrypt request
                byte[] byteArray = Encoding.UTF8.GetBytes(decrypted_jsondata);
                //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
                MemoryStream stream = new MemoryStream(byteArray);
                context.HttpContext.Request.Body = stream;  // Truyền lại request đã decrypt vào body để controller đọc
            }
            else 
            {
                context.Result = new JsonResult(new { message = "Invalid Encryted Data" }) { StatusCode = StatusCodes.Status400BadRequest };
            }
        }
        catch {
            context.Result = new JsonResult(new { message = "Cound not send request" }) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }

}
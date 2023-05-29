
public class UserAccessor : IUserAccessor
{ 
      private IHttpContextAccessor _accessor;
      public UserAccessor(IHttpContextAccessor accessor)
      {
           _accessor = accessor;
      }
      public string? getToken() => _accessor.HttpContext?.Request.Headers["Authorization"].ToString().Split(" ")[1];
      public string? getID() => _accessor.HttpContext?.Items["ID"]?.ToString();
      public string? getCode() => _accessor.HttpContext?.Items["Code"]?.ToString();
      public string? getName() => _accessor.HttpContext?.Items["Name"]?.ToString();
      public string? getUsername() => _accessor.HttpContext?.Items["Username"]?.ToString();


}
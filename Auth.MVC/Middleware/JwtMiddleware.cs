//using Auth.Core;

namespace Auth.MVC.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Se já existe o header Authorization, não faz nada
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            // Tenta buscar o token de um cookie
            var token = context.Request.Cookies["access_token"];
            Console.WriteLine("Authorization Header: " + context.Request.Headers["Authorization"]);


            if (!string.IsNullOrEmpty(token))
            {
                // Injeta o token no header Authorization
                context.Request.Headers.Append("Authorization", $"Bearer {token}");
            }
        }

        await _next(context);
    }
}


//public class JwtMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly string _secretKey;

//    public JwtMiddleware(RequestDelegate next, IConfiguration config)
//    {
//        _next = next;
//        _secretKey = config["Jwt:Key"];
//    }

//    public async Task Invoke(HttpContext context)
//    {
//        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

//        if (!string.IsNullOrEmpty(token))
//        {
//            var authService = new AuthService(_secretKey);
//            try
//            {
//                var principal = authService.ValidateToken(token);
//                context.User = principal;
//            }
//            catch
//            {
//                // Token inválido
//            }
//        }

//        await _next(context);
//    }
//}

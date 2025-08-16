# 🔐 ASP.NET Core JWT Authentication
## MVC & Web API Integration

This project demonstrates a clean and modular implementation of JWT-based authentication using ASP.NET Core MVC and Web API. It includes token generation, validation, and middleware integration to secure endpoints and enable role-based access control.

---

## 📁 Project Structure

- `Auth.WebAPI`: Responsible for issuing JWT tokens via a login endpoint.
- `Auth.MVC`: A client application that consumes the token and accesses protected resources.
- `AuthService`: Encapsulates token generation logic using `System.IdentityModel.Tokens.Jwt`.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 6.0+](https://dotnet.microsoft.com/en-us/download)
- Visual Studio or VS Code
- Postman or any HTTP client for testing

---

## ⚙️ Configuration

Ensure your `appsettings.json` contains the following section:

```json
"Jwt": {
  "Key": "your-secure-key",
  "Issuer": "AuthApp",
  "Audience": "AuthApp"
}
```

---

## 🔧 Web API Setup (`Auth.WebAPI`)

### `AuthService.cs`

Handles token creation with embedded claims:

```csharp
public class AuthService
{
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(string username, string role, string profile)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim("profile", profile)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

### `AuthController.cs`

Exposes a login endpoint that returns a JWT upon successful authentication:

```csharp
[HttpPost("login")]
public IActionResult Login([FromBody] LoginModel model)
{
    if (model.Username == "ivan" && model.Password == "123")
    {
        var token = _authService.GenerateToken(model.Username, "Admin", "Finance");
        return Ok(new { token });
    }

    return Unauthorized();
}
```

---

## 🖥️ MVC Client Setup (`Auth.MVC`)

### `Program.cs`

Configures authentication and authorization using JWT Bearer:

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "AuthApp",
            ValidAudience = "AuthApp",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
```

### Middleware Pipeline

```csharp
app.UseStaticFiles();
app.UseRouting();
app.UseMiddleware<JwtMiddleware>(); // Injects token into request headers
app.UseAuthentication();            // Validates token
app.UseAuthorization();             // Enforces policies
app.MapDefaultControllerRoute();
```

---

## 🔐 Securing Endpoints

Use the `[Authorize]` attribute to protect controllers or actions:

```csharp
[Authorize(Roles = "Admin")]
public IActionResult Dashboard()
{
    return View();
}
```

---

## 🧪 Testing

1. Send a `POST` request to `/api/auth/login` with valid credentials.
2. Receive a JWT token in the response.
3. Include the token in the `Authorization` header of MVC requests:
   ```
   Authorization: Bearer <your-token>
   ```
4. Access protected routes and verify role-based access.

---

## 📌 Notes

- Token expiration is set to 1 hour.
- Claims include `Name`, `Role` and `Profile`.
- Middleware ensures token is injected and validated before authorization.

---

## 🧠 Credits

Crafted with care by me, with a focus on clarity, modularity, and security.  
If you find this helpful, feel free to fork, extend, or adapt it to your own needs.
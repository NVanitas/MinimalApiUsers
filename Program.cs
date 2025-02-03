using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalApiUsers.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuração do JWT
var keyString = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(keyString))
{
    throw new InvalidOperationException("A chave JWT não está configurada.");
}
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

// Adicionando o contexto do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do JWT Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = key
        };
    });

// Adicionando o serviço de autorização
builder.Services.AddAuthorization();

// Adicionando Swagger para documentação
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Usando Swagger para documentação da API
app.UseSwagger();
app.UseSwaggerUI();

// Usando autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Endpoints da API

// Endpoint para registro de usuário
app.MapPost("/register", async (UserDto userDto, AppDbContext dbContext) =>
{
    var user = new User
    {
        Name = userDto.Name,
        Email = userDto.Email
    };

    dbContext.Users.Add(user);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/users/{user.Id}", user);
}).WithName("RegisterUser");

// Endpoint para login (geração de token JWT)
app.MapPost("/login", async (UserLoginDto userLoginDto, AppDbContext dbContext) =>
{
    var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == userLoginDto.Email);
    if (user == null || userLoginDto.Password != "password") 
    {
        return Results.Unauthorized();
    }

    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new System.Security.Claims.ClaimsIdentity(new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.Name),
        }),
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    var tokenString = tokenHandler.WriteToken(token);

    return Results.Ok(new { Token = tokenString });
}).WithName("LoginUser");

app.Run();

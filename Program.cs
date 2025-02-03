using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalApiUsers.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do JWT
var keyString = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(keyString))
{
    throw new InvalidOperationException("A chave JWT n�o est� configurada.");
}
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

// Adicionando o contexto do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura��o do JWT Authentication
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

// Adicionando o servi�o de autoriza��o
builder.Services.AddAuthorization();

// Adicionando Swagger para documenta��o
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Usando Swagger para documenta��o da API
app.UseSwagger();
app.UseSwaggerUI();

// Usando autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();

// Endpoints da API

// Endpoint para registro de usu�rio
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

// Endpoint para login (gera��o de token JWT)
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

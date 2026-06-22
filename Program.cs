
using Microsoft.EntityFrameworkCore;
using BookingApiControl.Data;

using BookingApiControl.Services;
using BookingApiControl.Models.Enums;


//JWT Auth
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers(); //Добавляем для работы Controllers

//Для работы с БД
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnectionDb")));

builder.Services.AddScoped<AuthService>(); //AuthService.cs
builder.Services.AddScoped<RoomService>(); //RoomService.cs
builder.Services.AddScoped<AdminService>(); //AdminService.cs
builder.Services.AddScoped<BookingService>(); //BookingService.cs

//JWT Auth
var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key");
//Проверка на пустоту
if (string.IsNullOrWhiteSpace(jwtKey)) { throw new Exception("JWT Key is missing in appsettings.json"); } 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            )
        };
    });

builder.Services.AddScoped<JwtTokenService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Создание Админа.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!db.Users.Any(x => x.Role == Role.Admin.ToString()))
    {
        var admin = new User
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            Email = "admin@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Role = Role.Admin.ToString()
        };

        db.Users.Add(admin);
        db.SaveChanges();
    }
}


app.MapControllers();   //Добавляем для работы Controllers

app.UseHttpsRedirection(); //переадресация на https

app.UseAuthentication(); //используется для аунтификации(проверки токена)
app.UseAuthorization(); //используется авторизация



app.Run();



using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using taskmanagerapi.Configs;
using taskmanagerapi.Models;
using taskmanagerapi.Models.UserModels;
using taskmanagerapi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TaskService>();
// add database
builder.Services.AddDbContext<TaskdbContext>(opt => opt.UseLazyLoadingProxies().UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql")));

// add identity
builder.Services.AddIdentity<User, IdentityRole>(opts =>
    {
        opts.Password.RequiredLength = 5;
        opts.Password.RequireNonAlphanumeric = false;   
        opts.Password.RequireLowercase = false; 
        opts.Password.RequireUppercase = false; 
        opts.Password.RequireDigit = true;
    })
    .AddEntityFrameworkStores<TaskdbContext>()
    .AddDefaultTokenProviders();



// jwt token
var getJwtSection = builder.Configuration.GetSection(JwtConfig.Position);
var jwtConfig = getJwtSection.Get<JwtConfig>();
builder.Services.Configure<JwtConfig>(getJwtSection);
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

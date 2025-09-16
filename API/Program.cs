using Application.Interface;
using Application.Mapper;
using AutoMapper;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Optivem.Framework.Core.Domain;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

// Auto mapper

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));

builder.Services.AddIdentity<User , IdentityRole> (options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
}).AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<AppDbContext>();


    builder.Services.AddAuthentication((options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    })).
    AddJwtBearer(options =>
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    }
    );
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HRMS API", Version = "v1" });

    // Add JWT Bearer Definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your valid token "
    });

    // Add global security requirement
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>()); // Corrected to use a lambda to configure AutoMapper
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
//builder.Services.AddScoped<IDeptEmpService, DeptEmpService>();
//builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddScoped<IAuthService, AuthService>();
var imagePath = Path.Combine(builder.Environment.WebRootPath, "images");

builder.Services.AddScoped<IImageService>(provider =>
{
    var dbContext = provider.GetRequiredService<AppDbContext>();
    return new ImageService(dbContext, imagePath);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

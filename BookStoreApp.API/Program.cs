using BookStoreApp.API.Configurations;
using BookStoreApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;//2.0
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var connString = builder.Configuration.GetConnectionString("BookStoreAppDbConnection"); // this tells look in the appsettings.json ->(.Configuration object) and GetConnectionString("") by that name
        builder.Services.AddDbContext<BookStoreDbContext>(options => options.UseSqlServer(connString));
        builder.Services.AddIdentityCore<ApiUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<BookStoreDbContext>(); // added service for Identity.EFC for API authentication

        builder.Services.AddAutoMapper(typeof(MapperConfig));

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Host.UseSerilog((ctx, lc/*we are creating a deligate function ctx-configurationContext, lc-LoggingConfiguration*/) =>
            lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration /*ctx represents the entire configuartion file and object is Configuration*/)
        ); //1.0 //3.0

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll"/*ALlowAll is the policy name*/, b => b.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin() /*b is the action name and the rest is are the actions done by b*/);
        }); // this is the CORS policy that we have built now we need to tell our app to use it 

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //added jwt authentication
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = builder.Configuration["JwtSettings: Issuer"],
                ValidAudience = builder.Configuration["JwtSettings: Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))


            };
        });

        var app = builder.Build(); // Below this is the app building section or called as "Middleware section"

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors("AllowAll"); // This CORS policy will allow any client from any where to access

        app.UseAuthentication(); // add authentication 

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
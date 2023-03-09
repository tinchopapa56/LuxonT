using Persistence;
using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.Repositories;
using API.TokenService;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<LuxonDB>(opts=>
// {
//     opts.UseNpgsql (builder.Configuration.GetConnectionString("LC"));
// });

builder.Services.AddDbContext<LuxonDB>(options =>
{
       // Use connection string provided at runtime by FlyIO.
       var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
       // Parse connection URL to connection string for Npgsql
       connUrl = connUrl.Replace("postgres://", string.Empty);
       var pgUserPass = connUrl.Split("@")[0];
       var pgHostPortDb = connUrl.Split("@")[1];
       var pgHostPort = pgHostPortDb.Split("/")[0];
       var pgDb = pgHostPortDb.Split("/")[1];
       var pgUser = pgUserPass.Split(":")[0];
       var pgPass = pgUserPass.Split(":")[1];
       var pgHost = pgHostPort.Split(":")[0];
       var pgPort = pgHostPort.Split(":")[1];
       var connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};";
       options.UseNpgsql(connStr);      
});

builder.Services.AddSendGrid(options =>
{
    // options.ApiKey = (builder.Configuration.GetSection("SendGridApiKey").Value);
    // options.ApiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY") //TOML;
    options.ApiKey = "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";       //ESTA HARDCODEADOOOOOOOOOOOOO
});

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ITaskyRepo, TaskyRepo>();
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<TokenService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//ADD CORS
 builder.Services.AddCors(opt => 
{
    opt.AddPolicy("CorsPolicy", policy => 
    {
        policy 
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin();
            // .AllowCredentials()
            // .WithOrigins("http://localhost:5173");
    });
});


// var tokenSecret= Environment.GetEnvironmentVariable("SendGridApiKey"); //TOML;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters{
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = "http://localhost:5157",
                ValidAudience = "http://localhost:5157",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superIncredibleSecret"))

                // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superIncredibleSecret")), //ESTA HARDCODEADO

                // ValidIssuer = builder.Configuration["Jwt: Issuer"],
                // ValidAudience = builder.Configuration["Jwt: Audience"],
                // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try{
    var context = services.GetRequiredService<LuxonDB>();
    // var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context);
}
catch(Exception ex){
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error ocurred during migration");
}

app.Run();

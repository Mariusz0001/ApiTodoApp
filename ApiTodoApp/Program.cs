using ApiTodoApp.Infrastructure.Authentication;
using ApiTodoApp.Infrastructure.Database;
using ApiTodoApp.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var authSecrets = new AuthSecrets(builder.Configuration
    .GetSection("AuthSecrets.UserName").Get<string>(), builder.Configuration
    .GetSection("AuthSecrets.Password").Get<string>(), builder.Configuration
    .GetSection("AuthSecrets.Issuer").Get<string>(), builder.Configuration
    .GetSection("AuthSecrets.Audience").Get<string>(), builder.Configuration
    .GetSection("AuthSecrets.ExpirationSeconds").Get<double>(), builder.Configuration
    .GetSection("AuthSecrets.SigningKey").Get<string>());

builder.Services.AddSingleton(authSecrets);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        byte[] signingKeyBytes = Encoding.UTF8
            .GetBytes(authSecrets.SigningKey);

        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authSecrets.Issuer,
            ValidAudience = authSecrets.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IPersonalTasksRepository, PersonalTasksRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DbConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DbConnection")));
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost").AllowAnyHeader().AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapPost("/tokens/connect", (HttpContext ctx, AuthSecrets authSecrets)
    => TokenEndpoint.Connect(ctx, authSecrets));

app.Run();

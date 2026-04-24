using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using ProvNetChallenge.Infrastructure.Data;
using ProvNetChallenge.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

#region Controllers
builder.Services.AddControllers();
#endregion

#region OpenAPI + Scalar
// dotnet add package Microsoft.OpenApi
// dotnet add package Scalar.AspNetCore
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        var bearerScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            In = ParameterLocation.Header,
            BearerFormat = "JWT"
        };

        document.Components ??= new OpenApiComponents();
        document.AddComponent("Bearer", bearerScheme);

        return Task.CompletedTask;
    });

    options.AddOperationTransformer((operation, context, cancellationToken) =>
    {
        // Get the attributes you put on the endpoint (e.g., [Authorize], [AllowAnonymous])
        var metadata = context.Description.ActionDescriptor.EndpointMetadata;
        
        var hasAuthorize = metadata.OfType<Microsoft.AspNetCore.Authorization.IAuthorizeData>().Any();
        var hasAllowAnonymous = metadata.OfType<Microsoft.AspNetCore.Authorization.IAllowAnonymous>().Any();

        // If the endpoint has [Authorize] and does NOT have [AllowAnonymous]...
        if (hasAuthorize && !hasAllowAnonymous)
        {
            // Add the security requirement (the lock icon)
            var securityRequirement = new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = []
            };

            operation.Security ??= new List<OpenApiSecurityRequirement>();
            operation.Security.Add(securityRequirement);
        }

        return Task.CompletedTask;
    });
});
#endregion

#region JWT
// dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
builder.Services.AddAuthorization();
var secretKey = builder.Configuration["JwtSecretKey"]!;
if (string.IsNullOrWhiteSpace(secretKey) || secretKey.Length < 32)
{
    Console.ForegroundColor = ConsoleColor.Red;
    
    Console.WriteLine("\n=======================================================================");
    Console.WriteLine(" FATAL ERROR: The server cannot start.");
    Console.WriteLine(" 'JwtSecretKey' is missing in environment variables or is invalid.");
    Console.WriteLine("=======================================================================\n");
    
    Console.ResetColor();

    // Turn off the application forcefully. The '1' tells Docker/Coolify that the app failed and should not be deployed.
    Environment.Exit(1); 
}
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
    {
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        
        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidateAudience = true;
        options.TokenValidationParameters.ValidateLifetime = true;
        options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
    });
#endregion

#region Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region Services
// Application
builder.Services.AddScoped<ProvNetChallenge.Application.Interfaces.IUserService, ProvNetChallenge.Application.Services.UserService>();
builder.Services.AddScoped<ProvNetChallenge.Application.Interfaces.IAuthService, ProvNetChallenge.Application.Services.AuthService>();
builder.Services.AddScoped<ProvNetChallenge.Application.Interfaces.IProductService, ProvNetChallenge.Application.Services.ProductService>();

// Infraestructura
builder.Services.AddScoped<ProvNetChallenge.Application.Interfaces.Repositories.IUserRepository, ProvNetChallenge.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<ProvNetChallenge.Application.Interfaces.Repositories.IProductRepository, ProvNetChallenge.Infrastructure.Repositories.ProductRepository>();
builder.Services.AddScoped<ProvNetChallenge.Application.Interfaces.IJwtService, ProvNetChallenge.Infrastructure.Services.JwtService>();
#endregion

var app = builder.Build();

// Crear la base de datos automáticamente al arrancar
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    app.MapGet("/", () => Results.Redirect("/scalar/v1"))
       .ExcludeFromDescription();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseRouting();

// Estos dos siempre van juntos en este orden exacto, DESPUÉS del routing
app.UseAuthentication(); 
app.UseAuthorization();  

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();
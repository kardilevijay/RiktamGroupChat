using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Riktam.GroupChat.Apis.Configurations;
using Riktam.GroupChat.Apis.Filters;
using Riktam.GroupChat.Domain;
using Riktam.GroupChat.Domain.Common;
using Riktam.GroupChat.SqlDbProvider;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
AddApiVersioning(builder.Services);
AddAuthentication(builder);
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
    options.Filters.Add(BuildAuthorizationFilter(JwtBearerDefaults.AuthenticationScheme));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
AddSwaggerGen(builder.Services);
ConfigureApiBehaviorOptions(builder);

DomainConfigrator.ConfigureServices(builder.Services, builder.Configuration);
SqlDbProviderConfigrator.ConfigureServices(builder.Services, builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    var apiVersionInfo = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwaggerUI(o =>
    {
        foreach (var version in apiVersionInfo.ApiVersionDescriptions.Reverse().Select(x => x.GroupName))
        {
            o.SwaggerEndpoint($"/swagger/{version}/swagger.json", version);
        }
        o.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void ConfigureApiBehaviorOptions(WebApplicationBuilder builder)
{
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Type = AppErrorCodes.InvalidRequest.ToString(),
                Title = AppErrorCodes.InvalidRequest.GetDescription(),
                Status = StatusCodes.Status400BadRequest,
            };

            return new BadRequestObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json", "application/problem+xml" }
            };
        };
    });
}

static void AddAuthentication(WebApplicationBuilder builder)
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? throw new ApplicationException("JwtSettings.SecretKey not defined"));

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
}
static AuthorizeFilter BuildAuthorizationFilter(string scheme)
{
    var policyBuilder = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(scheme);

    return new AuthorizeFilter(policyBuilder.Build());
}

static void AddSwaggerGen(IServiceCollection services)
{
    services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme }
                },
                Array.Empty<string>()
            }
        });
    });
}

static void AddApiVersioning(IServiceCollection services)
{
    services.AddApiVersioning(o =>
    {
        o.ApiVersionReader = new UrlSegmentApiVersionReader();
        o.UseApiBehavior = true;
    });

    services.AddVersionedApiExplorer(o =>
    {
        o.GroupNameFormat = "'v'VVV";
        o.SubstituteApiVersionInUrl = true;
    });
}

public partial class Program { }

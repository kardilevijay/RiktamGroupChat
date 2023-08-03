using Microsoft.AspNetCore.Mvc;
using Riktam.GroupChat.Apis.Filters;
using Riktam.GroupChat.Domain;
using Riktam.GroupChat.Domain.Common;
using Riktam.GroupChat.SqlDbProvider;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
ConfigureApiBehaviorOptions(builder);

DomainConfigrator.ConfigureServices(builder.Services, builder.Configuration);
SqlDbProviderConfigrator.ConfigureServices(builder.Services, builder.Configuration);

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
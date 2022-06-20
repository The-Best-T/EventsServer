using AspNetCoreRateLimit;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using Server.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
services.ConfigureCors();
services.ConfigureIISIntegration();

services.AddMemoryCache();
services.AddHttpContextAccessor();
services.ConfigureRateLimitingOptions();

services.ConfigureApi();
services.ConfigureSwagger();
services.ConfigureVersioning();

services.ConfigureIdentity();
services.ConfigureRepositoryManager();
services.ConfigureJWT(builder.Configuration);

services.ConfigureFilters();
services.ConfigureLoggerService();
services.AddAutoMapper(typeof(Program));
services.ConfigureSqlContext(builder.Configuration);

services.AddControllers(config =>
{
    config.ReturnHttpNotAcceptable = true;
    config.RespectBrowserAcceptHeader = true;
}).AddXmlDataContractSerializerFormatters();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Meetup API v1");
});

app.ConfigureExceptionHandler(new LoggerManager());
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("CorsPolicy");
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseIpRateLimiting();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

using (var scope = app.Services.CreateScope())
{
    var scopedServices = scope.ServiceProvider;
    try
    {
        var db = scopedServices.GetRequiredService<RepositoryContext>();
        var userManager = scopedServices.GetRequiredService<UserManager<User>>();

        await db.Database.MigrateAsync();
        await AdminInitializer.InitializeAsync(userManager);
    }
    catch (Exception ex)
    {
        var logger = scopedServices.GetRequiredService<ILoggerManager>();
        logger.LogError($"An error occurred while seeding the database: {ex}.");
    }
}

app.Run();

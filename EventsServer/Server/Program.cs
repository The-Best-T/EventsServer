using AspNetCoreRateLimit;
using Microsoft.AspNetCore.HttpOverrides;
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
services.ConfigureVersioning();

services.AddAuthentication();
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

app.Run();

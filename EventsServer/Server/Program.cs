using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using Server.Extensions;
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
services.ConfigureApi();
services.ConfigureCors();
services.ConfigureFilters();
services.ConfigureVersioning();
services.ConfigureLoggerService();
services.ConfigureIISIntegration();
services.ConfigureRepositoryManager();
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

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

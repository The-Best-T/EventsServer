using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using Server.Extensions;
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
services.ConfigureCors();
services.ConfigureLoggerService();
services.ConfigureIISIntegration();
services.ConfigureRepositoryManager();
services.AddAutoMapper(typeof(Program));
services.ConfigureSqlContext(builder.Configuration);

builder.Services.AddControllers();

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

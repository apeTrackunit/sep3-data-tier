using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Sep3DataTier.Database;
using Sep3DataTier.Repository;
using Sep3DataTier.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

builder.WebHost.ConfigureKestrel(options =>
{
    // Setup a HTTP/2 endpoint without TLS.
    options.ListenLocalhost(5266, o => o.Protocols = HttpProtocols.Http2);
});
// Add services to the container.

builder.Services.AddGrpc();

//Efc services
builder.Services.AddScoped<IReportDao, ReportDao>();
builder.Services.AddScoped<IUserDao, UserDao>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<DatabaseContext>();
builder.Services.AddDbContext<DatabaseContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ReportService>();
app.MapGrpcService<AuthService>();
app.UseAuthentication();
app.Run();


using ClientAuthentication;
using Ghtk.Authorization;
using GHTK.API.AuthenticationHandler;
using GHTK.Repository;
using GHTK.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;

namespace GHTK.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            IClientSourceAuthenticationHandler clientSourceAuthenticationHandler = new RemoteClientSourceAuthenticationHandler(builder.Configuration["AuthenticationService"] ?? throw new Exception("ClientAuthenticationServiceUrl not found"));

            builder.Services.AddControllers();

            builder.Services.AddAuthentication("XClientSource").AddXClientSource(
                    options =>
                    {
                        options.ClientValidator = (clientSource,token, principal) => clientSourceAuthenticationHandler.Validate(clientSource);
                        options.IssuerSigningKey = builder.Configuration["IssuerSigningKey"] ?? "";
                    }
                );

            builder.Services.AddDbContext<SqlServerDbContext>(
                opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
            builder.Services.AddTransient<IOrderRepository, SqlServerOrderRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

using ClientAuthentication;
using Ghtk.Authorization;
using GHTK.API.AuthenticationHandler;
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

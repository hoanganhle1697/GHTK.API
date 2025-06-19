using ClientAuthentication;

namespace GHTK.API.AuthenticationHandler
{
    public class RemoteClientSourceAuthenticationHandler : IClientSourceAuthenticationHandler
    {
        private readonly string _authenticationServiceUrl;
        private static HttpClient httpClient = new();

        public RemoteClientSourceAuthenticationHandler(string authenticactionServiceUrl)
        {
            _authenticationServiceUrl = authenticactionServiceUrl;
        }
        public bool Validate(string clientSource)
        {
            if (string.IsNullOrEmpty(clientSource))
            {
                return false;
            }
            var response= httpClient.GetAsync($"{_authenticationServiceUrl}/api/ClientSource/{clientSource}").Result;
            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}

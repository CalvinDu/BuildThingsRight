using IdentityModel;
using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }
            var token = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A",
                Scope = "api1"
            });

            if (token.IsError)
            {
                Console.WriteLine(token.Error);
                return;
            }
            Console.WriteLine(token.Json);

            await Task.Delay(3600 * 1000);
            client.SetBearerToken(token.AccessToken);

            var response = await client.GetAsync("https://localhost:44357/weatherforecast");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else 
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }

        }
    }
}

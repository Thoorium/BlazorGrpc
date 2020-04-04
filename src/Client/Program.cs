using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Grpc.Net.Client;
using GrpcGreeter;

namespace BlazorGrpc.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var wasmHttpMessageHandlerType = System.Reflection.Assembly.Load("WebAssembly.Net.Http").GetType("WebAssembly.Net.Http.HttpClient.WasmHttpMessageHandler");
            var streamingProperty = wasmHttpMessageHandlerType.GetProperty("StreamingEnabled", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            streamingProperty.SetValue(null, true, null);

            builder.Services.AddBaseAddressHttpClient();

            builder.Services.AddSingleton(services =>
            {
                // Create a gRPC-Web channel pointing to the backend server
                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
                var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });

                // Now we can instantiate gRPC clients for this channel
                return new Greeter.GreeterClient(channel);
            });


            await builder.Build().RunAsync();
        }
    }
}

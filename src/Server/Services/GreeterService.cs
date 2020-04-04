using Grpc.Core;
using GrpcGreeter;
using System.Threading.Tasks;

namespace BlazorGrpc.Server.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            var reply = new HelloReply { Message = string.IsNullOrWhiteSpace(request.Name) ? "Please give me your name." : $"Hello from gRPC, {request.Name}!" };
            return Task.FromResult(reply);
        }
    }
}

using Grpc.Core;
using GrpcGreeter;
using System;
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

        public override async Task SayHellos(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            if(string.IsNullOrEmpty(request.Name))
            {
                var reply = new HelloReply { Message = "Please give me your name." };
                await responseStream.WriteAsync(reply);
                return;
            }

            for (int i = 1; i <= 5; i++)
            {
                var reply = new HelloReply { Message = $"Hello from gRPC, {request.Name}{"!".PadRight(i, '!')}" };
                await responseStream.WriteAsync(reply);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}

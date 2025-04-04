using Grpc.Core;
using Grpc.Core.Interceptors;
using System.Diagnostics;

namespace Products.Common.Protos
{
    public class SessionIdGrpcInterceptor : Interceptor
    {
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var metadata = new Metadata();
            var sessionId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString();

            metadata.Add("X-Session-Id", sessionId);

            var updatedContext = new ClientInterceptorContext<TRequest, TResponse>(
                context.Method,
                context.Host,
                context.Options.WithHeaders(metadata)
            );

            return continuation(request, updatedContext);
        }
    }
}

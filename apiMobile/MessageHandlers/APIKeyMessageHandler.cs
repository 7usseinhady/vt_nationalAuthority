using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace apiMobile.MessageHandlers
{
    public class APIKeyMessageHandler : DelegatingHandler
    {
        /// <summary>
        ///   Check This User Have Our Key Or Not. 
        /// </summary>
        /// <param name="httpRequestMessage"> Represents A Http Request Message. </param>
        /// <param name="cancellationToken">  Propagates notification that operations should be canceled. </param>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            bool validKey = false, log = false;
            IEnumerable<string> requestHeaders, values;
            string ipAddress = httpRequestMessage.GetClientIpAddress();

            var x = httpRequestMessage.Headers.TryGetValues("APIKey", out values);
            if (x)
            {
                values = httpRequestMessage.Headers.GetValues("APIKey");
                log = new logs().reqLogs(httpRequestMessage, "APIKey", values.FirstOrDefault().ToString(), ipAddress);
            }
            else
                log = new logs().reqLogs(httpRequestMessage, null, null, ipAddress);

            var checkApiKeyExists = httpRequestMessage.Headers.TryGetValues("APIKey", out requestHeaders);
            if (checkApiKeyExists && log)
                if (requestHeaders.FirstOrDefault().Equals(values.FirstOrDefault().ToString()))
                    validKey = true;

            if (!validKey)
                return httpRequestMessage.CreateResponse(System.Net.HttpStatusCode.Forbidden);

            var response = await base.SendAsync(httpRequestMessage, cancellationToken);
            return response;
        }
    }

    public static class HttpRequestMessageExtensions
    {
        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }

            return null;
        }
    }
}
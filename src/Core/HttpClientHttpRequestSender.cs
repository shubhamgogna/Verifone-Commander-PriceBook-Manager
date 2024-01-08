// -----------------------------------------------------------------------
// <copyright file="HttpClientHttpRequestSender.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class HttpClientHttpRequestSender : IHttpRequestSender, IDisposable
    {
        private readonly HttpClientHandler httpClientHandler;
        private readonly HttpClient httpClient;

        public HttpClientHttpRequestSender()
        {
            this.httpClientHandler = new HttpClientHandler()
            {
                // Required for custom certificate generated for the POS systems
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
            };

            this.httpClient = new HttpClient(this.httpClientHandler);
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return this.httpClient.SendAsync(request, cancellationToken);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.httpClientHandler.Dispose();
            this.httpClient.Dispose();
        }
    }
}

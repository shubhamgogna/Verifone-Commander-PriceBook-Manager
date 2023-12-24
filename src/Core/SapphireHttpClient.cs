// -----------------------------------------------------------------------
// <copyright file="SapphireHttpClient.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Microsoft.Extensions.Logging;

    public class SapphireHttpClient : ISapphireHttpClient
    {
        private const string CgiBinNaxmlPath = "/cgi-bin/NAXML";

        private readonly HttpClientHandler httpClientHandler;
        private readonly HttpClient httpClient;

        private readonly Uri requestUri;
        private readonly string username;
        private readonly string password;
        private readonly ILogger<SapphireHttpClient> logger;

        private string cookie;

        public SapphireHttpClient(
            string hostname,
            string username,
            string password,
            ILogger<SapphireHttpClient> logger)
        {
            if (string.IsNullOrWhiteSpace(hostname))
            {
                throw new ArgumentException($"'{nameof(hostname)}' cannot be null or empty.", nameof(hostname));
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException($"'{nameof(username)}' cannot be null or empty.", nameof(username));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));
            }

            this.httpClientHandler = new HttpClientHandler()
            {
                // Required for custom certificate generated for the POS systems
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
            };
            this.httpClient = new HttpClient(this.httpClientHandler);

            this.requestUri = new Uri("https://" + hostname + CgiBinNaxmlPath);
            this.username = username;
            this.password = password;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<string> SendAsync(
            string cmdHeader,
            CancellationToken cancellationToken)
        {
            return this.SendAsync(
                cmdHeader: cmdHeader,
                body: null,
                cancellationToken: cancellationToken);
        }

        public async Task<string> SendAsync(
            string cmdHeader,
            string body,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(cmdHeader))
            {
                throw new ArgumentException($"'{nameof(cmdHeader)}' cannot be null or whitespace.", nameof(cmdHeader));
            }

            await this.EnsureCookieAsync(cancellationToken).ConfigureAwait(false);

            var requestContent = GetRequestBody(cmdHeader, body, this.cookie);
            using var request = this.CreateRequest(requestContent);
            using var response = await this.httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var responseContent = await ReadResponseContentAsync(response, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode ||
                responseContent.Contains("VFI:Fault") ||
                responseContent.Contains("faultCode") ||
                responseContent.Contains("faultString"))
            {
                this.LogUnexpectedResponse(
                    GetRequestBody(cmdHeader, body, "[REDACTED_COOKIE]"),
                    response,
                    responseContent);
                throw new HttpRequestException(responseContent);
            }

            return responseContent;
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

        private static string GetRequestBody(
            string cmdHeader,
            string body,
            string cookie)
        {
            var sb = new StringBuilder();
            sb.Append(cmdHeader);
            sb.Append("&cookie=");
            sb.AppendLine(cookie);

            if (!string.IsNullOrEmpty(body))
            {
                sb.AppendLine();
                sb.Append(body);
            }

            return sb.ToString();
        }

        private static async Task<string> ReadResponseContentAsync(
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            var responseContent = string.Empty;
            if (response.Content != null)
            {
                responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            }

            return responseContent;
        }

        private HttpRequestMessage CreateRequest(
            string content)
        {
            return new HttpRequestMessage(HttpMethod.Post, this.requestUri)
            {
                Content = new StringContent(content, Encoding.UTF8, "text/plain"),
            };
        }

        private void LogUnexpectedResponse(
            string requestContent,
            HttpResponseMessage response,
            string responseContent)
        {
            this.logger.LogError(
                "Request with '{requestContent}' failed with '{httpResponseCode}' '{httpResponsePhrase}' and '{httpResponseContent}'",
                requestContent,
                response.StatusCode,
                response.ReasonPhrase,
                responseContent);
        }

        private async Task EnsureCookieAsync(
            CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(this.cookie))
            {
                return;
            }

            using var request = this.CreateRequest($"cmd=validate&user={this.username}&passwd={this.password}");
            using var response = await this.httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var responseContent = await ReadResponseContentAsync(response, cancellationToken).ConfigureAwait(false);

            var doc = XDocument.Parse(responseContent);
            this.cookie = doc.Descendants("cookie").First().Value;
        }
    }
}

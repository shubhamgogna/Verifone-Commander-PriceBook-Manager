// -----------------------------------------------------------------------
// <copyright file="SapphireCredentialProvider.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Microsoft.Extensions.Logging;

    public class SapphireCredentialProvider : IModifiableSapphireCredentialsProvider
    {
        private const string CgiBinNaxmlPath = "/cgi-bin/NAXML";

        private readonly IHttpRequestSender httpRequestSender;
        private readonly ILogger<SapphireCredentialProvider> logger;

        private Uri requestUri;
        private string username;
        private string password;
        private string cookie;

        public SapphireCredentialProvider(
            IHttpRequestSender httpRequestSender,
            ILogger<SapphireCredentialProvider> logger)
        {
            this.httpRequestSender = httpRequestSender ?? throw new ArgumentNullException(nameof(httpRequestSender));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void SetLoginCredentials(
            string hostName,
            string username,
            string password)
        {
            if (string.IsNullOrWhiteSpace(hostName))
            {
                throw new ArgumentException($"'{nameof(hostName)}' cannot be null or empty.", nameof(hostName));
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException($"'{nameof(username)}' cannot be null or empty.", nameof(username));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));
            }

            // Reset cookie
            this.cookie = null;

            this.requestUri = new Uri("https://" + hostName + CgiBinNaxmlPath);
            this.username = username;
            this.password = password;
        }

        public async Task<ISapphireCredentials> GetCredentialsAsync(
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(this.cookie))
            {
                using var request = SapphireHttpUtil.CreateRequest(
                    this.requestUri,
                    $"cmd=validate&user={this.username}&passwd={this.password}");

                using var response = await this.httpRequestSender.SendAsync(
                    request,
                    cancellationToken).ConfigureAwait(false);

                var responseContent = await SapphireHttpUtil.ReadResponseContentAsync(
                    response,
                    cancellationToken).ConfigureAwait(false);

                if (SapphireHttpUtil.IsUnsuccessfulResponse(
                    response: response,
                    requestContent: "cmd=validate",
                    responseContent: responseContent,
                    logger: this.logger))
                {
                    throw new SapphireRequestException(responseContent);
                }

                var doc = XDocument.Parse(responseContent);
                this.cookie = doc.Descendants("cookie").First().Value;
            }

            return new SapphireCredential()
            {
                NaxmlRequestUri = this.requestUri,
                Cookie = this.cookie,
            };
        }

        private class SapphireCredential : ISapphireCredentials
        {
            public Uri NaxmlRequestUri { get; set; }

            public string Cookie { get; set; }
        }
    }
}

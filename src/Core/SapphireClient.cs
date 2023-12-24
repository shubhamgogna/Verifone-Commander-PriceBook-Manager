// -----------------------------------------------------------------------
// <copyright file="SapphireClient.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Microsoft.Extensions.Logging;
    using VerifoneCommander.PriceBookManager.Core.Models;

    public class SapphireClient : ISapphireClient, IDisposable
    {
        private const string CgiBinNaxmlPath = "cgi-bin/NAXML";

        private readonly HttpClientHandler httpClientHandler;
        private readonly HttpClient httpClient;

        private readonly Uri cgiBinNaxmlRequestUri;
        private readonly string userName;
        private readonly string password;
        private readonly ILogger logger;

        private string cookie;

        public SapphireClient(
            Uri baseUri,
            string userName,
            string password,
            ILogger logger)
        {
            if (baseUri is null)
            {
                throw new ArgumentNullException(nameof(baseUri));
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException($"'{nameof(userName)}' cannot be null or empty", nameof(userName));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty", nameof(password));
            }

            this.httpClientHandler = new HttpClientHandler()
            {
                // Required for custom certificate generated for the POS systems
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true,
            };
            this.httpClient = new HttpClient(this.httpClientHandler);

            this.cgiBinNaxmlRequestUri = new Uri(baseUri.AbsoluteUri + CgiBinNaxmlPath);
            this.userName = userName;
            this.password = password;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<Plu>> GetPriceLookUpsAsync()
        {
            const int PageSize = 1_000_000; // 1 million

            await this.EnsureValidCookie().ConfigureAwait(false);
            var content = $@"cmd=vPLUs&cookie={this.cookie}

<domain:PLUSelect xmlns:domain=""urn:vfi-sapphire:np.domain.2001-07-01"">
  <pageSize>{PageSize}</pageSize>
  <page>1</page>
</domain:PLUSelect>
";
            using var request = this.CreateRequest(content);
            var responseContent = await this.SendRequestAndEnsureSuccessAsync(request).ConfigureAwait(false);
            var doc = XDocument.Parse(responseContent);
            var list = doc.Descendants(SapphireXNames.Plu)
                .Select(element =>
                {
                    try
                    {
                        return ModelConverter.ConvertXmlToPlu(element);
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError(ex.ToString());
                        return null;
                    }
                })
                .Where(x => x != null)
                .ToList();

            return list;
        }

        public async Task UpdatePriceLookUpAsync(Plu plu)
        {
            _ = plu ?? throw new ArgumentNullException(nameof(plu));

            var documentElement = new XElement(
                SapphireXNames.Plus,
                new XAttribute(SapphireXNames.DomainNamespace, SapphireXNames.DomainNamespaceName),
                new XAttribute("page", "1"),
                new XAttribute("ofPages", "1"),
                ModelConverter.ConvertPluToXml(plu));

            await this.EnsureValidCookie().ConfigureAwait(false);
            var content = $@"cmd=uPLUs&cookie={this.cookie}

{documentElement}"; // Note: Space is required

            using var request = this.CreateRequest(content);
            await this.SendRequestAndEnsureSuccessAsync(request).ConfigureAwait(false);
        }

        public async Task DeletePriceLookUpAsync(long ean13, int modifier)
        {
            var documentElement = new XElement(
                SapphireXNames.Plus,
                new XAttribute(SapphireXNames.DomainNamespace, SapphireXNames.DomainNamespaceName),
                new XAttribute("page", "1"),
                new XAttribute("ofPages", "1"),
                new XElement(
                    "deletePLU",
                    new XElement(
                        "upc",
                        new XAttribute("source", "keyboard"),
                        ean13),
                    new XElement(
                        "upcModifier",
                        modifier.ToString("D3"))));

            await this.EnsureValidCookie().ConfigureAwait(false);
            var content = $@"cmd=uPLUs&cookie={this.cookie}

{documentElement}"; // Note: Space is required

            using var request = this.CreateRequest(content);
            await this.SendRequestAndEnsureSuccessAsync(request).ConfigureAwait(false);
        }

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            await this.EnsureValidCookie().ConfigureAwait(false);
            using var request = this.CreateRequest($"cmd=vposcfg&cookie={this.cookie}");
            var responseContent = await this.SendRequestAndEnsureSuccessAsync(request).ConfigureAwait(false);
            var doc = XDocument.Parse(responseContent);
            var list = doc.Descendants(SapphireXNames.Department)
                .Select(element =>
                 {
                     try
                     {
                         return ModelConverter.ConvertXmlToDepartment(element);
                     }
                     catch (Exception ex)
                     {
                         this.logger.LogError(ex.ToString());
                         return null;
                     }
                 })
                .Where(x => x != null)
                .ToList();

            return list;
        }

        public async Task<List<TaxRate>> GetTaxRatesAsync()
        {
            await this.EnsureValidCookie().ConfigureAwait(false);
            using var request = this.CreateRequest($"cmd=vpaymentcfg&cookie={this.cookie}");
            var responseContent = await this.SendRequestAndEnsureSuccessAsync(request).ConfigureAwait(false);
            var doc = XDocument.Parse(responseContent);
            var list = doc.Descendants("taxRate")
                .Select(element =>
                {
                    try
                    {
                        return ModelConverter.ConvertXmlToTaxRate(element);
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError(ex.ToString());
                        return null;
                    }
                })
                .Where(x => x != null)
                .ToList();

            return list;
        }

        public async Task<List<AgeValidation>> GetAgeValidationsAsync()
        {
            await this.EnsureValidCookie().ConfigureAwait(false);
            using var request = this.CreateRequest($"cmd=vrefinteg&dataset=ageValidations&cookie={this.cookie}");
            var responseContent = await this.SendRequestAndEnsureSuccessAsync(request).ConfigureAwait(false);
            var doc = XDocument.Parse(responseContent);
            var list = doc.Descendants(SapphireXNames.AgeValidation)
                .Select(element =>
                {
                    try
                    {
                        return ModelConverter.ConvertXmlToAgeValidation(element);
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError(ex.ToString());
                        return null;
                    }
                })
                .Where(x => x != null)
                .ToList();

            return list;
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

        private async Task EnsureValidCookie()
        {
            if (!string.IsNullOrEmpty(this.cookie))
            {
                return;
            }

            using var request = this.CreateRequest($"cmd=validate&user={this.userName}&passwd={this.password}");
            var responseContent = await this.SendRequestAndEnsureSuccessAsync(request).ConfigureAwait(false);
            var doc = XDocument.Parse(responseContent);

            this.cookie = doc.Descendants("cookie").First().Value;
        }

        private HttpRequestMessage CreateRequest(string content)
        {
            return new HttpRequestMessage(HttpMethod.Post, this.cgiBinNaxmlRequestUri)
            {
                Content = new StringContent(content, Encoding.UTF8, "text/plain"),
            };
        }

        private async Task<string> SendRequestAndEnsureSuccessAsync(HttpRequestMessage request)
        {
            using var response = await this.httpClient.SendAsync(request).ConfigureAwait(false);
            var responseContent = response.Content != null ? await response.Content.ReadAsStringAsync().ConfigureAwait(false) : string.Empty;

            if (!response.IsSuccessStatusCode ||
                responseContent.Contains("VFI:Fault") ||
                responseContent.Contains("faultCode") ||
                responseContent.Contains("faultString"))
            {
                this.LogUnexpectedResponse(request, response, responseContent);
                throw new HttpRequestException(responseContent);
            }

            return responseContent;
        }

        private void LogUnexpectedResponse(HttpRequestMessage request, HttpResponseMessage response, string responseContent)
        {
            var message = string.Empty;
            message += $"{(int)response.StatusCode} '{response.ReasonPhrase}' {request.Method} {request.RequestUri}";
            message += Environment.NewLine;

            foreach (var headerPair in response.Headers)
            {
                message += $"{headerPair.Key}: {string.Join(",", headerPair.Value)}";
                message += Environment.NewLine;
            }

            message += responseContent;
            message += Environment.NewLine;

            this.logger.LogError(message);
        }
    }
}

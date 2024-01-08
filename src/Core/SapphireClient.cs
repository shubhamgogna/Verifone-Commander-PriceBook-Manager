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
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Microsoft.Extensions.Logging;
    using VerifoneCommander.PriceBookManager.Core.Models;

    public class SapphireClient : ISapphireClient
    {
        private readonly IHttpRequestSender httpRequestSender;
        private readonly ISapphireCredentialsProvider credentialProvider;
        private readonly ILogger logger;

        public SapphireClient(
            IHttpRequestSender httpRequestSender,
            ISapphireCredentialsProvider credentialProvider,
            ILogger<SapphireClient> logger)
        {
            this.httpRequestSender = httpRequestSender ?? throw new ArgumentNullException(nameof(httpRequestSender));
            this.credentialProvider = credentialProvider ?? throw new ArgumentNullException(nameof(credentialProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<Plu>> GetPriceLookUpsAsync(
            CancellationToken cancellationToken)
        {
            const int PageSize = 1_000_000; // 1 million

            var body = $@"
<domain:PLUSelect xmlns:domain=""urn:vfi-sapphire:np.domain.2001-07-01"">
  <pageSize>{PageSize}</pageSize>
  <page>1</page>
</domain:PLUSelect>
";

            var responseContent = await this.SendAsync(
                cmdHeader: "cmd=vPLUs",
                body: body,
                cancellationToken: cancellationToken).ConfigureAwait(false);

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

        public async Task UpdatePriceLookUpAsync(
            Plu plu,
            CancellationToken cancellationToken)
        {
            _ = plu ?? throw new ArgumentNullException(nameof(plu));

            var element = new XElement(
                SapphireXNames.Plus,
                new XAttribute(SapphireXNames.DomainNamespace, SapphireXNames.DomainNamespaceName),
                new XAttribute("page", "1"),
                new XAttribute("ofPages", "1"),
                ModelConverter.ConvertPluToXml(plu));

            await this.SendAsync(
                cmdHeader: "cmd=uPLUs",
                body: element.ToString(),
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task DeletePriceLookUpAsync(
            long ean13,
            int modifier,
            CancellationToken cancellationToken)
        {
            var element = new XElement(
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

            await this.SendAsync(
                cmdHeader: "cmd=uPLUs",
                body: element.ToString(),
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<Department>> GetDepartmentsAsync(
            CancellationToken cancellationToken)
        {
            var responseContent = await this.SendAsync(
                cmdHeader: "cmd=vposcfg",
                cancellationToken: cancellationToken).ConfigureAwait(false);

            var doc = XDocument.Parse(responseContent);
            var list = doc
                .Descendants(SapphireXNames.Department)
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

        public async Task<List<TaxRate>> GetTaxRatesAsync(
            CancellationToken cancellationToken)
        {
            var responseContent = await this.SendAsync(
                cmdHeader: "cmd=vpaymentcfg",
                cancellationToken: cancellationToken).ConfigureAwait(false);

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

        public async Task<List<AgeValidation>> GetAgeValidationsAsync(
            CancellationToken cancellationToken)
        {
            var responseContent = await this.SendAsync(
                cmdHeader: "cmd=vrefinteg&dataset=ageValidations",
                cancellationToken: cancellationToken).ConfigureAwait(false);

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

        private Task<string> SendAsync(
                    string cmdHeader,
                    CancellationToken cancellationToken)
        {
            return this.SendAsync(
                cmdHeader: cmdHeader,
                body: null,
                cancellationToken: cancellationToken);
        }

        private async Task<string> SendAsync(
            string cmdHeader,
            string body,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(cmdHeader))
            {
                throw new ArgumentException($"'{nameof(cmdHeader)}' cannot be null or whitespace.", nameof(cmdHeader));
            }

            var credentials = await this.credentialProvider.GetCredentialsAsync(
                cancellationToken).ConfigureAwait(false);

            var requestContent = GetRequestBody(cmdHeader, body, credentials.Cookie);

            using var request = SapphireHttpUtil.CreateRequest(
                credentials.NaxmlRequestUri,
                requestContent);

            using var response = await this.httpRequestSender.SendAsync(
                request,
                cancellationToken).ConfigureAwait(false);

            var responseContent = await SapphireHttpUtil.ReadResponseContentAsync(
                response,
                cancellationToken).ConfigureAwait(false);

            if (SapphireHttpUtil.IsUnsuccessfulResponse(
                response: response,
                requestContent: GetRequestBody(cmdHeader, body, "[REDACTED_COOKIE]"),
                responseContent: responseContent,
                logger: this.logger))
            {
                throw new SapphireRequestException(responseContent);
            }

            return responseContent;

            static string GetRequestBody(
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
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="SapphireHttpUtil.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    internal static class SapphireHttpUtil
    {
        public static async Task<string> ReadResponseContentAsync(
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

        public static HttpRequestMessage CreateRequest(
            Uri requestUri,
            string content)
        {
            return new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(content, Encoding.UTF8, "text/plain"),
            };
        }

        public static bool IsUnsuccessfulResponse(
            HttpResponseMessage response,
            string requestContent,
            string responseContent,
            ILogger logger)
        {
            if (!response.IsSuccessStatusCode ||
                responseContent.Contains("VFI:Fault") ||
                responseContent.Contains("faultCode") ||
                responseContent.Contains("faultString"))
            {
                logger.LogError(
                    "Request with '{requestContent}' failed with '{httpResponseCode}' '{httpResponsePhrase}' and '{httpResponseContent}'",
                    requestContent,
                    response.StatusCode,
                    response.ReasonPhrase,
                    responseContent);

                return true;
            }

            return false;
        }
    }
}

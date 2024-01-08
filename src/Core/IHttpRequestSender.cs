// -----------------------------------------------------------------------
// <copyright file="IHttpRequestSender.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IHttpRequestSender
    {
        Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken);
    }
}

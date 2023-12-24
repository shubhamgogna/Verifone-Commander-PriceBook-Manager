// -----------------------------------------------------------------------
// <copyright file="ISapphireHttpClient.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISapphireHttpClient : IDisposable
    {
        Task<string> SendAsync(
            string cmdHeader,
            CancellationToken cancellationToken);

        Task<string> SendAsync(
            string cmdHeader,
            string body,
            CancellationToken cancellationToken);
    }
}

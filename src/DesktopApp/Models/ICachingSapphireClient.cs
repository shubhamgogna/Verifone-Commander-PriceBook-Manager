// -----------------------------------------------------------------------
// <copyright file="ICachingSapphireClient.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.Models
{
    using System.Threading;
    using System.Threading.Tasks;
    using VerifoneCommander.PriceBookManager.Core;

    public interface ICachingSapphireClient : ISapphireClient
    {
        public Task RefreshCacheAsync(
            CancellationToken cancellationToken);
    }
}

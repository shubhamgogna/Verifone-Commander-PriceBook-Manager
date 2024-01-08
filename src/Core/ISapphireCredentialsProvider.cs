// -----------------------------------------------------------------------
// <copyright file="ISapphireCredentialsProvider.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ISapphireCredentialsProvider
    {
        Task<ISapphireCredentials> GetCredentialsAsync(
            CancellationToken cancellationToken);
    }

    public interface IModifiableSapphireCredentialsProvider : ISapphireCredentialsProvider
    {
        void SetLoginCredentials(
            string hostName,
            string username,
            string password);
    }
}

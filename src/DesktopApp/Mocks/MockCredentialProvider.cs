// -----------------------------------------------------------------------
// <copyright file="MockCredentialProvider.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.Mocks
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VerifoneCommander.PriceBookManager.Core;

    public class MockCredentialProvider : IModifiableSapphireCredentialsProvider
    {
        public Task<ISapphireCredentials> GetCredentialsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult<ISapphireCredentials>(new SapphireCredentials
            {
                NaxmlRequestUri = new Uri("https://192.168.31.11/mock"),
                Cookie = "Mock",
            });
        }

        public void SetLoginCredentials(string hostName, string username, string password)
        {
            // Do nothing
        }

        private class SapphireCredentials : ISapphireCredentials
        {
            public Uri NaxmlRequestUri { get; set; }

            public string Cookie { get; set; }
        }
    }
}

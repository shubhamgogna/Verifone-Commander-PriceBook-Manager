// -----------------------------------------------------------------------
// <copyright file="ISapphireCredentials.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core
{
    using System;

    public interface ISapphireCredentials
    {
        Uri NaxmlRequestUri { get; }

        string Cookie { get; }
    }
}

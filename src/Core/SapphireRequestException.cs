// -----------------------------------------------------------------------
// <copyright file="SapphireRequestException.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core
{
    using System;

    public class SapphireRequestException : Exception
    {
        public SapphireRequestException(string message)
            : base(message)
        {
        }
    }
}

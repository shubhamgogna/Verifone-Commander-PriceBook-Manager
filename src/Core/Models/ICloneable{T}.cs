// -----------------------------------------------------------------------
// <copyright file="ICloneable{T}.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Core.Models
{
    public interface ICloneable<T>
    {
        T Clone();
    }
}

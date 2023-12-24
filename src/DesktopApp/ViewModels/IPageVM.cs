// -----------------------------------------------------------------------
// <copyright file="IPageVM.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    public interface IPageVM
    {
        string Name { get; }

        int SymbolCode { get; }
    }
}

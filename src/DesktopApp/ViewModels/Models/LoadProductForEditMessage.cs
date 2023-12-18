// -----------------------------------------------------------------------
// <copyright file="LoadProductForEditMessage.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels.Models
{
    internal class LoadProductForEditMessage
    {
        public LoadProductForEditMessage(long ean13, int modifier)
        {
            this.Ean13 = ean13;
            this.Modifier = modifier;
        }

        public long Ean13 { get; }

        public int Modifier { get; }
    }
}

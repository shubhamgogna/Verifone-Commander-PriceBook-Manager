// -----------------------------------------------------------------------
// <copyright file="Settings.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.Models
{
    public class Settings
    {
        public string Hostname { get; set; }

        public string Username { get; set; }

        public bool Ean13IncludesCheckDigit { get; set; }

        public bool UseMockData { get; set; }
    }
}

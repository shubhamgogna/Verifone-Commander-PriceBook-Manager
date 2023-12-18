// -----------------------------------------------------------------------
// <copyright file="Settings.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.Models
{
    public class Settings
    {
        public string Hostname { get; set; } = "192.168.31.11";

        public string Username { get; set; } = string.Empty;

        public bool UseMocks { get; set; }
    }
}

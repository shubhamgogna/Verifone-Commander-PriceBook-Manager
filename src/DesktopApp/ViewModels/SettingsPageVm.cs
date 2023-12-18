// -----------------------------------------------------------------------
// <copyright file="SettingsPageVm.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    using System;
    using CommunityToolkit.Mvvm.Messaging;
    using Microsoft.Extensions.Logging;
    using VerifoneCommander.PriceBookManager.DesktopApp.Models;

    public class SettingsPageVm : PageVm
    {
        private readonly Settings settings;

        public SettingsPageVm(
            IUiThreadDispatcher uiThreadDispatcher,
            IMessenger messenger,
            ILogger logger,
            Settings settings)
            : base(uiThreadDispatcher, messenger, logger)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

            this.Hostname = new ValidatedTextVm(
                v =>
                {
                    if (string.IsNullOrWhiteSpace(v))
                    {
                        return "Empty hostname not allowed";
                    }

                    // Valid. Update in settings.
                    this.settings.Hostname = v;
                    return string.Empty;
                });

            this.Hostname.Text = this.settings.Hostname;
        }

        public override string Name => "Settings";

        // Settings
        public override int SymbolCode => 0xE115;

        public ValidatedTextVm Hostname { get; }
    }
}

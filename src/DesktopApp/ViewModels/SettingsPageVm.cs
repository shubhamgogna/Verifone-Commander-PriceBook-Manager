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

    public class SettingsPageVm : ViewModelBase, IPageVM
    {
        private readonly Settings settings;

        public SettingsPageVm(
            Settings settings,
            IUiThreadDispatcher uiThreadDispatcher,
            IMessenger messenger,
            ILogger logger)
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
        }

        public string Name => "Settings";

        // Settings
        public int SymbolCode => 0xE115;

        public ValidatedTextVm Hostname { get; }

        public bool Ean13IncludesCheckDigit
        {
            get => this.settings.Ean13IncludesCheckDigit;
            set => this.SetProperty(
                oldValue: this.settings.Ean13IncludesCheckDigit,
                newValue: value,
                model: this.settings,
                callback: (model, val) => model.Ean13IncludesCheckDigit = val);
        }
    }
}

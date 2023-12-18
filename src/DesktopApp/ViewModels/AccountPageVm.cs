// -----------------------------------------------------------------------
// <copyright file="AccountPageVm.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Microsoft.Extensions.Logging;
    using VerifoneCommander.PriceBookManager.Core;
    using VerifoneCommander.PriceBookManager.DesktopApp.Models;
    using VerifoneCommander.PriceBookManager.DesktopApp.ViewModels.Models;

    public partial class AccountPageVm : PageVm
    {
        private readonly Settings settings;
        private readonly IModifiableSapphireCredentialsProvider credentialProvider;
        private readonly ICachingSapphireClient sapphireClient;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsLoggingIn))]
        [NotifyPropertyChangedFor(nameof(IsLoggedIn))]
        [NotifyPropertyChangedFor(nameof(IsLoggedOut))]
        private LoginState loginState = LoginState.LoggedOut;

        [ObservableProperty]
        private ValidatedTextVm username = new ValidatedTextVm(
            v =>
            {
                if (string.IsNullOrWhiteSpace(v))
                {
                    return "Empty username is not allowed";
                }

                return string.Empty;
            });

        [ObservableProperty]
        private ValidatedTextVm password = new ValidatedTextVm(
            v =>
            {
                if (string.IsNullOrWhiteSpace(v))
                {
                    return "Empty password is not allowed";
                }

                return string.Empty;
            });

        [ObservableProperty]
        private string loginError;

        public AccountPageVm(
            IUiThreadDispatcher uiThreadDispatcher,
            IMessenger messenger,
            ILogger logger,
            Settings settings,
            IModifiableSapphireCredentialsProvider credentialProvider,
            ICachingSapphireClient sapphireClient)
            : base(uiThreadDispatcher, messenger, logger)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.credentialProvider = credentialProvider ?? throw new ArgumentNullException(nameof(credentialProvider));
            this.sapphireClient = sapphireClient ?? throw new ArgumentNullException(nameof(sapphireClient));

            this.Username.Text = this.settings.Username;

            this.LoginCommand = new AsyncRelayCommand(this.LoginAsync);
            this.LogoutCommand = new RelayCommand(this.Logout);
        }

        public override string Name => "Account";

        // Contact
        public override int SymbolCode => 0xE77B;

        public ICommand LoginCommand { get; }

        public ICommand LogoutCommand { get; }

        public bool IsLoggingIn => this.LoginState == LoginState.LoggingIn;

        public bool IsLoggedIn => this.LoginState == LoginState.LoggedIn;

        public bool IsLoggedOut => this.LoginState == LoginState.LoggedOut;

        private async Task LoginAsync(
            CancellationToken cancellationToken)
        {
            if (this.LoginState != LoginState.LoggedOut)
            {
                // Do nothing
                return;
            }

            if (this.Username.HasError_Revalidate() ||
                this.Password.HasError_Revalidate())
            {
                return;
            }

            this.credentialProvider.SetLoginCredentials(
                this.settings.Hostname,
                this.Username.Text,
                this.Password.Text);

            await this.DispatchOnUiThreadAsync(() =>
            {
                this.LoginError = string.Empty;
                this.LoginState = LoginState.LoggingIn;
            }).ConfigureAwait(false);

            try
            {
                // Execute an operation that would indicate if login was successful
                await this.sapphireClient.RefreshCacheAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await this.DispatchOnUiThreadAsync(() =>
                {
                    this.LoginError = ex.Message;
                    this.LoginState = LoginState.LoggedOut;
                }).ConfigureAwait(false);

                return;
            }

            await this.DispatchOnUiThreadAsync(() =>
            {
                this.LoginError = string.Empty;
                this.LoginState = LoginState.LoggedIn;

                // Save the current username since it was valid for login
                this.settings.Username = this.Username.Text;
            }).ConfigureAwait(false);
        }

        private void Logout()
        {
            this.LoginState = LoginState.LoggedOut;
        }

        partial void OnLoginStateChanged(LoginState value)
        {
            if (value == LoginState.LoggedIn || value == LoginState.LoggedOut)
            {
                this.Messenger.Send(new LoginStateChangedMessage(value));
            }
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp
{
    using System;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.Messaging;
    using CommunityToolkit.WinUI;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.UI.Dispatching;
    using Microsoft.UI.Xaml;
    using VerifoneCommander.PriceBookManager.Core;
    using VerifoneCommander.PriceBookManager.DesktopApp.Mocks;
    using VerifoneCommander.PriceBookManager.DesktopApp.Models;
    using VerifoneCommander.PriceBookManager.DesktopApp.ViewModels;

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private static bool useMocks = true;

        private Window window;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        /// <remarks>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </remarks>
        public App()
        {
            this.InitializeComponent();

            var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            var uiThreadDispatcher = new UiThreadDispatcher(dispatcherQueue);
            var messenger = WeakReferenceMessenger.Default;
            var logger = NullLogger.Instance;

            IModifiableSapphireCredentialsProvider credentialsProvider;
            ISapphireClient sapphireClient;

            if (useMocks)
            {
                credentialsProvider = new MockCredentialProvider();
                sapphireClient = new MockSapphireClient();
            }
            else
            {
#pragma warning disable CA2000 // Dispose objects before losing scope
                var httpRequestSender = new HttpClientHttpRequestSender();
#pragma warning restore CA2000 // Dispose objects before losing scope

                credentialsProvider = new SapphireCredentialProvider(
                    httpRequestSender,
                    NullLogger<SapphireCredentialProvider>.Instance);

                sapphireClient = new SapphireClient(
                    httpRequestSender,
                    credentialsProvider,
                    NullLogger<SapphireClient>.Instance);
            }

            var cachingSapphireClient = new CachingSapphireClient(sapphireClient);

            var mainNavigationVm = new MainNavigationVm(
                uiThreadDispatcher,
                messenger,
                logger,
                credentialsProvider,
                cachingSapphireClient);

            ViewModelResolver = new AppViewModelResolver(mainNavigationVm);
        }

        public static IAppViewModelResolver ViewModelResolver { get; private set; }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            this.window = new MainWindow();
            this.window.Activate();
        }

        private class UiThreadDispatcher : IUiThreadDispatcher
        {
            private readonly DispatcherQueue dispatcherQueue;

            public UiThreadDispatcher(
                DispatcherQueue dispatcherQueue)
            {
                this.dispatcherQueue = dispatcherQueue ?? throw new ArgumentNullException(nameof(dispatcherQueue));
            }

            public Task DispatchAsync(Action action)
            {
                return this.dispatcherQueue.EnqueueAsync(action);
            }
        }
    }
}

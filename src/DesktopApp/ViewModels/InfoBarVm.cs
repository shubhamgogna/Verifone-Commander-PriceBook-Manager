// -----------------------------------------------------------------------
// <copyright file="InfoBarVm.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class InfoBarVm : ObservableObject
    {
        [ObservableProperty]
        private string message;

        [ObservableProperty]
        private InfoBarSeverity severity = InfoBarSeverity.Informational;

        [ObservableProperty]
        private bool isOpen = false;
    }

#pragma warning disable SA1201 // Elements should appear in the correct order
    public enum InfoBarSeverity
#pragma warning restore SA1201 // Elements should appear in the correct order
    {
        /// <summary>
        /// Informational
        /// </summary>
        Informational = 0,

        /// <summary>
        /// Success
        /// </summary>
        Success = 1,

        /// <summary>
        /// Warning
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Error
        /// </summary>
        Error = 3,
    }
}

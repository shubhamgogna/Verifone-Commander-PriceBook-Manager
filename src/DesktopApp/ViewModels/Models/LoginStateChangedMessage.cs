// -----------------------------------------------------------------------
// <copyright file="LoginStateChangedMessage.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels.Models
{
    internal class LoginStateChangedMessage
    {
        public LoginStateChangedMessage(LoginState state)
        {
            this.State = state;
        }

        public LoginState State { get; }
    }
}

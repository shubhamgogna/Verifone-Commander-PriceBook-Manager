using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    public class AccountPageVM : BindableBase, IPageVM
    {
        public string Name => "Account";

        public int SymbolCode => 57661; // Contact
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    public interface IPageVM
    {
        string Name { get; }

        int SymbolCode { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerifoneCommander.PriceBookManager.DesktopApp.ViewModels
{
    public class MainVM : BindableBase
    {
        private readonly AccountPageVM accountPage = new AccountPageVM();
        private readonly ObservableCollection<IPageVM> pages = new ObservableCollection<IPageVM>();

        public MainVM() 
        { 
            this.pages.Add(this.accountPage);
        }

        public ObservableCollection<IPageVM> Pages => this.pages;
    }
}

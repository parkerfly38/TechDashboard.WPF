using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using TechDashboard.Models;

namespace TechDashboard.ViewModels
{
    public class PartsAddPageViewModel
    {
        ObservableCollection<App_Item> _itemList;
        public ObservableCollection<App_Item> ItemList
        {
            get { return _itemList; }
        }

        App_WorkTicket _workTicket;
        public App_WorkTicket WorkTicket
        {
            get { return _workTicket; }
        }

        public PartsAddPageViewModel(App_WorkTicket workTicket)
        {
            _workTicket = workTicket;
            _itemList = new ObservableCollection<App_Item>(App.Database.GetItems());
        }

        public void FilterItemList(string filterText)
        {
            _itemList.Clear();
            foreach(App_Item newItem in App.Database.GetItems(filterText, filterText))
            {
                _itemList.Add(newItem);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using EasyShopManagement.Class.Models;
namespace EasyShopManagement.Class.Searchs
{
    class CustomerSearch
    {
        public CustomerSearch(ICollectionView filteredList, TextBox textEdit)
        {
            string filterText = string.Empty;
            
            filteredList.Filter = delegate(object obj)
            {
                if (String.IsNullOrEmpty(filterText))
                {
                    return true;
                }
                ModelCustomerIno str = obj as ModelCustomerIno;
                if (str.CustomerName==null)
                {
                    return true;
                }
                if (str.CustomerName.ToUpper().Contains(filterText.ToUpper()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            };
            textEdit.TextChanged += delegate
            {
                filterText = textEdit.Text;
                filteredList.Refresh();
            };
        }
    }
}
